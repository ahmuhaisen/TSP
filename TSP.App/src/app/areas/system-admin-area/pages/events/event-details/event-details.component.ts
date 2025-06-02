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
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { BreadcrumbService } from 'xng-breadcrumb';
import { EventsService } from '../../../services/events.service';
import { ActivatedRoute, Router } from '@angular/router';
import { EventDetailsDTO } from '../../../api-interfaces/event.types';
import { CommonModule } from '@angular/common';
import { EventFeedbackService } from '../../../../public-forms/event-feedback/event-feedback.service';
import { NzMessageService } from 'ng-zorro-antd/message';
import { EventRequestDetailsComponent } from "../../../../../components/event-request-details/event-request-details.component";
import { LoaderService } from '../../../../../common/services/loader.service';

// NzStatusType is used in nzStatus
type NzStatusType = 'wait' | 'process' | 'finish' | 'error';

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
    NzDropDownModule,
    NzMenuModule,
    CommonModule,
    EventRequestDetailsComponent
],
  templateUrl: './event-details.component.html',
  styleUrl: './event-details.component.css'
})
export class EventDetailsComponent implements OnInit {

  breadcrumbService = inject(BreadcrumbService);
  eventService = inject(EventsService);
  activatedRoute = inject(ActivatedRoute);
  eventDetailsDTO!: EventDetailsDTO;
  feedbackService = inject(EventFeedbackService);
  nzMessageService = inject(NzMessageService);
  loaderService = inject(LoaderService);
  router = inject(Router);
  route = inject(ActivatedRoute);

  tabs = [];

  isEventRequestModalVisible = false;
  ngOnInit() {
    var eventRequestId = this.route.snapshot.paramMap.get('id')!;
    console.log(eventRequestId)

    this.loaderService.show();
    this.eventService.getEventDetails(eventRequestId).subscribe({
      next: data =>{
        this.eventDetailsDTO = data;
        this.loaderService.hide();
        this.breadcrumbService.set('@eventName', 'Junior to Solver 6.0');
      },
      error: _ => {
        this.loaderService.hide();
        this.nzMessageService.error('Failed to load event details');
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

  redirectToFeedbackQrLinkView() {
    const isFeedbackFormOpen = false;

    this.feedbackService.isFeedbackOpen(this.eventDetailsDTO.id).subscribe({
      next: res => {
        if (res) {
          const link = `public-forms/event-feedback/${this.eventDetailsDTO.id}`;
          const isInternal = true;
          const description = `Feedback for ${this.eventDetailsDTO.eventName}`;

          const queryParams = new URLSearchParams({
            link: link,
            isInternal: String(isInternal),
            description: description
          });
  
          const fullUrl = `${window.location.origin}/qr-viewer?${queryParams.toString()}`;
  
          window.open(fullUrl, '_blank');
        }
        else {
          this.nzMessageService.info('The feedback form is not available yet');
        }
      }
    })
  }

  navigateToFeedbackSummary() {
    this.router.navigate(['feedback-summary'], { relativeTo: this.activatedRoute });
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
