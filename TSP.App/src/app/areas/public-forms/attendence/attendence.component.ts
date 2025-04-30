import { ActivatedRoute } from '@angular/router';
import { Component, inject, OnInit, HostListener, AfterViewInit, Renderer2 } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

import { NzFormModule } from 'ng-zorro-antd/form';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzResultModule } from 'ng-zorro-antd/result';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzButtonComponent } from 'ng-zorro-antd/button';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { NzCascaderModule, NzCascaderOption } from 'ng-zorro-antd/cascader';

import { PostAttendance } from './attendance.types';
import { AttendanceService } from './attendance.service';
import { SchoolService } from '../../../common/services/school.service';
import { SchoolWithDepartmentsBasicDetails } from '../../../common/types/system-tables.types';
import { DatePipe, NgIf } from '@angular/common';
import { NzAlertModule } from 'ng-zorro-antd/alert';
import { NzOptionComponent } from 'ng-zorro-antd/select';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { AuthService } from '../../../common/services/auth.service';
import { SecureLocalStorageService } from '../../../common/services/secure-local-storage.service';

// Import model and components
import { EventDetails } from './models/event-details.model';
import { EventHeroComponent } from './components/event-hero/event-hero.component';
import { StickyInfoBarComponent } from './components/sticky-info-bar/sticky-info-bar.component';
import { EventInfoBarComponent } from './components/event-info-bar/event-info-bar.component';
import { EventDetailsComponent } from './components/event-details/event-details.component';
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
    NzButtonComponent,
    NzDividerModule,
    NzCascaderModule,
    NzResultModule,
    NzSpinModule,
    NzAvatarModule,
    ReactiveFormsModule,
    NzCascaderModule,
    NzAlertModule,
    NzOptionComponent,
    NzFormModule,
    NzTagModule,
    DatePipe,
    // Add component imports
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
export class AttendenceComponent implements OnInit, AfterViewInit {
  eventId = '3FA85F64-5717-4562-B3FC-2C963F66AFA6';
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
  isSubmitting = false;
  isRegisterSucceeded = false;
  isFormEnabled = true;
  isLoggedIn = false;
  userDetails: any = null;
  isLoading = false;

  registrationForm: FormGroup;
  registrationType: 'account' | 'anonymous' = 'anonymous';

  // Mock event details - in a real app, this would come from a service
  eventDetails: EventDetails = {
    name: 'Tech Conference 2024',
    type: 'Conference',
    description: 'Join us for an exciting day of technology talks, workshops, and networking opportunities. Learn from industry experts and connect with like-minded professionals.',
    location: 'University Conference Center, Building A',
    date: new Date('2024-06-15'),
    startTime: '09:00 AM',
    endTime: '05:00 PM',
    societyName: 'Computer Science Society',
    societyLogo: '/assets/images/cs-society-logo.png',
    societyDescription: 'The Computer Science Society aims to foster interest in computer science and technology through workshops, events, and networking opportunities for students.'
  };

