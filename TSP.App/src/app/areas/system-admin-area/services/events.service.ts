import { inject, Injectable } from '@angular/core';
import { DbService } from '../../../common/services/db.service';
import { AuthService } from '../../../common/services/auth.service';
import { EventDetailsDTO, EventSimpleRequest } from '../api-interfaces/event.types';

@Injectable({
    providedIn: 'root'
})
export class EventsService {
    model = "AdminArea/Events";
    db = inject(DbService);
    authService = inject(AuthService);
    userId = this.authService.currentUser()?.id;
    getEventRequests() {
        return this.db.getRequest<EventSimpleRequest[]>(`${this.model}`);
    }
    getEventDetails(eventRequestId: string) {
        return this.db.getRequest<EventDetailsDTO>(`${this.model}/${eventRequestId}`);
    }
} 