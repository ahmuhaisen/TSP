import { NgIf } from '@angular/common';
import { Component } from '@angular/core';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NotificationLineComponent } from "./notification-line/notification-line.component";
import { INotification } from '../../../../../common/types/notification.types';

@Component({
  selector: 'app-notifications',
  imports: [
    NgIf,
    NzIconModule,
    NotificationLineComponent
],
  templateUrl: './notifications.component.html',
  styleUrl: './notifications.component.css'
})
export class NotificationsComponent {
  isNotificationsDropdownOpen = false;

  toggleNotificationsDropdown(): void {
    this.isNotificationsDropdownOpen = !this.isNotificationsDropdownOpen;
  }

  notifications: INotification[] = [
    {
      username: 'Ahmad Alhawamdeh',
      message: 'hi Dr. Sami Sarhan, A new event has been created, check it out now!',
      date: new Date(),
      image: 'https://robohash.org/Ahmad@ju.edu.jo?bgset=bg1',
      link: 'events'
    },
    {
      username: 'Suhaib Saleh',
      message: 'a new society has been created',
      date: new Date(),
      image: 'https://robohash.org/Suhaib@ju.edu.jo?bgset=bg2',
      link: 'societies'
    },
    {
      username: 'Sameer Ibrahim',
      message: 'a new event has been created',
      date: new Date(),
      image: 'https://robohash.org/Sameer@ju.edu.jo?bgset=bg1',
      link: 'events'
    }
  ];
}
