import { DatePipe } from '@angular/common';
import { Component, input } from '@angular/core';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';

@Component({
  selector: 'app-committee-table',
  imports: [
    DatePipe,
    NzBreadCrumbModule,
    NzButtonModule,
    NzDividerModule,
    NzIconModule,
    NzTableModule,
    NzPopconfirmModule,
    NzToolTipModule,
    NzModalModule
  ],
  templateUrl: './committee-table.component.html',
  styleUrl: './committee-table.component.css'
})
export class CommitteeTableComponent {
  isEditCommitteePopupVisible = false;
  isEditCommitteePopupLoading = false;
  memberToEdit = null;

  committee = input.required<any[]>();

  openEditMemberPopup(id: string) {
    this.memberToEdit = this.committee()!.find(m => m.id === id);
    this.isEditCommitteePopupVisible = true;
  }

  removeCommitteeMember(id: string) {
    // remove member
  }

  handleCancelEditCommitteeMember() {
    this.isEditCommitteePopupVisible = false;
    this.isEditCommitteePopupLoading = false;
    this.memberToEdit = null;
  }

  handleOkEditCommitteeMember() {

  }
}
