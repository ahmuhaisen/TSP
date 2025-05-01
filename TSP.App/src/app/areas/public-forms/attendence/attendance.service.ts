import { inject, Injectable } from "@angular/core";
import { DbService } from "../../../common/services/db.service";
import { EventBasicDetails, PostAttendance } from "./attendance.types";

@Injectable()
export class AttendanceService {
    model = 'attendees';
    eventModel = 'events';

    db = inject(DbService);

    getEvent(id: string) {
        return this.db.getRequest<EventBasicDetails>(this.getUrlWithId(this.eventModel,id));
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

