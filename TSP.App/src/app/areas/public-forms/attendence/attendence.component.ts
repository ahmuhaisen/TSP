import { NgIf } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Component, inject, OnInit, HostListener, AfterViewInit, Renderer2, ViewChild, OnDestroy, signal } from '@angular/core';

import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NzAlertModule } from 'ng-zorro-antd/alert';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { NzResultModule } from 'ng-zorro-antd/result';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzCascaderModule, NzCascaderOption } from 'ng-zorro-antd/cascader';

import { PostAttendance } from './attendance.types';
import { AttendanceService } from './attendance.service';
import { AuthService } from '../../../common/services/auth.service';
import { SchoolService } from '../../../common/services/school.service';
import { EventsService } from '../../system-admin-area/services/events.service';
import { SchoolWithDepartmentsBasicDetails } from '../../../common/types/system-tables.types';
import { SecureLocalStorageService } from '../../../common/services/secure-local-storage.service';

import { EventDetails } from './models/event-details.model';
import { EventHeroComponent } from './components/event-hero/event-hero.component';
import { EventDetailsComponent } from './components/event-details/event-details.component';
import { EventInfoBarComponent } from './components/event-info-bar/event-info-bar.component';
import { StickyInfoBarComponent } from './components/sticky-info-bar/sticky-info-bar.component';
import { RegistrationFormComponent } from './components/registration-form/registration-form.component';

interface SchoolMajorOption {
  value: string;
  label: string;
  children?: SchoolMajorOption[];
}

@Component({
  selector: 'app-attendence',
  standalone: true,
  imports: [
    NgIf,
    NzIconModule,
    NzFormModule,
    NzInputModule,
    NzDividerModule,
    NzCascaderModule,
    NzResultModule,
    NzSpinModule,
    NzAvatarModule,
    ReactiveFormsModule,
    NzCascaderModule,
    NzAlertModule,
    NzFormModule,
    NzTagModule,
    EventHeroComponent,
    StickyInfoBarComponent,
    EventInfoBarComponent,
    EventDetailsComponent,
    RegistrationFormComponent
  ],
  templateUrl: './attendence.component.html',
  providers: [
    AttendanceService
  ]
})
export class AttendenceComponent implements OnInit, AfterViewInit, OnDestroy {
  eventId = '';
  currentYear = new Date().getFullYear();
  nzOptions: NzCascaderOption[] = [];

  fb = inject(FormBuilder);
  activatedRoute = inject(ActivatedRoute);
  schoolService = inject(SchoolService);
  attendanceService = inject(AttendanceService);
  messageService = inject(NzMessageService);
  authService = inject(AuthService);
  renderer = inject(Renderer2);
  localStorageService = inject(SecureLocalStorageService);
  eventsService = inject(EventsService);

  isSubmitting = false;
  isRegisterSucceeded = false;
  isFormEnabled = true;
  isLoggedIn = false;
  userDetails: any = null;
  isLoading = false;
  isEventLoading = true;
  isEventAvailable = signal(false);
  notAvailableMessage = signal('This event is not available anymore');

  registrationForm: FormGroup;
  registrationType: 'account' | 'anonymous' = 'anonymous';

  eventDetails: EventDetails = {} as EventDetails;

  schoolMajorOptions: SchoolMajorOption[] = [];

  @ViewChild(RegistrationFormComponent) registrationFormComponent!: RegistrationFormComponent;

