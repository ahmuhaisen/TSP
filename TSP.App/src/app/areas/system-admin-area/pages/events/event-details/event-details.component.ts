import { Component, inject, OnInit } from '@angular/core';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { NzTabsModule } from 'ng-zorro-antd/tabs';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzStepsModule } from 'ng-zorro-antd/steps';
import { BreadcrumbService } from 'xng-breadcrumb';
import { EventsService } from '../../../services/events.service';
import { ActivatedRoute } from '@angular/router';
import { EventDetailsDTO } from '../../../api-interfaces/event.types';
import { CommonModule } from '@angular/common';
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
    NzTagModule,
    NzStepsModule,
    CommonModule,
  ],
  templateUrl: './event-details.component.html',
  styleUrl: './event-details.component.css'
})
export class EventDetailsComponent implements OnInit {

  breadcrumbService = inject(BreadcrumbService);
  eventService = inject(EventsService);
  activatedRoute = inject(ActivatedRoute);
  eventDetailsDTO!: EventDetailsDTO;


  tabs = [];

  isEventRequestModalVisible = false;
  constructor(private route: ActivatedRoute) {
    var eventRequestId = this.route.snapshot.paramMap.get('id')!;
    console.log(eventRequestId)
    this.eventService.getEventDetails(eventRequestId).subscribe(
      data => this.eventDetailsDTO = data
    );
  }
  ngOnInit() {

    this.breadcrumbService.set('@eventName', 'Junior to Solver 6.0');
  }

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
