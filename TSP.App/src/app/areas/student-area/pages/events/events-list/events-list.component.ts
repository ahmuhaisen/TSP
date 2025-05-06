import { CommonModule, NgClass, NgStyle } from '@angular/common';
import { Component, inject, HostListener } from '@angular/core';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { DisabledTimeFn, DisabledTimePartial, NzDatePickerModule, NzRangePickerComponent } from 'ng-zorro-antd/date-picker';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzStepsModule } from 'ng-zorro-antd/steps';
import { differenceInCalendarDays, setHours } from 'date-fns';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzSwitchModule } from 'ng-zorro-antd/switch';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NzTabsModule } from 'ng-zorro-antd/tabs';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzCalendarModule } from 'ng-zorro-antd/calendar';
import { NzBadgeModule } from 'ng-zorro-antd/badge';
import { NzDrawerModule } from 'ng-zorro-antd/drawer';
import { MemberAssociatedSociety } from '../../../api-interfaces/society.types';
import { StudentsService } from '../../../services/students.service';
import { AddEventRequest, MemberEventDetailsDTO } from '../../../api-interfaces/event.types';
import { EventsService } from '../../../services/events.service';

@Component({
  selector: 'app-events-list',
  standalone: true,
  imports: [
    NzFormModule,
    NzInputModule,
    NzSelectModule,
    NzSwitchModule,
    NzDatePickerModule,
    NzIconModule,
    NzButtonModule,
    NzDividerModule,
    NzModalModule,
    NzStepsModule,
    CommonModule,
    NzToolTipModule,
    ReactiveFormsModule,
    NzTabsModule,
    NzTagModule,
    NzEmptyModule,
    NzCardModule,
    NzCalendarModule,
    NzBadgeModule,
    NzDrawerModule,
    FormsModule
  ],
  providers: [EventsService],
  templateUrl: './events-list.component.html',
  styleUrl: './events-list.component.css',
  styles: [`
    ::ng-deep .date-time-dropdown {
      max-width: calc(100vw - 32px);
    }
    ::ng-deep .ant-picker-dropdown, ::ng-deep .ant-picker-panel-container {
      max-width: 100%;
    }
    ::ng-deep .ant-picker-panels {
      flex-wrap: wrap;
      gap: 8px;
    }
  `]
})
export class EventsListComponent {

  isEventRequestModalVisible = false;
  currentStep = 0;
  selectedSocietyId: string = "";
  studentsService = inject(StudentsService);
  messageService = inject(NzMessageService);
  eventsService = inject(EventsService);


  committeeSocieties: MemberAssociatedSociety[] = [];
  today = new Date();
  timeDefaultValue = setHours(new Date(), 8);
  currentDate = new Date();

  disabledDate = (current: Date): boolean => {
    const currentMonth = this.currentDate.getMonth();
    const currentYear = this.currentDate.getFullYear();
    return current.getMonth() !== currentMonth || current.getFullYear() !== currentYear;
  };

  disabledRangeTime: DisabledTimeFn = (_value, type?: DisabledTimePartial) => {
    return {
      nzDisabledHours: () => this.range(0, 8), // Disable hours before 8 AM and after 8 PM
      nzDisabledMinutes: () => [], // Allow selecting all minutes
      nzDisabledSeconds: () => this.range(1, 60) // Remove seconds selection
    };
  };

  range(start: number, end: number): number[] {
    const result: number[] = [];
    for (let i = start; i < end; i++) {
      result.push(i);
    }
    return result;
  }

  allNumbersNotDividedBy5(): number[] {
    const result: number[] = [];
    for (let i = 0; i < 60; i++) {
      if (i % 5 !== 0) {
        result.push(i);
      }
    }
    return result;
  }

  eventRequestForm!: FormGroup;
  private fb = inject(FormBuilder);

