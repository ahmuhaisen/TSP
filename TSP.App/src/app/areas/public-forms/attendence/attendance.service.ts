import { inject, Injectable } from "@angular/core";
import { DbService } from "../../../common/services/db.service";
import { PostAttendance } from "./attendance.types";
import { EventDetailsDTO } from "../../system-admin-area/api-interfaces/event.types";

@Injectable({
    providedIn: 'root'
})
export class AttendanceService {
    model = 'attendees';
    eventModel = 'events';

    db = inject(DbService);

    getEventDetails(eventRequestId: string) {
        return this.db.getRequest<EventDetailsDTO>(`${this.model}/events/${eventRequestId}`);
    }


    getAttendance(eventRequestId: string) {
        return this.db.getRequest<AttendanceLine[]>(`${this.model}?eventId=${eventRequestId}`);
    }

    post(attendance: PostAttendance) {
        console.log('AttendanceService.post called with:', attendance);
        return this.db.postRequest<any, PostAttendance>(this.model, attendance);
    }


    private getUrl() {
        return `${this.model}`;
    }

    private getUrlWithId(model: string, id: string) {
        return `${model}/${id}`;
    }
}

export interface AttendanceLine{
    fullName: string;
    email: string;
    universityNumber: string;
    departmentName: string;
    notes: string;
}