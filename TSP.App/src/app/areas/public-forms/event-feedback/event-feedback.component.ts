import { NgIf } from '@angular/common';
import { DatePipe } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Component, inject, OnInit, signal } from '@angular/core';

import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NzAlertModule } from 'ng-zorro-antd/alert';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzRateModule } from 'ng-zorro-antd/rate';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzButtonModule } from 'ng-zorro-antd/button';

import { EventsService } from '../../system-admin-area/services/events.service';
import { EventDetailsDTO } from '../../system-admin-area/api-interfaces/event.types';
import { EventFeedbackService } from './event-feedback.service';
import { EventDetails } from '../attendence/models/event-details.model';

// Import attendance components
import { EventHeroComponent } from '../attendence/components/event-hero/event-hero.component';
import { StickyInfoBarComponent } from '../attendence/components/sticky-info-bar/sticky-info-bar.component';
import { EventInfoBarComponent } from '../attendence/components/event-info-bar/event-info-bar.component';
import { EventDetailsComponent } from '../attendence/components/event-details/event-details.component';

@Component({
  selector: 'app-event-feedback',
  standalone: true,
  imports: [
    NgIf,
    DatePipe,
    NzIconModule,
    NzFormModule,
    NzInputModule,
    NzSpinModule,
    NzAlertModule,
    NzRateModule,
    NzTagModule,
    NzButtonModule,
    ReactiveFormsModule,
    // Add attendance components
    EventHeroComponent,
    StickyInfoBarComponent,
    EventInfoBarComponent,
    EventDetailsComponent
  ],
  templateUrl: './event-feedback.component.html',
  providers: [
    EventFeedbackService
  ]
})
export class EventFeedbackComponent implements OnInit {
  eventId = '';
  currentYear = new Date().getFullYear();
  
  fb = inject(FormBuilder);
  activatedRoute = inject(ActivatedRoute);
  messageService = inject(NzMessageService);
  eventsService = inject(EventsService);
  feedbackService = inject(EventFeedbackService);
  
  isSubmitting = false;
  isSubmitSucceeded = false;
  isEventLoading = true;
  _isEventAvailable = signal(false);
  _notAvailableMessage = 'This event is not available anymore';
  
  feedbackForm: FormGroup;
  eventDetailsDTO: EventDetailsDTO = {} as EventDetailsDTO;
  eventDetails: EventDetails = {} as EventDetails;
  
  constructor() {
    this.feedbackForm = this.fb.group({
      rating: [0, [Validators.required, Validators.min(1), Validators.max(5)]],
      notes: ['']
    });
  }
  
  isEventAvailable(): boolean {
    return this._isEventAvailable();
  }
  
  notAvailableMessage(): string {
    return this._notAvailableMessage;
  }
  
  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.eventId = params.get('eventId')!;
      
      this.checkFeedbackFormAvailability();
    });
  }

  checkFeedbackFormAvailability() {
    this.isEventLoading = true;
    this.feedbackService.isFeedbackOpen(this.eventId).subscribe({
      next: (response) => {
        this.isEventLoading = false;
        this._isEventAvailable.set(response);
        this._notAvailableMessage= 'Feedback form is not available for this event';
        
        // If feedback is available, fetch event details
        if (response) {
          this.fetchEventDetails();
        }
      },
      error: _ => {
        this.isEventLoading = false;
        this._isEventAvailable.set(false);
        this._notAvailableMessage = 'Event not found or no longer available';
      }
    });
  }

  fetchEventDetails(): void {
    this.isEventLoading = true;
    
    this.feedbackService.getEventDetails(this.eventId).subscribe({
      next: (response) => {
        this.eventDetailsDTO = response;
        
        // Transform DTO to EventDetails format for the reused components
        this.mapDtoToEventDetails();
        
        this.isEventLoading = false;
        this._isEventAvailable.set(true);
      },
      error: (error) => {
        console.error('Error fetching event details:', error);
        this.isEventLoading = false;
        this._isEventAvailable.set(false);
        this._notAvailableMessage = 'Event not found or no longer available';
      }
    });
  }
  
  mapDtoToEventDetails(): void {
    const startDate = new Date(this.eventDetailsDTO.startDateTime);
    const endDate = new Date(this.eventDetailsDTO.endDateTime);
    
    this.eventDetails = {
      name: this.eventDetailsDTO.eventName,
      description: this.eventDetailsDTO.eventDescription,
      location: this.eventDetailsDTO.locationString,
      type: this.eventDetailsDTO.type || 'Event',
      date: startDate,
      startTime: startDate,
      endTime: endDate,
      societyName: this.eventDetailsDTO.eventSociety?.societyName,
      societyDescription: this.eventDetailsDTO.eventSociety?.societyDescription
    };
  }
  
  onSubmit(): void {
    if (this.feedbackForm.invalid) {
      // Validate all form fields
      Object.values(this.feedbackForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity();
        }
      });
      return;
    }
    
    this.isSubmitting = true;
    
    const feedbackData = {
      eventId: this.eventId,
      rating: this.feedbackForm.value.rating,
      notes: this.feedbackForm.value.notes || ''
    };

    console.table(feedbackData)
    
    this.feedbackService.submitFeedback(feedbackData).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.isSubmitSucceeded = true;
        this.messageService.success('Thank you for your feedback!');
      },
      error: (error) => {
        this.isSubmitting = false;
        console.error('Error submitting feedback:', error);
        this.messageService.error('Failed to submit feedback. Please try again.');
      }
    });
  }
} 