import { CommonModule, NgFor, NgIf } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { IGenericNotification } from '../../common/types/notification.types';
import { NotificationHubService } from '../../common/services/notification-hub.service';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { NotificationService } from '../../common/services/notification.service';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { TimeagoModule } from 'ngx-timeago';

@Component({
  selector: 'app-notifications',
  imports: [
    NgIf,
    NzIconModule,
    CommonModule,
    NzDropDownModule,
    NzButtonModule,
    NzIconModule,
    NgIf,
    NgFor,
    TimeagoModule
  ],
  templateUrl: './notifications.component.html',
  styleUrl: './notifications.component.css'
})
export class NotificationsComponent {

  isNotificationsDropdownOpen = signal(false);

  notifications = signal<IGenericNotification[]>([]);
  notificationCount = signal<number>(0);

  notificationHubService = inject(NotificationHubService);
  nzNotificationService = inject(NzNotificationService);
  notificationService = inject(NotificationService);

  ngOnInit() {
    this.fillUserNotifications();

    this.notificationHubService.startConnection();

    this.notificationHubService.onNotification((data) => {
      console.log('Notification received:', data);
      this.nzNotificationService.info(
        data.subject,
        data.body
      );

      this.notifications.update((prev) => {
        return [data, ...prev];
      });

      this.notificationCount.update((prev) => {
        return prev + 1;
      }
      );
    });

  }

  toggleNotificationsDropdown(): void {
    this.isNotificationsDropdownOpen.update((prev) => !prev);
  }

  fillUserNotifications(): void {
    this.notificationService.all().subscribe((data) => {
      console.log('Notifications from DB:', data);
      this.notifications.set(data);
      this.notificationCount.set(data.filter((n) => !n.isSeen).length);
    });
  }

  markAllAsRead(){

    if (this.notificationCount() === 0) {
      return;
    }

    this.notificationService.markAllAsRead().subscribe((data) => {
      console.log('All notifications marked as read:', data);
    });

    this.notifications.update((prev) => {
      return prev.map((notification) => {
        notification.isSeen = true;
        return notification;
      });
    });

    this.notificationCount.set(0);
  }

  markAsRead(notification: IGenericNotification){

    if (notification.isSeen) {
      return;
    }

    this.notificationService.markAsRead(notification.id).subscribe((data) => {
      console.log('Notification marked as read:', data);
    });

    this.notifications.update((prev) => {
      return prev.map((n) => {
        if (n.id === notification.id) {
          n.isSeen = true;
        }
        return n;
      });
    });

    this.notificationCount.update((prev) => {
      return prev - 1;
    });
  }

}
