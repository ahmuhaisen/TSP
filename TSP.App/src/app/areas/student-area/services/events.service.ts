import { DbService } from "../../../common/services/db.service";
import { inject } from "@angular/core";
import { AddEventRequest, MemberEventDetailsDTO, EventSimpleDTO } from "../api-interfaces/event.types";
import { AuthService } from "../../../common/services/auth.service";
import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root'
})
export class EventsService {
    private db = inject(DbService);
    authService = inject(AuthService);
    userId = this.authService.currentUser()?.id;

    model = "StudentArea/Events";
    postEvent(addEventRequest: AddEventRequest) {
        addEventRequest.committeeId = this.userId || "";
        return this.db.postRequest<string, AddEventRequest>(`${this.model}/Requests`, addEventRequest);
    }
    getCommitteeEventsRequests() {
        return this.db.getRequest<MemberEventDetailsDTO[]>(`${this.model}/Requests`);
    }
    getEventsByMonth() {
        const date = new Date();
        const _date = date.getFullYear() +
            '-' + String(date.getMonth() + 1).padStart(2, '0') +
            '-' + String(date.getDate()).padStart(2, '0');
        return this.db.getRequest<EventSimpleDTO[]>(`${this.model}?date=${_date}`);
    }

}