import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { TruncatePipe } from '../../../../../common/pipes/truncate.pipe';
import { NzButtonComponent, NzButtonModule } from 'ng-zorro-antd/button';
import { NgClass } from '@angular/common';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzMessageService } from 'ng-zorro-antd/message';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzTabsModule } from 'ng-zorro-antd/tabs';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-societies-list',
  imports: [
    NgClass,
    RouterLink,
    TruncatePipe,
    NzIconModule,
    NzDividerModule,
    NzButtonModule,
    NzEmptyModule,
    NzModalModule,
    ReactiveFormsModule,
    NzSelectModule,
    NzInputModule,
    NzFormModule,
    NzTagModule,
    NzTableModule,
    NzTabsModule,
    DatePipe
  ],
  templateUrl: './societies-list.component.html',
  styleUrl: './societies-list.component.css'
})
export class SocietiesListComponent {
  messageService = inject(NzMessageService);

  belongingSocieties = [
    {
      id: '1ab-2cd-3ef-4gh',
      name: 'ACM University of Jordan Student Chapter',
      description: 'A Chapter of the Association for Computing Machinery, interested in computer science and programming.',
      logoUrl: 'https://robohash.org/society1',
      isCommittee: true,
      position: 'President'
    },
    {
      id: '9qr-0st-1uv-2wx',
      name: 'Linux Society JU',
      description: 'Linux Society of Jordan',
      logoUrl: 'https://robohash.org/society2',
      isCommittee: false,
      position: 'Media'
    },
    {
      id: '321-1sad-1uv-23sd',
      name: 'Waves JU',
      description: 'Waves Society of Jordan, a student chapter interested in robotics.',
      logoUrl: 'https://robohash.org/society3',
      isCommittee: false,
      position: 'Technical Team'
    },
  ];

  otherSocieties = [
    {
      id: '5ij-6kl-7mn-8op',
      name: 'IEEE CS JU',
      description: 'The IEEE Computer Society of Jordan',
      logoUrl: 'https://robohash.org/society1',
      themeColor: '#1f1f1f',
    },
    {
      id: '1ab-2cd-3ef-4gh',
      name: 'ACM University of Jordan Student Chapter',
      description: 'A Chapter of the Association for Computing Machinery, interested in computer science and programming.',
      logoUrl: 'https://robohash.org/society2',
      themeColor: '#1f1f1f',
    },
    {
      id: '9qr-0st-1uv-2wx',
      name: 'Linux Society JU',
      description: 'Linux Society of Jordan',
      logoUrl: 'https://robohash.org/society3',
      themeColor: '#1f1f1f',
    },
    {
      id: '2345-fghi-0123-4mn',
      name: 'Waves JU',
      description: 'The Waves Society of Jordan',
      logoUrl: 'https://robohash.org/society4',
      themeColor: '#1f1f1f',
    }
  ];

  societyToLeave = this.belongingSocieties[0];
  isLeaveSocietyPopupVisible = false;
  isLeaveSocietyLoading = false;

  isJoinSocietyModalVisible = false;
  isJoinSocietyLoading = false;
  joinSocietyForm: FormGroup;

  suggestedSections = [
    'Technical',
    'Media',
    'Logistics',
    'Human Resources',
    'Public Relations',
    'Content Creation'
  ];

  joinRequests: SocietyJoinRequest[] = [
    {
      id: '1',
      societyId: '5ij-6kl-7mn-8op',
      societyName: 'IEEE CS JU',
      societyLogo: 'https://robohash.org/society1',
      section: 'Technical',
      status: 'pending',
      submittedAt: new Date(2024, 2, 15),
      motivation: 'I want to contribute to the technical team...'
    },
    {
      id: '2',
      societyId: '2345-fghi-0123-4mn',
      societyName: 'Waves JU',
      societyLogo: 'https://robohash.org/society4',
      section: 'Media',
      status: 'approved',
      submittedAt: new Date(2024, 2, 10),
      motivation: 'I have experience in media...'
    },
    {
      id: '3',
      societyId: '9qr-0st-1uv-2wx',
      societyName: 'Linux Society JU',
      societyLogo: 'https://robohash.org/society3',
      section: 'Content Creation',
      status: 'rejected',
      submittedAt: new Date(2024, 2, 5),
      motivation: 'I want to help create content...'
    }
  ];

  constructor(private fb: FormBuilder) {
    this.joinSocietyForm = this.fb.group({
      societyId: [null, [Validators.required]],
      section: ['', [Validators.required]],
      motivation: ['', [Validators.required, Validators.minLength(50)]]
    });
  }

  leaveSociety(society: MemberAssociatedSociety) {
    this.societyToLeave = society;
    this.isLeaveSocietyPopupVisible = true;
  }

  handleCancelLeaveSociety() {
    this.isLeaveSocietyPopupVisible = false;
    this.isLeaveSocietyLoading = false;
  }

  handleOkLeaveSociety() {
    this.isLeaveSocietyLoading = true;
    
    setTimeout(() => {
      this.isLeaveSocietyPopupVisible = false;
      this.isLeaveSocietyLoading = false;
      this.messageService.success('You have left ' + this.societyToLeave.name + ' successfully.');
    }, 1000);
  }

  showJoinSocietyModal(): void {
    this.isJoinSocietyModalVisible = true;
  }

  handleCancelJoinSociety(): void {
    this.isJoinSocietyModalVisible = false;
    this.joinSocietyForm.reset();
  }

  handleJoinSociety(): void {
    if (this.joinSocietyForm.valid) {
      this.isJoinSocietyLoading = true;
      
      // Simulate API call
      setTimeout(() => {
        this.isJoinSocietyLoading = false;
        this.isJoinSocietyModalVisible = false;
        this.messageService.success('Your request to join the society has been submitted successfully!');
        this.joinSocietyForm.reset();
      }, 1000);
    } else {
      Object.values(this.joinSocietyForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsTouched();
        }
      });
    }
  }

  selectSuggestedSection(section: string): void {
    this.joinSocietyForm.patchValue({ section });
  }

  getStatusColor(status: string): string {
    switch (status) {
      case 'pending': return 'processing';
      case 'approved': return 'success';
      case 'rejected': return 'error';
      default: return '';
    }
  }
}

export interface MemberAssociatedSociety {
  id: string;
  name: string;
  description: string;
  logoUrl: string;
  position: string;
  isCommittee: boolean;
}

interface SocietyJoinRequest {
  id: string;
  societyId: string;
  societyName: string;
  societyLogo: string;
  section: string;
  status: 'pending' | 'approved' | 'rejected';
  submittedAt: Date;
  motivation: string;
}