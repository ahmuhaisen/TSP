import { ActivatedRoute, RouterLink } from '@angular/router';
import { Component, inject, signal } from '@angular/core';

import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';

import { PageMode } from '../../../../../common/types/presentaion.types';
import { GenSocietyDetailsComponent } from '../../../../../components/society-details/gen-society-details.component';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzTabComponent } from 'ng-zorro-antd/tabs';
import { NzDrawerModule } from 'ng-zorro-antd/drawer';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { DatePipe, NgFor, NgIf } from '@angular/common';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { MembershipRequestDTO, UpdateMembershipRequest } from '../../../api-interfaces/membership.types';
import { SocietiesService } from '../../../services/societies.service';
@Component({
  selector: 'app-society-details',
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
    NgFor,
    GenSocietyDetailsComponent
  ],
  templateUrl: './society-details.component.html',
  styleUrl: './society-details.component.css'
})
export class SocietyDetailsComponent {
  activatedRoute = inject(ActivatedRoute);

  pageMode = signal<PageMode>('VIEW_ONLY');
  societyId = signal<string>('');
  isMembershipRequestModalVisible = false;

  window = window; // Make window available to template
  constructor(
    private socitiesService: SocietiesService
  ) { }
  ngOnInit() {
    this.activatedRoute.url.subscribe(url => {
      this.societyId.set(this.activatedRoute.snapshot.params['id']);
      if (url.some(u => u.path === 'manage')) {
        this.pageMode.set('STUDENT_MANAGE');
      }
      else {
        this.pageMode.set('VIEW_ONLY');
      }
    })
  }



  membershipRequests: MembershipRequestDTO[] = []

  isRequestDetailsVisible = false;
  selectedRequest: MembershipRequestDTO | null = null;
  showSocietyRequests(): void {
    this.isMembershipRequestModalVisible = true;

    this.socitiesService.getJoinRequests(this.activatedRoute.snapshot.params['id'] || "notworking")
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
    this.socitiesService.updateMembershipRequests(parsedRequest).subscribe();
  }

  rejectRequest(requestId: string): void {
    const request = this.membershipRequests.find(r => r.id === requestId);
    const parsedRequest: UpdateMembershipRequest = {
      SocietyId: this.activatedRoute.snapshot.params['id'],
      MembershipRequestId: request?.id || "",
      isAccepted: false
    }

    this.socitiesService.updateMembershipRequests(parsedRequest).subscribe();

  }
}

