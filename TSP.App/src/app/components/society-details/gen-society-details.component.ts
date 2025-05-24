import { Component, inject, input, signal, ViewChild } from '@angular/core';
import { BreadcrumbService } from 'xng-breadcrumb';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzTableModule } from 'ng-zorro-antd/table';
import { MembersTableComponent } from "./members-table/members-table.component";
import { CommitteeTableComponent } from "./committee-table/committee-table.component";
import { NzModalModule } from 'ng-zorro-antd/modal';
import { EditSocietyInfoFormComponent } from "./edit-society-info-form/edit-society-info-form.component";
import { NzMessageService } from 'ng-zorro-antd/message';
import { AddCommitteeMemberFormComponent } from "./add-committee-member-form/add-committee-member-form.component";
import { AddMemberFormComponent } from "./add-member-form/add-member-form.component";
import { PageMode } from '../../common/types/presentaion.types';
import { SocietiesService } from '../../areas/system-admin-area/services/societies.service';
import { SocietyMember, SocietyWithAdvisor } from '../../areas/system-admin-area/api-interfaces/society.types';
import { DatePipe } from '@angular/common';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { Router } from '@angular/router';
import { NzSkeletonModule } from 'ng-zorro-antd/skeleton';
import { AuthService } from '../../common/services/auth.service';
import { MembershipRequestsComponent } from "./membership-requests/membership-requests.component";
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';
import { environment } from '../../../environments/environment';
import { RouterLink } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-gen-society-details',
  imports: [
    DatePipe,
    NzBreadCrumbModule,
    NzButtonModule,
    NzDividerModule,
    NzIconModule,
    NzTableModule,
    NzToolTipModule,
    NzAvatarModule,
    MembersTableComponent,
    CommitteeTableComponent,
    NzModalModule,
    NzSkeletonModule,
    AddCommitteeMemberFormComponent,
    MembershipRequestsComponent,
    RouterLink
  ],
  templateUrl: './gen-society-details.component.html',
  styleUrl: './gen-society-details.component.css'
})
export class GenSocietyDetailsComponent {
  societyService = inject(SocietiesService);
  pageMode = input<PageMode>('VIEW_ONLY');
  societyId = input.required<string>();
  society: SocietyWithAdvisor | null = null;
  members = signal<SocietyMember[]>([]);
  committee = signal<SocietyMember[]>([]);
  authService = inject(AuthService);

  baseUserImage: string = environment.gitHubUsersPicturesURL;
  baseSocietyImage: string = environment.gitHubSocietiesPicturesURL;

  isSocietyLoading = false;
  isCommitteeLoading = false;
  isMembersLoading = false;

  messageService = inject(NzMessageService);
  breadcrumbService = inject(BreadcrumbService);
  router = inject(Router);

  isEditSocietyInfoPopupVisible = false;
  isEditSocietyInfoLoading = false;

  isAddCommitteePopupVisible = false;
  isAddCommitteeLoading = false;

  isAddMemberPopupVisible = false;
  isAddMemberLoading = false;

  activateRoute = inject(ActivatedRoute);
  routeFirstSegment: string = "";

  @ViewChild(EditSocietyInfoFormComponent) editSocietyInfoFormComponent?: EditSocietyInfoFormComponent;
  @ViewChild(AddCommitteeMemberFormComponent) addCommitteeMemberForm?: AddCommitteeMemberFormComponent;
  @ViewChild(AddMemberFormComponent) addMemberForm: AddMemberFormComponent | undefined;

  ngOnInit() {
    this.isSocietyLoading = true;
    this.isCommitteeLoading = true;
    this.isMembersLoading = true;
    this.routeFirstSegment = this.activateRoute.snapshot.pathFromRoot[1]?.url[0]?.path;

    this.societyService.find(this.societyId()).subscribe({
      next: society => {
        if (!society) {
          return;
        }

        if (this.pageMode() === 'ADMIN_MANAGE') {
          if (this.authService.currentUser()?.id !== society.advisor.id) {
            this.router.navigate(['forbidden']);
            return;
          }
        }

        console.table(society);
        this.society = society;
        this.breadcrumbService.set('@societyName', this.society!.name);
        console.log('pageMode:', this.pageMode(), 'societyId:', this.societyId());

        this.isSocietyLoading = false;
      },
      error: () => {
        this.isSocietyLoading = false;
        this.router.navigate(['/societies']);
      }
    });

    this.societyService.societyMembers(this.societyId(), true).subscribe({
      next: members => {
        this.committee.set(members);
        this.isCommitteeLoading = false;

        if (this.pageMode() === 'STUDENT_MANAGE') {
          if (!members.some(member => member.id === this.authService.currentUser()?.id)) {
            this.router.navigate(['forbidden']);
            return;
          }
        }
      }
    });

    this.societyService.societyMembers(this.societyId(), false).subscribe({
      next: members => {
        this.members.set(members);
        this.isMembersLoading = false;
      }
    });
  }


