import { Component, inject, ViewChild } from '@angular/core';
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

@Component({
  selector: 'app-manage-society',
  imports: [
    NzBreadCrumbModule,
    NzButtonModule,
    NzDividerModule,
    NzIconModule,
    NzTableModule,
    MembersTableComponent,
    CommitteeTableComponent,
    NzModalModule,
    EditSocietyInfoFormComponent,
    AddCommitteeMemberFormComponent,
    AddMemberFormComponent,
],
  templateUrl: './manage-society.component.html',
  styleUrl: './manage-society.component.css'
})
export class ManageSocietyComponent {
  society = {
    id: '32-afd43',
    name: 'ACM JU',
    description: 'This is a description',
    creationDate: new Date('2017-01-01'),
    themeColor: '#1677ff',
    logo: 'https://zos.alipayobjects.com/rmsportal/jkjgkEfvpUPVyRjUImniVslZfWPnJuuZ.png',
    advisorId: 1
  }

  committee = [
    {
      id: '23fs-sdf',
      name: 'Suhaib Saleh',
      position: 'President',
      imageUrl: 'https://randomuser.me/api/portraits/lego/1.jpg',
      startDate: '2024-01-01',
    },
    {
      id: '23fs-sdf',
      name: 'Amer Khaleel',
      position: 'Vice President',
      imageUrl: 'https://randomuser.me/api/portraits/lego/2.jpg',
      startDate: '2024-01-01',
    },
    {
      id: '23fs-sdf',
      name: 'Noor Aldeen',
      position: 'Treasure',
      imageUrl: 'https://randomuser.me/api/portraits/lego/3.jpg',
      startDate: '2024-01-01',
    }
  ];

  messageService = inject(NzMessageService);
  breadcrumbService = inject(BreadcrumbService);

  isEditSocietyInfoPopupVisible = false;
  isEditSocietyInfoLoading = false;

  isAddCommitteePopupVisible = false;
  isAddCommitteeLoading = false;

  @ViewChild(EditSocietyInfoFormComponent) editSocietyInfoFormComponent?: EditSocietyInfoFormComponent;
  @ViewChild(AddCommitteeMemberFormComponent) addCommitteeMemberForm?: AddCommitteeMemberFormComponent;

  ngOnInit() {
    this.breadcrumbService.set('@societyName', this.society.name);
  }

  openEditSocietyInfoPopup() {
    this.isEditSocietyInfoPopupVisible = true;
  }

  handleCancelEditSociety() {
    this.isEditSocietyInfoPopupVisible = false;
    this.editSocietyInfoFormComponent!.createSocietyForm?.reset();
  }

  handleOkEditSociety() {
    if(this.editSocietyInfoFormComponent!.createSocietyForm!.invalid){
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
    if(!this.addCommitteeMemberForm!.isFormValid()) {
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
    if(!this.addCommitteeMemberForm!.isFormValid()) {
      this.addCommitteeMemberForm!.messageService.error('Please fill in all required fields.');
      return;
    }

    console.table(this.addCommitteeMemberForm!.getFormValue());
  }
}