  constructor() {
    this.registrationForm = this.fb.group({
      fullName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      universityNumber: ['', [Validators.required]],
      schoolMajor: [[], [Validators.required]],
      notes: ['']
    });
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.eventId = params.get('eventId')!;

      // Start fetching event details
      this.fetchEventDetails();
    });

    if (this.isAttendanceSavedToLocalStorage(this.eventId)) {
      this.isRegisterSucceeded = true;
    }

    if (this.isRegisterSucceeded)
      return;

    this.fetchSchools();

    this.isLoggedIn = this.authService.isAuthenticated();
    if (this.isLoggedIn) {
      this.registrationType = 'account';

      this.setupAutoRefresh();
    } else {
      this.registrationType = 'anonymous';
    }
  }

  ngAfterViewInit(): void {
    console.log('View initialized, registration form component available:', !!this.registrationFormComponent);

    // If the form component is available and we're using account registration, ensure forms are synced
    if (this.registrationFormComponent && this.registrationType === 'account' && this.userDetails) {
      setTimeout(() => {
        // Ensure the child form is properly filled with user details
        this.registrationFormComponent.registrationForm.patchValue({
          fullName: this.userDetails.fullName,
          email: this.userDetails.email,
          universityNumber: this.userDetails.universityNumber
        });
      });
    }
  }

  @HostListener('window:scroll', [])
  onWindowScroll() {
    const stickyInfoBar = document.getElementById('stickyInfoBar');
    if (!stickyInfoBar) return;

    // Show the sticky header after scrolling down 300px
    if (window.scrollY > 300) {
      this.renderer.setStyle(stickyInfoBar, 'transform', 'translateY(0)');
    } else {
      this.renderer.setStyle(stickyInfoBar, 'transform', 'translateY(-100%)');
    }
  }

  onSubmit(): void {
    console.log('onSubmit called - Starting registration process');
    console.log('Registration type:', this.registrationType);

    if (this.registrationFormComponent) {
      console.log('Child component form valid?', this.registrationFormComponent.registrationForm.valid);
      console.log('Child component form values:', this.registrationFormComponent.registrationForm.value);
    } else {
      console.log('Child component not available');
    }

    if (this.registrationType === 'account' && !this.isLoggedIn) {
      this.messageService.error('Please log in to register with your account');
      console.log('Registration failed: User not logged in for account registration');
      return;
    }

    // Check if user is a faculty member
    if (this.authService.isFacultyMember()) {
      this.messageService.error('Faculty members are not allowed to register for events');
      console.log('Registration failed: Faculty member attempted to register');
      return;
    }

    this.isSubmitting = true;
    console.log('isSubmitting set to true');

    // For account registration, use the user details directly
    if (this.registrationType === 'account' && this.userDetails) {
      const attendanceData: PostAttendance = {
        eventId: this.eventId,
        fullName: this.userDetails.fullName,
        email: this.userDetails.email,
        universityNumber: this.userDetails.universityNumber,
        phoneNumber: '',
        departmentId: this.userDetails.departmentId?.toString() || '', // Ensure it's a string
        notes: ''
      };

      console.log('Submitting with account details:', attendanceData);

      // Call the actual API endpoint
      this.attendanceService.post(attendanceData).subscribe({
        next: _ => {
          console.log('Registration successful (account)');
          this.isRegisterSucceeded = true;
          this.messageService.success('Registration successful!');
          this.saveAttendanceToLocalStorage(this.eventId);
        },
        error: error => {
          console.error('Registration failed (account):', error);
          this.isSubmitting = false;
        },
        complete: () => {
          console.log('Registration request completed (account)');
          this.isSubmitting = false;
        }
      });

      return;
    }

    // For anonymous registration, validate the form first
    if (this.registrationType === 'anonymous') {
      console.log('Form validation for anonymous registration');

      // Check if we have access to the child component form
      if (this.registrationFormComponent && !this.registrationFormComponent.registrationForm.valid) {
        console.log('Child component form is invalid');

        Object.keys(this.registrationFormComponent.registrationForm.controls).forEach(key => {
          const control = this.registrationFormComponent.registrationForm.get(key);
          console.log(`Control "${key}" valid: ${control?.valid}, value: ${JSON.stringify(control?.value)}, errors:`, control?.errors);

          if (control?.invalid) {
            control.markAsDirty();
            control.updateValueAndValidity({ onlySelf: true });
          }
        });

        this.messageService.warning('Please fill in all required fields');
        this.isSubmitting = false;
        return;
      }
    }

    // Form is valid, proceed with submission
    const postObject = this.getPostAttendanceObject();
    console.log('Submitting form data (anonymous):', postObject);

    if (!postObject.departmentId) {
      console.error('Department ID is missing');
      this.messageService.error('Please select a School and Major');
      this.isSubmitting = false;
      return;
    }

    // Call the actual API endpoint
    this.attendanceService.post(postObject).subscribe({
      next: _ => {
        console.log('Registration successful (anonymous)');
        this.isRegisterSucceeded = true;
        this.messageService.success('Registration successful!');
        this.saveAttendanceToLocalStorage(this.eventId);
      },
      error: error => {
        console.error('Registration failed (anonymous):', error);
        this.messageService.error('Registration failed. Please try again.');
        this.isSubmitting = false;
      },
      complete: () => {
        console.log('Registration request completed (anonymous)');
        if (this.registrationFormComponent) {
          this.registrationFormComponent.registrationForm.reset();
        } else {
          this.registrationForm.reset();
        }
        this.isSubmitting = false;
      }
    });
  }

  fetchSchools(): void {
    this.schoolService.allSchoolsWithDepartments().subscribe({
      next: (res) => {
        this.nzOptions = this.convertToCascadeOptions(res);
      }
    });
  }

  convertToCascadeOptions(schools: SchoolWithDepartmentsBasicDetails[]): NzCascaderOption[] {
    return schools.map(school => ({
      value: school.id.toString(),
      label: school.name,
      isLeaf: school.departments.length === 0,
      children: school.departments.map(department => ({
        value: department.id.toString(),
        label: department.name,
        isLeaf: true
      }))
    }));
  }

  getPostAttendanceObject() {
    // Try to get form data directly from the child component
    if (this.registrationFormComponent) {
      console.log('Getting form data directly from child component:',
        this.registrationFormComponent.registrationForm.value);

      const childFormValue = this.registrationFormComponent.registrationForm.value;

      let selectedDepartmentId = '';

      try {
        const schoolMajor = childFormValue.schoolMajor;

        if (Array.isArray(schoolMajor) && schoolMajor.length > 0) {
          selectedDepartmentId = schoolMajor[schoolMajor.length - 1].toString();
        } else if (schoolMajor && typeof schoolMajor === 'string') {
          selectedDepartmentId = schoolMajor;
        }
      } catch (error) {
        console.error('Error parsing department ID from child form:', error);
      }

      return {
        eventId: this.eventId,
        fullName: childFormValue.fullName || '',
        email: childFormValue.email || '',
        universityNumber: childFormValue.universityNumber || '',
        phoneNumber: '',
        departmentId: selectedDepartmentId,
        notes: childFormValue.notes || ''
      } as PostAttendance;
    }

    try {
      const storedFormData = sessionStorage.getItem('registrationFormData');
      if (storedFormData) {
        const formData = JSON.parse(storedFormData);
        console.log('Retrieved form data from sessionStorage:', formData);

        let selectedDepartmentId = '';

        try {
          const schoolMajor = formData.schoolMajor;

          if (Array.isArray(schoolMajor) && schoolMajor.length > 0) {
            selectedDepartmentId = schoolMajor[schoolMajor.length - 1].toString();
          } else if (schoolMajor && typeof schoolMajor === 'string') {
            selectedDepartmentId = schoolMajor;
          }
        } catch (error) {
          console.error('Error parsing department ID from sessionStorage:', error);
        }

        sessionStorage.removeItem('registrationFormData');

        return {
          eventId: this.eventId,
          fullName: formData.fullName || '',
          email: formData.email || '',
          universityNumber: formData.universityNumber || '',
          phoneNumber: '',
          departmentId: selectedDepartmentId,
          notes: formData.notes || ''
        } as PostAttendance;
      }
    } catch (e) {
      console.error('Error reading form data from sessionStorage:', e);
    }

    let selectedDepartmentId = '';

    try {
      const schoolMajor = this.registrationForm.value.schoolMajor;

      if (Array.isArray(schoolMajor) && schoolMajor.length > 0) {
        // Last item in the array should be the department ID
        selectedDepartmentId = schoolMajor[schoolMajor.length - 1].toString();
      } else if (schoolMajor && typeof schoolMajor === 'string') {
        // If it's directly a string value
        selectedDepartmentId = schoolMajor;
      }
    } catch (error) {
      console.error('Error parsing department ID from form controls:', error);
    }

    return {
      eventId: this.eventId,
      fullName: this.registrationForm.value.fullName || '',
      email: this.registrationForm.value.email || '',
      universityNumber: this.registrationForm.value.universityNumber || '',
      phoneNumber: '',
      departmentId: selectedDepartmentId,
      notes: this.registrationForm.value.notes || ''
    } as PostAttendance
  }

  saveAttendanceToLocalStorage(eventId: string) {
    // save attendance to local storage, the saved item is an array of events ids which the user successfully registered
    const savedAttendances = localStorage.getItem('savedAttendances');
    if (!savedAttendances) {
      localStorage.setItem('savedAttendances', JSON.stringify([eventId]));
      return;
    }

    const parsedSavedAttendances = JSON.parse(savedAttendances);
    parsedSavedAttendances.push(eventId);
    localStorage.setItem('savedAttendances', JSON.stringify(parsedSavedAttendances));
  }

  isAttendanceSavedToLocalStorage(eventId: string): boolean {
    const savedAttendances = localStorage.getItem('savedAttendances');
    if (!savedAttendances) return false;
    const parsedSavedAttendances = JSON.parse(savedAttendances);
    return parsedSavedAttendances.includes(eventId);
  }

  // Set up auto refresh interval for user details when using account registration
  private setupAutoRefresh() {
    // Clear any existing interval
    if (this.refreshIntervalId) {
      clearInterval(this.refreshIntervalId);
      this.refreshIntervalId = null;
    }

    // If using account registration and logged in, set up auto refresh
    if (this.registrationType === 'account' && this.isLoggedIn) {
      // Refresh immediately and then every 30 seconds
      this.forceRefreshUserDetails();

      // Set up interval for auto refresh
      this.refreshIntervalId = setInterval(() => {
        if (this.registrationType === 'account' && this.isLoggedIn && !this.isSubmitting) {
          console.log('Auto-refreshing user details...');
          this.forceRefreshUserDetails(true); // true = silent refresh (no UI indicators)
        }
      }, 30000); // 30 seconds
    }
  }

  // Variable to store the refresh interval ID
  private refreshIntervalId: any = null;

  ngOnDestroy() {
    // Clear refresh interval on component destruction
    if (this.refreshIntervalId) {
      clearInterval(this.refreshIntervalId);
    }
  }

  selectRegistrationType(type: 'account' | 'anonymous') {
    if (this.registrationType === type) return; // Don't do anything if it's already selected

    this.registrationType = type;
    this.isLoading = true;

    if (type === 'account') {
      // Check if user is logged in
      if (!this.isLoggedIn) {
        this.messageService.error('Please log in to register with your account');
        this.registrationType = 'anonymous';
        this.isLoading = false;
        return;
      }

      // Set up automatic refresh of user details
      this.setupAutoRefresh();
    } else {
      // Clear auto refresh interval if switching to anonymous
      if (this.refreshIntervalId) {
        clearInterval(this.refreshIntervalId);
        this.refreshIntervalId = null;
      }

      // Small delay to show loading state
      setTimeout(() => {
        this.registrationForm.reset();
        this.registrationForm.enable();
        // Reset validation state
        this.registrationForm.markAsUntouched();
        this.registrationForm.updateValueAndValidity();
        this.isLoading = false;
      }, 300);
    }
  }

  forceRefreshUserDetails(silent: boolean = false) {
    if (!silent) {
      this.isLoading = true;
    }
    console.log('Force refreshing user details...');

    // Check current authentication status
    this.isLoggedIn = this.authService.isAuthenticated();
    console.log('Authentication check result:', this.isLoggedIn);

    if (this.isLoggedIn) {
      // Directly fetch the current user info from the API
      this.authService.fetchCurrentUserInfo().subscribe({
        next: (userInfo) => {
          if (userInfo) {
            console.log('Fetched user details:', userInfo);

            // Check if the user is a faculty member
            if (userInfo.userType?.toUpperCase() === 'FACULTY') {
              if (!silent) {
                this.messageService.error('Faculty members are not allowed to register for events');
              }
              this.registrationType = 'anonymous';
              this.isFormEnabled = false;
              this.isLoading = false;
              return;
            }

            this.userDetails = {
              fullName: userInfo.fullName || 'Unknown',
              email: userInfo.email || 'Unknown@Unknown',
              universityNumber: userInfo.number || 'Unknown',
              schoolName: 'Unknown',
              departmentName: 'Unknown',
              departmentId: userInfo.departmentId
            };

            // Update the registration type
            this.registrationType = 'account';
            this.isFormEnabled = true;

            // Pre-fill form
            this.registrationForm.patchValue({
              fullName: this.userDetails.fullName,
              email: this.userDetails.email,
              universityNumber: this.userDetails.universityNumber,
              // If we have departmentId, we could try to select the correct department in schoolMajor
              // but that would require mapping the departmentId to the proper option structure
            });

            // If we have the child component reference, update its form too
            if (this.registrationFormComponent) {
              this.registrationFormComponent.registrationForm.patchValue({
                fullName: this.userDetails.fullName,
                email: this.userDetails.email,
                universityNumber: this.userDetails.universityNumber
              });
            }

          } else {
            console.warn('Could not fetch user details from API');
            // Fall back to using current user from signal
            const currentUser = this.authService.currentUser();

            if (currentUser) {
              // Check if the user is a faculty member
              if (this.authService.isFacultyMember()) {
                if (!silent) {
                  this.messageService.error('Faculty members are not allowed to register for events');
                }
                this.registrationType = 'anonymous';
                this.isFormEnabled = false;
                this.isLoading = false;
                return;
              }

              this.userDetails = {
                fullName: currentUser.name || 'Unknown',
                email: currentUser.email || 'Unknown@Unknown',
                universityNumber: currentUser.number || currentUser.id || 'Unknown',
                schoolName: 'Unknown',
                departmentName: 'Unknown',
                departmentId: currentUser.departmentId
              };

              // Update the registration type
              this.registrationType = 'account';
              this.isFormEnabled = true;

              // Pre-fill form
              this.registrationForm.patchValue({
                fullName: this.userDetails.fullName,
                email: this.userDetails.email,
                universityNumber: this.userDetails.universityNumber
              });

              // If we have the child component reference, update its form too
              if (this.registrationFormComponent) {
                this.registrationFormComponent.registrationForm.patchValue({
                  fullName: this.userDetails.fullName,
                  email: this.userDetails.email,
                  universityNumber: this.userDetails.universityNumber
                });
              }
            } else {
              console.warn('Still no current user available after refresh');
              if (!silent) {
                this.messageService.error('Could not load user information. Please try logging in again.');
              }
              this.userDetails = null;
              this.registrationType = 'anonymous';
            }
          }

          this.isLoading = false;

          // Reset validation state
          this.registrationForm.markAsUntouched();
          this.registrationForm.updateValueAndValidity();

          if (this.registrationFormComponent) {
            this.registrationFormComponent.registrationForm.markAsUntouched();
            this.registrationFormComponent.registrationForm.updateValueAndValidity();
          }
        },
        error: (error) => {
          console.error('Error fetching user details:', error);
          if (!silent) {
            this.messageService.error('Failed to load user information. Please try again.');
          }
          this.userDetails = null;
          this.registrationType = 'anonymous';
          this.isLoading = false;
        }
      });
    } else {
      if (!silent) {
        this.messageService.warning('You are not logged in. Please log in to use account registration.');
      }
      this.userDetails = null;
      this.registrationType = 'anonymous';
      this.isLoading = false;
    }
  }

  /**
   * Get the initials from the society name (first letter of first and second word)
   * @param societyName The society name to extract initials from
   * @returns The initials (up to 2 characters)
   */
  getSocietyInitials(societyName: string): string {
    if (!societyName) return 'EO'; // Default to "EO" for "Event Organizer"

    // Split the name into words
    const words = societyName.split(' ').filter(word => word.length > 0);

    if (words.length === 0) return 'EO';

    // Get first letter of first word
    const firstInitial = words[0][0].toUpperCase();

    // If there's a second word, get its first letter too
    if (words.length > 1) {
      const secondInitial = words[1][0].toUpperCase();
      return firstInitial + secondInitial;
    }

    // If only one word, return the first letter only
    return firstInitial;
  }

  // Fetch event details from the API
  fetchEventDetails() {
    this.isEventLoading = true;

    this.attendanceService.getEventDetails(this.eventId).subscribe({
      next: (eventDetails) => {
        this.isEventLoading = false;
        console.log('Event details fetched:', eventDetails);

        // Check if the event is not ended
        const currentDate = new Date();
        const eventEndDate = new Date(eventDetails.endDateTime);
        if (eventEndDate < currentDate) {
          this.isEventAvailable.set(false);
          this.notAvailableMessage.set('Event has ended.');
          return;
        }

        // check if the event has IsAttendeesFormEnabled or not
        console.log('isAttendeesFormEnabled', eventDetails.eventRequestDTO?.isAttendeesFormEnabled);
        if (!eventDetails.eventRequestDTO.isAttendeesFormEnabled) {
          this.isEventAvailable.set(false);
          this.notAvailableMessage.set('Event is not available for registration.');
          return;
        }

        // Map the API response to our EventDetails model
        this.eventDetails = {
          name: eventDetails.eventName || 'Event',
          type: eventDetails.type || 'N/A',
          description: eventDetails.eventDescription || 'No description available',
          location: eventDetails.locationString || 'TBD',
          date: new Date(eventDetails.startDateTime),
          startTime: eventDetails.eventRequestDTO?.startTime ? new Date(eventDetails.eventRequestDTO.startTime) : new Date(eventDetails.startDateTime),
          endTime: eventDetails.eventRequestDTO?.endTime ? new Date(eventDetails.eventRequestDTO.endTime) : new Date(eventDetails.endDateTime),
          societyName: eventDetails.eventSociety?.societyName || 'Event Organizer',
          societyLogo: eventDetails.eventSociety?.societyLogoId ? `/api/files/${eventDetails.eventSociety.societyLogoId}` : '',
          societyDescription: eventDetails.eventSociety?.societyDescription || 'No description available',
          eventManagerEmail: eventDetails.eventManager.studentEmail || 'tsp@ju.edu.jo'
        };

        this.isEventAvailable.set(true);
      },
      error: (error) => {
        console.error('Error fetching event details:', error);

        this.isEventAvailable.set(false);
        this.notAvailableMessage.set('Event not found.');

        this.isEventLoading = false;
      }
    });
  }
}

