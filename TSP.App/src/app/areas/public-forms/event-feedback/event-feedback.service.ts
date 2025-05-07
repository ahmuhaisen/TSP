import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { DbService } from '../../../common/services/db.service';
import { EventDetailsDTO } from '../../system-admin-area/api-interfaces/event.types';

export interface EventFeedback {
  eventId: string;
  rating: number;
  notes: string;
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
} 