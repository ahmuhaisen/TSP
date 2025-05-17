import { Component, inject } from '@angular/core';
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
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { EventsService } from '../../areas/system-admin-area/services/events.service';
import { EventDetailsDTO } from '../../areas/system-admin-area/api-interfaces/event.types';

type NzStatusType = 'wait' | 'process' | 'finish' | 'error';

@Component({
  selector: 'app-event-request-details',
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
  templateUrl: './event-request-details.component.html',
  styleUrl: './event-request-details.component.css'
})
export class EventRequestDetailsComponent {
  breadcrumbService = inject(BreadcrumbService);
  eventService = inject(EventsService);
  activatedRoute = inject(ActivatedRoute);
  eventDetailsDTO!: EventDetailsDTO;

  tabs = [];

  isEventRequestModalVisible = false;
  constructor(private route: ActivatedRoute) {
    var eventRequestId = this.route.snapshot.paramMap.get('id')!;

    this.eventService.getEventDetails(eventRequestId).subscribe({
      next: data => {
        this.eventDetailsDTO = data;
        this.breadcrumbService.set('@eventName', data.eventName);
      }
    });
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

  // Application History methods
  getApplicationHistoryStep(): number {
    if (!this.eventDetailsDTO) return 0;

    if (this.isEventFullyApproved()) {
      return 3; // All steps completed, event ready
    } else if (this.eventDetailsDTO.isDeanAssistantApproved) {
      return 2; // Dean Assistant approved
    } else if (this.eventDetailsDTO.isAdvisorApproved) {
      return 1; // Advisor approved, waiting for Dean Assistant
    } else {
      return 0; // Only submitted
    }
  }

  getApplicationHistoryStatus(): NzStatusType {
    if (!this.eventDetailsDTO) return 'process';

    if (this.eventDetailsDTO.approvalStatus === 'Approved') {
      return 'finish';
    } else if (this.eventDetailsDTO.approvalStatus === 'Rejected') {
      return 'error';
    } else {
      return 'process';
    }
  }

  getAdvisorApprovalDescription(): string {
    if (!this.eventDetailsDTO) return '';

    if (this.eventDetailsDTO.isAdvisorApproved) {
      return 'Approved by ' + this.eventDetailsDTO.advisor.advisorName;
    } else {
      return 'Waiting for advisor approval.';
    }
  }

  getDeanAssistantApprovalDescription(): string {
    if (!this.eventDetailsDTO) return '';

    if (this.eventDetailsDTO.isDeanAssistantApproved) {
      return 'Approved';
    } else if (this.eventDetailsDTO.isAdvisorApproved) {
      return 'Waiting for Dean Assistant approval.';
    } else {
      return 'Pending advisor approval first.';
    }
  }

  isEventFullyApproved(): boolean {
    if (!this.eventDetailsDTO) return false;

    return this.eventDetailsDTO.isDeanAssistantApproved === true &&
      this.eventDetailsDTO.isAdvisorApproved === true;
  }
}