  upcomingEvents = [
    {
      id: 1,
      title: 'Introduction to AI Workshop',
      society: 'IEEE CS JU',
      startDate: new Date(new Date().getFullYear(), new Date().getMonth(), 15, 10, 0),
      endDate: new Date(new Date().getFullYear(), new Date().getMonth(), 15, 12, 0),
      location: 'Progressoft Lab',
      type: 'Workshop',
      description: 'Learn the basics of Artificial Intelligence and its applications.',
      hasAttendanceForm: true
    },
    {
      id: 10,
      title: '.NET Workshop',
      society: 'ACM University of Jordan Student Chapter',
      startDate: new Date(new Date().getFullYear(), new Date().getMonth(), 15, 10, 0),
      endDate: new Date(new Date().getFullYear(), new Date().getMonth(), 15, 12, 0),
      location: 'Progressoft Lab',
      type: 'Workshop',
      description: 'Learn the basics of Artificial Intelligence and its applications.',
      hasAttendanceForm: true
    },
    {
      id: 2,
      title: 'Web Development Bootcamp',
      society: 'ACM University of Jordan Student Chapter',
      startDate: new Date(new Date().getFullYear(), new Date().getMonth(), 17, 9, 0),
      endDate: new Date(new Date().getFullYear(), new Date().getMonth(), 17, 15, 0),
      location: 'Computer Lab 3',
      type: 'Training',
      description: 'Intensive bootcamp covering modern web development technologies including React',
      hasAttendanceForm: true
    },
    {
      id: 3,
      title: 'Competitive Programming Contest',
      society: 'ACM University of Jordan Student Chapter',
      startDate: new Date(new Date().getFullYear(), new Date().getMonth(), 20, 13, 0),
      endDate: new Date(new Date().getFullYear(), new Date().getMonth(), 20, 17, 0),
      location: 'King Hussein School for Computing',
      type: 'Contest',
      description: 'Monthly programming contest to enhance problem-solving skills.',
      hasAttendanceForm: true
    },
    {
      id: 4,
      title: 'Cloud Computing Seminar',
      society: 'IEEE CS JU',
      startDate: new Date(new Date().getFullYear(), new Date().getMonth(), 22, 11, 0),
      endDate: new Date(new Date().getFullYear(), new Date().getMonth(), 22, 13, 0),
      location: 'Engineering Auditorium',
      type: 'Seminar',
      description: 'Learn about cloud computing platforms and their applications in modern software development.',
      hasAttendanceForm: true
    },
    {
      id: 5,
      title: 'Mobile App Development Workshop',
      society: 'Waves JU',
      startDate: new Date(new Date().getFullYear(), new Date().getMonth(), 25, 10, 0),
      endDate: new Date(new Date().getFullYear(), new Date().getMonth(), 25, 14, 0),
      location: 'Innovation Lab',
      type: 'Workshop',
      description: 'Hands-on workshop on building mobile applications using Flutter.',
      hasAttendanceForm: false
    },
    {
      id: 6,
      title: 'Tech Talk: Future of AI',
      society: 'IEEE CS JU',
      startDate: new Date(new Date().getFullYear(), new Date().getMonth(), 28, 12, 0),
      endDate: new Date(new Date().getFullYear(), new Date().getMonth(), 28, 14, 0),
      location: 'Engineering Conference Room',
      type: 'Conference',
      description: 'Industry experts discuss the future of AI and its impact on various sectors.',
      hasAttendanceForm: true
    },
    {
      id: 6,
      title: 'Tech Talk: Future of AI',
      society: 'IEEE CS JU',
      startDate: new Date(new Date().getFullYear(), new Date().getMonth(), 28, 12, 0),
      endDate: new Date(new Date().getFullYear(), new Date().getMonth(), 28, 14, 0),
      location: 'Engineering Conference Room',
      type: 'Conference',
      description: 'Industry experts discuss the future of AI and its impact on various sectors.',
      hasAttendanceForm: true
    }
  ];

