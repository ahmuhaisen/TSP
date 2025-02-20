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

  ngOnInit() {
    this.activatedRoute.url.subscribe(url => {
      this.societyId.set(this.activatedRoute.snapshot.params['id']);
      if(url.some(u => u.path === 'manage')){
        this.pageMode.set('STUDENT_MANAGE');
      }
      else{
        this.pageMode.set('VIEW_ONLY');
      }
    })
  }



  membershipRequests: MembershipRequest[] = [
    {
      id: '1',
      studentName: 'John Smith',
      studentId: 'STD001',
      section: 'Programming Team',
      requestedOn: new Date('2024-03-20T14:30:00'),
      reason: 'I have been coding for 2 years and would love to contribute to the programming team. I have experience in Python and JavaScript, and Im eager to learn from other members.',
      status: 'PENDING',
      profilePictureUrl: 'https://api.dicebear.com/7.x/avataaars/svg?seed=John'
    },
    {
      id: '2',
      studentName: 'Sarah Johnson',
      studentId: 'STD002',
      section: 'Design Team',
      requestedOn: new Date('2024-03-19T09:15:00'),
      reason: 'I am passionate about UI/UX design and have completed several courses on web design. I would like to help improve the society\'s digital presence.',
      status: 'PENDING',
      profilePictureUrl: 'https://api.dicebear.com/7.x/avataaars/svg?seed=Sarah'
    },
    {
      id: '3',
      studentName: 'Michael Chen',
      studentId: 'STD003',
      section: 'Events Team',
      requestedOn: new Date('2024-03-18T16:45:00'),
      reason: 'I have experience organizing college events and would love to help plan and execute society activities. I am good at coordination and have strong communication skills.',
      status: 'PENDING',
      profilePictureUrl: 'https://api.dicebear.com/7.x/avataaars/svg?seed=Michael'
    },
    {
      id: '4',
      studentName: 'Sarah Johnson',
      studentId: 'STD002',
      section: 'Design Team',
      requestedOn: new Date('2024-03-19T09:15:00'),
      reason: 'I am passionate about UI/UX design and have completed several courses on web design. I would like to help improve the society\'s digital presence.',
      status: 'PENDING',
      profilePictureUrl: 'https://api.dicebear.com/7.x/avataaars/svg?seed=Sarah'
    },
    {
      id: '5',
      studentName: 'Michael Chen',
      studentId: 'STD003',
      section: 'Events Team',
      requestedOn: new Date('2024-03-18T16:45:00'),
      reason: 'I have experience organizing college events and would love to help plan and execute society activities. I am good at coordination and have strong communication skills.',
      status: 'PENDING',
      profilePictureUrl: 'https://api.dicebear.com/7.x/avataaars/svg?seed=Michael'
    },
    {
      id: '6',
      studentName: 'Sarah Johnson',
      studentId: 'STD002',
      section: 'Design Team',
      requestedOn: new Date('2024-03-19T09:15:00'),
      reason: 'I am passionate about UI/UX design and have completed several courses on web design. I would like to help improve the society\'s digital presence.',
      status: 'PENDING',
      profilePictureUrl: 'https://api.dicebear.com/7.x/avataaars/svg?seed=Sarah'
    },
    {
      id: '7',
      studentName: 'Michael Chen',
      studentId: 'STD003',
      section: 'Events Team',
      requestedOn: new Date('2024-03-18T16:45:00'),
      reason: 'I have experience organizing college events and would love to help plan and execute society activities. I am good at coordination and have strong communication skills.',
      status: 'PENDING',
      profilePictureUrl: 'https://api.dicebear.com/7.x/avataaars/svg?seed=Michael'
    },
    {
      id: '8',
      studentName: 'Sarah Johnson',
      studentId: 'STD002',
      section: 'Design Team',
      requestedOn: new Date('2024-03-19T09:15:00'),
      reason: 'I am passionate about UI/UX design and have completed several courses on web design. I would like to help improve the society\'s digital presence.',
      status: 'PENDING',
      profilePictureUrl: 'https://api.dicebear.com/7.x/avataaars/svg?seed=Sarah'
    },
    {
      id: '9',
      studentName: 'Michael Chen',
      studentId: 'STD003',
      section: 'Events Team',
      requestedOn: new Date('2024-03-18T16:45:00'),
      reason: 'I have experience organizing college events and would love to help plan and execute society activities. I am good at coordination and have strong communication skills.',
      status: 'PENDING',
      profilePictureUrl: 'https://api.dicebear.com/7.x/avataaars/svg?seed=Michael'
    }
  ];


  isRequestDetailsVisible = false;
  selectedRequest: MembershipRequest | null = null;

  showRequestDetails(request: MembershipRequest): void {
    this.selectedRequest = request;
    this.isRequestDetailsVisible = true;
  }

  acceptRequest(requestId: string): void {
    // For development/demo purposes
    const request = this.membershipRequests.find(r => r.id === requestId);
    if (request) {
      request.status = 'ACCEPTED';
      // In real implementation, make API call here
      console.log(`Accepted request ${requestId}`);
    }
  }

  rejectRequest(requestId: string): void {
    // For development/demo purposes
    const request = this.membershipRequests.find(r => r.id === requestId);
    if (request) {
      request.status = 'REJECTED';
      // In real implementation, make API call here
      console.log(`Rejected request ${requestId}`);
    }
  }
}

export interface MembershipRequest {
  id: string;
  studentName: string;
  studentId: string;
  section: string;
  requestedOn: Date;
  reason: string;
  status: 'PENDING' | 'ACCEPTED' | 'REJECTED';
  profilePictureUrl?: string;
}
