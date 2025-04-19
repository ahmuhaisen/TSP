import { inject, Injectable } from "@angular/core";
import { DbService } from "./db.service";
import { IGenericNotification } from "../types/notification.types";

@Injectable({ providedIn: 'root' })
export class NotificationService {
    model = 'notifications';

    db = inject(DbService);

    all() {
        return this.db.getRequest<IGenericNotification[]>(this.model);
    }

    private getUrl() {
        return `${this.model}`;
    }
}