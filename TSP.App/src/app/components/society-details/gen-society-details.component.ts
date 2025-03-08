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
import { Member, SocietyMember, SocietyWithAdvisor } from '../../areas/system-admin-area/api-interfaces/society.types';
import { DatePipe } from '@angular/common';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';

@Component({
  selector: 'app-gen-society-details',
  imports: [
    DatePipe,
    NzBreadCrumbModule,
    NzButtonModule,
    NzDividerModule,
    NzIconModule,
    NzTableModule,
    NzAvatarModule,
    MembersTableComponent,
    CommitteeTableComponent,
    NzModalModule,
    EditSocietyInfoFormComponent,
    AddCommitteeMemberFormComponent,
    AddMemberFormComponent,
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

  messageService = inject(NzMessageService);
  breadcrumbService = inject(BreadcrumbService);

  isEditSocietyInfoPopupVisible = false;
  isEditSocietyInfoLoading = false;

  isAddCommitteePopupVisible = false;
  isAddCommitteeLoading = false;

  @ViewChild(EditSocietyInfoFormComponent) editSocietyInfoFormComponent?: EditSocietyInfoFormComponent;
  @ViewChild(AddCommitteeMemberFormComponent) addCommitteeMemberForm?: AddCommitteeMemberFormComponent;

  ngOnInit() {
    this.societyService.find(this.societyId()).subscribe({
      next: society => {
        if (!society) {
          return;
        }

        console.table(society);
        this.society = society;
        this.breadcrumbService.set('@societyName', this.society!.name);
        console.log('pageMode:', this.pageMode(), 'societyId:', this.societyId());
      }
    });


    this.societyService.societyMembers(this.societyId(), false).subscribe({
      next: members => {
        this.members.set(members);
      }
    });

    this.societyService.societyMembers(this.societyId(), true).subscribe({
      next: members => {
        this.committee.set(members);
      }
    });
  }

  openEditSocietyInfoPopup() {
    this.isEditSocietyInfoPopupVisible = true;
  }

  handleCancelEditSociety() {
    this.isEditSocietyInfoPopupVisible = false;
    this.editSocietyInfoFormComponent!.createSocietyForm?.reset();
  }

  handleOkEditSociety() {
    if (this.editSocietyInfoFormComponent!.createSocietyForm!.invalid) {
      this.editSocietyInfoFormComponent!.messageService.error('Please fill in all required fields.');
      this.editSocietyInfoFormComponent!.createSocietyForm?.markAllAsTouched();
      return;
    }

    console.table(this.editSocietyInfoFormComponent!.createSocietyForm?.value);
    this.isEditSocietyInfoLoading = true;

    setTimeout(() => {
      this.isEditSocietyInfoLoading = false;
      this.isEditSocietyInfoPopupVisible = false;
      this.isEditSocietyInfoPopupVisible = false;
      this.messageService.success('Society info updated successfully.');
    }, 1000);
  }

  openAddCommitteePopup() {
    this.isAddCommitteePopupVisible = true;
  }

  handleCancelAddCommittee() {
    this.isAddCommitteePopupVisible = false;
  }

  handleOkAddCommittee() {
    if (!this.addCommitteeMemberForm!.isFormValid()) {
      this.addCommitteeMemberForm!.messageService.error('Please fill in all required fields.');
      return;
    }

    console.table(this.addCommitteeMemberForm!.getFormValue());
  }

  isAddMemberPopupVisible = false;

  openAddMemberPopup() {
    this.isAddCommitteePopupVisible = true;
  }

  handleCancelAddMember() {
    this.isAddCommitteePopupVisible = false;
  }

  handleOkAddMember() {
    if (!this.addCommitteeMemberForm!.isFormValid()) {
      this.addCommitteeMemberForm!.messageService.error('Please fill in all required fields.');
      return;
    }

    console.table(this.addCommitteeMemberForm!.getFormValue());
  }
}