  handleOkEditSociety() {
    if (this.editSocietyInfoFormComponent!.createSocietyForm!.invalid) {
      this.editSocietyInfoFormComponent!.messageService.error('Please fill in all required fields.');
      this.editSocietyInfoFormComponent!.createSocietyForm?.markAllAsTouched();
      return;
    }

    const formValue = this.editSocietyInfoFormComponent!.createSocietyForm!.value;
    this.isEditSocietyInfoLoading = true;

    const updateRequest = {
      id: this.societyId(),
      name: formValue.name,
      description: formValue.description,
      logoBase64: formValue.logo || this.society?.logoId || '',
      themeColor: formValue.themeColor
    };

    console.log('Update request:', updateRequest);

    this.societyService.update(this.societyId(), updateRequest).subscribe({
      next: () => {
        this.messageService.success('Society info updated successfully.');
        this.isEditSocietyInfoPopupVisible = false;
        this.editSocietyInfoFormComponent!.createSocietyForm?.reset();

        // Refresh society details
        this.societyService.find(this.societyId()).subscribe({
          next: society => {
            if (!society) {
              return;
            }
            this.society = society;
            this.breadcrumbService.set('@societyName', this.society!.name);
          }
        });
      },
      error: (error: unknown) => {
        this.messageService.error('Failed to update society info');
        console.error('Error updating society:', error);
      },
      complete: () => {
        this.isEditSocietyInfoLoading = false;
      }
    });
  }

  openAddCommitteePopup() {
    this.isAddCommitteePopupVisible = true;
  }

  handleCancelAddCommittee() {
    this.isAddCommitteePopupVisible = false;
  }

  handleOkAddCommittee() {
    if (!this.addCommitteeMemberForm!.isFormValid()) {
      this.messageService.error('Please fill in all required fields.');
      return;
    }

    const formValue = this.addCommitteeMemberForm!.getFormValue();
    this.isAddCommitteeLoading = true;

    // Format date as yyyy-MM-dd
    const date = new Date(formValue.startDate);
    const formattedDate = date.getFullYear() + '-' +
      String(date.getMonth() + 1).padStart(2, '0') + '-' +
      String(date.getDate()).padStart(2, '0');

    this.societyService.addCommittee(this.societyId(), formValue.studentId, {
      position: formValue.position,
      startDate: formattedDate
    }).subscribe({
      next: () => {
        this.messageService.success('Committee member added successfully');
        this.isAddCommitteePopupVisible = false;

        // Remove member from members list
        const updatedMembers = this.members().filter(member => member.id !== formValue.studentId);
        this.members.set(updatedMembers);

        // Refresh committee list
        this.societyService.societyMembers(this.societyId(), true).subscribe({
          next: members => {
            this.committee.set(members);
          }
        });
      },
      error: (error: unknown) => {
        this.messageService.error('Failed to add committee member');
        console.error('Error adding committee member:', error);
      },
      complete: () => {
        this.isAddCommitteeLoading = false;
      }
    });
  }

  handleCommitteeChange(newCommittee: SocietyMember[]) {
    const removedMembers = this.committee().filter(member =>
      !newCommittee.some(newMember => newMember.id === member.id)
    );

    // Add removed committee members to the members list
    if (removedMembers.length > 0) {
      const updatedMembers = [...this.members(), ...removedMembers.map(member => ({
        ...member,
        isCommitteeMember: false
      }))];
      this.members.set(updatedMembers);
    }

    // Update committee list
    this.committee.set(newCommittee);
  }

  @ViewChild(MembershipRequestsComponent) membershipRequestsComponent: MembershipRequestsComponent | undefined;

  showSocietyRequests() {
    this.membershipRequestsComponent?.showSocietyRequests();
  }

  downloadMembersAsCSV(): void {
    const members = this.members();
    if (!members.length) {
      this.messageService.warning('No members to export');
      return;
    }

    // Define CSV headers
    const headers = ['First Name', 'Last Name', 'Position', 'Join Date'];

    // Convert members data to CSV rows
    const csvRows = members.map(member => [
      member.firstName,
      member.lastName,
      member.position,
      new Date(member.joinDate).toLocaleDateString()
    ]);

    // Combine headers and rows
    const csvContent = [
      headers.join(','),
      ...csvRows.map(row => row.join(','))
    ].join('\n');

    // Create blob and download link
    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    const link = document.createElement('a');
    const url = URL.createObjectURL(blob);

    // Set download attributes
    link.setAttribute('href', url);
    link.setAttribute('download', `${this.society?.name || 'society'}_members.csv`);
    link.style.visibility = 'hidden';

    // Trigger download
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }
}
