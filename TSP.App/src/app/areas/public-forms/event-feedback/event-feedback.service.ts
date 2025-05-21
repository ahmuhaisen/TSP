import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { DbService } from '../../../common/services/db.service';
import { EventDetailsDTO } from '../../system-admin-area/api-interfaces/event.types';

export interface EventFeedback {
  eventId: string;
  rating: number;
  notes: string;
}

export interface EventFeedbackSummary {
  event: {
    id: string;
    name: string;
  };
  summary: {
    summaryId: string;
    averageRating: number;
    totalResponses: number;
    sentiment: string;
    topics: string;
    aiSummary: string;
    calculatedAt: string;
  };
  feedbacks: Array<{
    rating: number;
    notes: string;
    submittedAt: string;
  }>;
}

@Injectable({
    providedIn: 'root'
})
export class EventFeedbackService {
  model = 'feedback';
  
  private db = inject(DbService);

  isFeedbackOpen(eventId: string){
    return this.db.getRequest<boolean>(`${this.model}/is-open/${eventId}`);
  }
  
  submitFeedback(feedback: EventFeedback): Observable<any> {
    return this.db.postRequest<any, EventFeedback>(this.model, feedback);
  }

  getEventDetails(eventRequestId: string) {
    return this.db.getRequest<EventDetailsDTO>(`${this.model}/events/${eventRequestId}`);
  }
  
  getEventFeedbackSummary(eventId: string) {
    return this.db.getRequest<EventFeedbackSummary>(`${this.model}/events/${eventId}/summary`);
  }
} 