  eventRequests: MemberEventDetailsDTO[] = [];
  selectedEvent: any = null;
  isEventDetailsVisible = false;
  isEventModalVisible = false;
  selectedDate: Date | null = null;
  viewType: 'calendar' | 'cards' = 'calendar'; // Default to calendar view

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    if (window.innerWidth < 640) { // Adjust the width as needed
      this.viewType = 'cards'; // Force table view on small screens
    }
  }

  constructor() {
    this.initForm();
    this.eventsService.getCommitteeEventsRequests().subscribe(data => {
      this.eventRequests = data
      
    })

  }

  private initForm(): void {
    this.eventRequestForm = this.fb.group({
      societyId: [null, Validators.required],
      title: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      description: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(500)]],
      location: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      eventType: ['', Validators.required],
      dateRange: [null, Validators.required],
      hasAttendanceForm: [false]
    });
  }

  openEventRequestModal() {
    // Set default start time to next day at 9:00 AM
    const startDate = new Date();
    startDate.setDate(startDate.getDate() + 1);
    startDate.setHours(9, 0, 0, 0);

    // Set default end time to same day at 11:00 AM
    const endDate = new Date(startDate);
    endDate.setHours(11, 0, 0, 0);

    this.eventRequestForm.patchValue({
      dateRange: [startDate, endDate]
    });
    this.studentsService.getCommitteeSocieties().subscribe(
      data => this.committeeSocieties = data
    )
    console.log(this.committeeSocieties)
    this.isEventRequestModalVisible = true;
  }

  handleEventRequestModalCancel() {
    this.isEventRequestModalVisible = false;
    this.resetForm();
  }

  pre(): void {
    this.currentStep -= 1;
    //this.changeContent();
  }

  next(): void {
    if (this.currentStep === 0) {
      if (!this.eventRequestForm.get('societyId')?.valid) {
        this.messageService.error('Please select a society');
        return;
      }
    }
    this.currentStep += 1;
    //this.changeContent();
  }

  done(): void {
    if (this.eventRequestForm.valid) {
      const formValue = this.eventRequestForm.value;

      const eventRequest: AddEventRequest = {
        societyId: formValue.societyId,
        committeeId: "",
        title: formValue.title,
        description: formValue.description,
        location: formValue.location,
        type: formValue.eventType,
        startDate: formValue.dateRange[0],
        endDate: formValue.dateRange[1],
        isAttendanceFormEnabled: formValue.hasAttendanceForm
      };
      this.eventsService.postEvent(eventRequest).subscribe();
      console.log('Event Request:', eventRequest);

      this.isEventRequestModalVisible = false;
      this.messageService.success('Event request submitted successfully');
      this.resetForm();
    } else {
      Object.keys(this.eventRequestForm.controls).forEach(key => {
        const control = this.eventRequestForm.get(key);
        if (control?.invalid) {
          control.markAsTouched();
        }
      });
      this.messageService.error('Please fill all required fields correctly');
    }
  }

  selectSociety(societyId: string) {
    this.selectedSocietyId = societyId;
    this.eventRequestForm.patchValue({ societyId });
  }

  private resetForm(): void {
    this.currentStep = 0;
    this.selectedSocietyId = "";
    this.eventRequestForm.reset();
    this.initForm();
  }

  getStatusColor(status: string): string {
    const statusColors = {
      pending_advisor: 'gold',
      pending_dean: 'blue',
      approved: 'success',
      rejected: 'error'
    };
    return statusColors[status as keyof typeof statusColors] || 'default';
  }

  getStatusText(status: string): string {
    const statusTexts = {
      pending_advisor: 'Pending Advisor Approval',
      pending_dean: 'Pending Dean Approval',
      approved: 'Approved',
      rejected: 'Rejected'
    };
    return statusTexts[status as keyof typeof statusTexts] || status;
  }

  getDayEvents(date: Date): any[] {
    if (!date) return []; // Return an empty array if date is null
    return this.upcomingEvents.filter(event => {
      const eventDate = new Date(event.startDate);
      return eventDate.getDate() === date.getDate() &&
        eventDate.getMonth() === date.getMonth() &&
        eventDate.getFullYear() === date.getFullYear();
    });
  }

  getMonthData(date: Date): number | null {
    const events = this.upcomingEvents.filter(event => {
      const eventDate = new Date(event.startDate);
      return eventDate.getMonth() === date.getMonth() &&
        eventDate.getFullYear() === date.getFullYear();
    });
    return events.length || null;
  }

  showEventDetails(event: any): void {
    console.log('Selected Event:', event);
    this.selectedEvent = { ...event };
    this.isEventDetailsVisible = true;
    // Force change detection
    setTimeout(() => {
      console.log('Selected Event in state:', this.selectedEvent);
    }, 0);
  }

  closeEventDetails(): void {
    this.selectedEvent = null;
    this.isEventDetailsVisible = false;
  }

  getEventBadgeStatus(eventDate: Date): string {
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    const eventDay = new Date(eventDate);
    eventDay.setHours(0, 0, 0, 0);

    if (eventDay.getTime() === today.getTime()) {
      return 'success';  // green for today
    } else if (eventDay < today) {
      return 'warning';  // orange for past events
    } else {
      return 'processing';  // blue for upcoming events
    }
  }

  isToday(date: Date): boolean {
    const today = new Date();
    return date.getDate() === today.getDate() &&
      date.getMonth() === today.getMonth() &&
      date.getFullYear() === today.getFullYear();
  }

  openEventModal(date: Date): void {
    this.selectedDate = date;
    this.isEventModalVisible = true;
  }

  closeEventModal(): void {
    this.isEventModalVisible = false;
    this.selectedDate = null;
  }

  setViewType(type: 'calendar' | 'cards'): void {
    this.viewType = type;
  }

  registerForEvent(event: any): void {
    // Implement your registration logic here
    console.log('Registering for event:', event);
    // You can also show a success message or perform any other action
  }

}
