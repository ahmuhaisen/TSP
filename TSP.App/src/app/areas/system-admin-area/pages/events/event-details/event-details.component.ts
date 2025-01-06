import { Component } from '@angular/core';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { NzTabsModule } from 'ng-zorro-antd/tabs';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { NzTagModule } from 'ng-zorro-antd/tag';

@Component({
  selector: 'app-event-details',
  imports: [
    NzIconModule,
    NzBreadCrumbModule,
    NzButtonModule,
    NzDividerModule,
    NzAvatarModule,
    NzTabsModule,
    NzModalModule,
    NzEmptyModule,
    NzTagModule
  ],
  templateUrl: './event-details.component.html',
  styleUrl: './event-details.component.css'
})
export class EventDetailsComponent {
  tabs = [
    {
      name: 'Description',
      icon: 'info-circle',
      content: `Great college programmers 🤓
      It's time to learn about many cool, fun and exciting things in our major and our college as well 🙆
      In an event titled Junior to Solver 🤓
      We will introduce you to us, Acm, and talk more about our majors and expand a little on Problem Solving, and talk about its benefits and importance to the job market in the presence of our distinguished guests 😍
      Meet us on Sunday from 12 - 1 in the Ahmed Al-Louzi Auditorium at King Abdullah II College of Information Technology.
      In addition to a competition in the lab after the event to live the atmosphere of competitions 🔥
      There will be sweet prizes, don't miss them`
    },
    {
      name: 'Participants',
      icon: 'team',
      content: 'The event is not approved yet!'
    },
    {
      name: 'Event manager',
      icon: 'user',
      content: '**** Event manager / requester info ****'
    },
  ];

  isEventRequestModalVisible = false;

  showEventRequestModal(): void {
    this.isEventRequestModalVisible = true;
  }

  handleEventRequestModalCancel() {
    this.isEventRequestModalVisible = false;
  }

  handleEventRequestModalOk() {
    this.isEventRequestModalVisible = false;
  }
}