  // Mock school and major options - in a real app, this would come from a service
  schoolMajorOptions: SchoolMajorOption[] = [
    {
      value: 'engineering',
      label: 'Engineering',
      children: [
        { value: 'computer', label: 'Computer Engineering' },
        { value: 'electrical', label: 'Electrical Engineering' },
        { value: 'mechanical', label: 'Mechanical Engineering' }
      ]
    },
    {
      value: 'science',
      label: 'Science',
      children: [
        { value: 'computer-science', label: 'Computer Science' },
        { value: 'physics', label: 'Physics' },
        { value: 'mathematics', label: 'Mathematics' }
      ]
    }
  ];

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
    });

    if(this.isAttendanceSavedToLocalStorage(this.eventId)) {
      this.isRegisterSucceeded = true;
    }

    this.fetchSchools();
    this.checkAuthStatus();
    
    // Set the default registration type
    // If user is logged in, default to account registration
    if (this.isLoggedIn && this.userDetails) {
      this.registrationType = 'account';
      // Pre-fill form with user details
      setTimeout(() => {
        this.registrationForm.patchValue({
          fullName: this.userDetails?.fullName || '',
          email: this.userDetails?.email || '',
          universityNumber: this.userDetails?.universityNumber || '',
          schoolMajor: []
        });
      }, 0);
    } else {
      this.registrationType = 'anonymous';
    }
  }

  ngAfterViewInit(): void {
    // No need to initialize - the CSS will handle it
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

  checkAuthStatus() {
    // Try to log in using the stored token
    const loginSuccess = this.authService.tryLogIn();
    
    // Check authentication status
    this.isLoggedIn = this.authService.isAuthenticated();
    console.log('User is authenticated:', this.isLoggedIn);
    
    if (this.isLoggedIn) {
      // Get user from auth service's currentUser signal
      const currentUser = this.authService.currentUser();
      console.log('Current user after tryLogIn:', currentUser);
      
      if (currentUser) {
        // Check if the user is a faculty member
        if (this.authService.isFacultyMember()) {
          this.messageService.error('Faculty members are not allowed to register for events');
          this.registrationType = 'anonymous';
          this.isFormEnabled = false;
          return;
        }
        
        // Set userDetails with data from currentUser
        this.userDetails = {
          fullName: currentUser.name,
          email: currentUser.email,
          universityNumber: currentUser.number || currentUser.id,
          schoolName: 'Unknown',
          departmentName: 'Unknown',
          departmentId: currentUser.departmentId
        };
        console.log('User details set:', this.userDetails);
      } else {
        console.warn('Authentication successful but currentUser is still null after tryLogIn');
        // Create a minimal user profile from what we can deduce
        this.userDetails = {
          fullName: 'Logged In User',
          email: 'user@example.com',
          universityNumber: 'Unknown',
          schoolName: 'Unknown',
          departmentName: 'Unknown'
        };
      }
    } else {
      console.warn('User is not authenticated');
      this.userDetails = null;
    }
  }

  onSubmit(): void {
    if (this.registrationType === 'account' && !this.isLoggedIn) {
      this.messageService.error('Please log in to register with your account');
      return;
    }

    // Check if user is a faculty member
    if (this.authService.isFacultyMember()) {
      this.messageService.error('Faculty members are not allowed to register for events');
      return;
    }

    this.isSubmitting = true;
    
    // For account registration, use the user details directly
    if (this.registrationType === 'account' && this.userDetails) {
      const attendanceData = {
        eventId: this.eventId,
        fullName: this.userDetails.fullName,
        email: this.userDetails.email,
        universityNumber: this.userDetails.universityNumber,
        phoneNumber: '',
        departmentId: this.userDetails.departmentId || '', // This would need to be set with actual department ID 
        notes: ''
      };
      
      console.log('Submitting with account details:', attendanceData);
      
      // Simulate API call
      setTimeout(() => {
        this.isSubmitting = false;
        this.isRegisterSucceeded = true;
        this.messageService.success('Registration successful!');
        
        // Save attendance to local storage for this event
        this.saveAttendanceToLocalStorage(this.eventId);
      }, 1500);
      
      return;
    }
    
    // For anonymous registration, validate the form first
    if (this.registrationType === 'anonymous' && !this.registrationForm.valid) {
      Object.values(this.registrationForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
      this.isSubmitting = false;
      return;
    }
    
    // Form is valid, proceed with submission
    const formData = this.registrationForm.getRawValue();
    console.log('Submitting form data:', formData);
    
    // Simulate API call
    setTimeout(() => {
      this.isSubmitting = false;
      this.isRegisterSucceeded = true;
      this.messageService.success('Registration successful!');
      
      // Save attendance to local storage for this event
      this.saveAttendanceToLocalStorage(this.eventId);
    }, 1500);
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
    return {
      eventId: this.eventId,
      fullName: this.registrationForm.value.fullName!,
      email: this.registrationForm.value.email!,
      universityNumber: this.registrationForm.value.universityNumber!,
      phoneNumber: '',
      departmentId: this.registrationForm.value.schoolMajor![1] ?? this.registrationForm.value.schoolMajor![0],
      notes: this.registrationForm.value.notes
    } as PostAttendance
  }

  postAttendance(): void {
    this.isSubmitting = true;
    const postObject = this.getPostAttendanceObject();

    this.attendanceService.post(postObject).subscribe({
      next: _ => {
        this.messageService.success('You have successfully submitted your attendance');
        this.isRegisterSucceeded = true;
        this.saveAttendanceToLocalStorage(this.eventId);
      },
      error: _ => {
        this.isSubmitting = false;
      },
      complete: () => {
        this.registrationForm.reset();
        this.isSubmitting = false;
      }
    });
  }

  saveAttendanceToLocalStorage(eventId: string) {
    // save attendance to local storage, the saved item is an array of events ids which the user successfully registered
    const savedAttendances = localStorage.getItem('savedAttendances');
    if (!savedAttendances){
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
      
      // Small delay to show loading state
      setTimeout(() => {
        // Pre-fill form with user details
        this.registrationForm.patchValue({
          fullName: this.userDetails?.fullName || '',
          email: this.userDetails?.email || '',
          universityNumber: this.userDetails?.universityNumber || '',
          schoolMajor: []
        });
        
        this.isLoading = false;
      }, 300);
    } else {
      // Small delay to show loading state
      setTimeout(() => {
        this.registrationForm.enable();
        this.registrationForm.reset();
        this.isLoading = false;
      }, 300);
    }
  }

  scrollToRegistration() {
    const registrationForm = document.querySelector('.lg\\:col-span-8');
    if (registrationForm) {
      registrationForm.scrollIntoView({ behavior: 'smooth' });
    }
  }

  forceRefreshUserDetails() {
    this.isLoading = true;
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
              this.messageService.error('Faculty members are not allowed to register for events');
              this.registrationType = 'anonymous';
              this.isFormEnabled = false;
              this.isLoading = false;
              return;
            }
            
            this.userDetails = {
              fullName: userInfo.fullName,
              email: userInfo.email,
              universityNumber: userInfo.number,
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
          } else {
            console.warn('Could not fetch user details from API');
            // Fall back to using current user from signal
            const currentUser = this.authService.currentUser();
            
            if (currentUser) {
              // Check if the user is a faculty member
              if (this.authService.isFacultyMember()) {
                this.messageService.error('Faculty members are not allowed to register for events');
                this.registrationType = 'anonymous';
                this.isFormEnabled = false;
                this.isLoading = false;
                return;
              }
              
              this.userDetails = {
                fullName: currentUser.name || 'Unknown',
                email: currentUser.email || 'Unknown',
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
            } else {
              console.warn('Still no current user available after refresh');
              this.userDetails = null;
              this.registrationType = 'anonymous';
            }
          }
          
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error fetching user details:', error);
          this.userDetails = null;
          this.registrationType = 'anonymous';
          this.isLoading = false;
        }
      });
    } else {
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
}

