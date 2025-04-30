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

    markAsRead(notificationId: string) {
        return this.db.putRequest<any, any>(this.getUrl() + '/' + notificationId + '/mark-as-read', {});
    }

    markAllAsRead() {
        return this.db.putRequest<any, any>(this.getUrl() + '/mark-all-as-read', {});
    }

    private getUrl() {
        return `${this.model}`;
    }
}