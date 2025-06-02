import { ActivatedRoute, RouterLink } from '@angular/router';
import { Component, inject, signal } from '@angular/core';

import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';

import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzDrawerModule } from 'ng-zorro-antd/drawer';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { DatePipe, NgIf } from '@angular/common';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';

import { SocietiesService } from '../../../areas/student-area/services/societies.service';
import { MembershipRequestDTO, UpdateMembershipRequest } from '../../../areas/student-area/api-interfaces/membership.types';

@Component({
  selector: 'app-membership-requests',
  imports: [
    DatePipe,
    NzIconModule,
    NzButtonModule,
    NzDividerModule,
    NzModalModule,
    NzTagModule,
    NzDrawerModule,
    NzEmptyModule,
    NzTableModule,
    NzAvatarModule,
    RouterLink,
    NgIf,
  ],
  templateUrl: './membership-requests.component.html',
  styleUrl: './membership-requests.component.css'
})
export class MembershipRequestsComponent {
  activatedRoute = inject(ActivatedRoute);
  societiesService = inject(SocietiesService);
  window = window;

  societyId = signal<string>('');
  isMembershipRequestModalVisible = false;

  membershipRequests: MembershipRequestDTO[] = []

  isRequestDetailsVisible = false;
  selectedRequest: MembershipRequestDTO | null = null;
  
  showSocietyRequests(): void {
    this.isMembershipRequestModalVisible = true;

    this.societiesService.getJoinRequests(this.activatedRoute.snapshot.params['id'] || "notworking")
      .subscribe({
        next: (requests: MembershipRequestDTO[]) => {
          this.membershipRequests = requests;
          console.log(JSON.stringify(this.membershipRequests, null, 2))

        }, error: (error: Error) => {
          console.log(JSON.stringify(error))
        }
      });
  }
  showRequestDetails(request: MembershipRequestDTO): void {
    this.selectedRequest = request;
    this.isRequestDetailsVisible = true;
  }

  acceptRequest(requestId: string): void {

    const request = this.membershipRequests.find(r => r.id === requestId);
    const parsedRequest: UpdateMembershipRequest = {
      SocietyId: this.activatedRoute.snapshot.params['id'],
      MembershipRequestId: request?.id || "",
      isAccepted: true
    }
    this.societiesService.updateMembershipRequests(parsedRequest).subscribe(
      (data) => {
        this.membershipRequests = this.membershipRequests.map(r => {
          if (r.id === requestId) {
            return {
              ...r,
              status: 'Accepted'
            };
          }
          return r;
        });
      });
  }

  rejectRequest(requestId: string): void {
    const request = this.membershipRequests.find(r => r.id === requestId);
    const parsedRequest: UpdateMembershipRequest = {
      SocietyId: this.activatedRoute.snapshot.params['id'],
      MembershipRequestId: request?.id || "",
      isAccepted: false
    }

    this.societiesService.updateMembershipRequests(parsedRequest).subscribe((data) => {
      this.membershipRequests = this.membershipRequests.map(r => {
        if (r.id === requestId) {
          return {
            ...r,
            status: 'Reject'
          };
        }
        return r;
      });
    });

  }
  getStatusColor(status: string): string {
    switch (status) {
      case 'Pending':
        return 'processing';
      case 'Accepted':
        return 'success';
      case 'Reject':
        return 'error';
      default:
        return 'default';
    }
  }

  getPendingRequests(): MembershipRequestDTO[] {
    return this.membershipRequests.filter(request => request.status === 'Pending');
  }

  getApprovedRequests(): MembershipRequestDTO[] {
    return this.membershipRequests.filter(request => request.status === 'Accepted');
  }

  getRejectedRequests(): MembershipRequestDTO[] {
    return this.membershipRequests.filter(request => request.status === 'Reject');
  }

  getProcessedRequests(): MembershipRequestDTO[] {
    return this.membershipRequests.filter(request => 
      request.status === 'Accepted' || request.status === 'Reject'
    );
  }

}
