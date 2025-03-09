import { inject, Injectable } from '@angular/core';
import { DbService } from '../../../common/services/db.service';
import { AuthService } from '../../../common/services/auth.service';
import { eventSimpleRequest } from '../api-interfaces/event.types';

@Injectable({
    providedIn: 'root'
})
export class EventsService {
    model = "";
    db = inject(DbService);
    authService = inject(AuthService);
    userId = this.authService.currentUser()?.id;
    getEventRequests(){
        return this.db.getRequest<eventSimpleRequest[]>('{model}');
    }
}