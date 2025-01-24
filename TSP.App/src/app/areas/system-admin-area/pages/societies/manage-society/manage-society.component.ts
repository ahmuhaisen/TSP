import { DatePipe } from '@angular/common';
import { Component, inject, ViewChild } from '@angular/core';
import { NzAvatarComponent } from 'ng-zorro-antd/avatar';
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
    EditSocietyInfoFormComponent
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

  messageService = inject(NzMessageService)

  isEditSocietyInfoPopupVisible = false;
  isEditSocietyInfoLoading = false;

  @ViewChild(EditSocietyInfoFormComponent) editSocietyInfoFormComponent?: EditSocietyInfoFormComponent;

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
}
