import { DatePipe } from '@angular/common';
import { Component, inject, input } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, FormGroup, Validators } from '@angular/forms';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzAlertModule } from 'ng-zorro-antd/alert';
import { committeePositions } from '../../../common/constants/committee-positions.constant';

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
    NzModalModule,
    NzDatePickerModule,
    NzFormModule,
    NzInputModule,
    NzSelectModule,
    NzAlertModule,
    ReactiveFormsModule,
  ],
  templateUrl: './committee-table.component.html',
})
export class CommitteeTableComponent {

  isViewOnly = input<boolean>(false);

  isEditCommitteePopupVisible = false;
  isEditCommitteePopupLoading = false;
  memberToEdit: any = null;

  committee = input.required<any[]>();

  messageService = inject(NzMessageService);

  formBuilder = inject(FormBuilder);
  editCommitteeMemberForm: FormGroup | undefined;

  positions = committeePositions;

  displayedPositions = [...this.positions];


  ngOnInit() {
    this.editCommitteeMemberForm = this.formBuilder.group({
      name: ['', [Validators.required]],
      position: ['', [Validators.required]],
      startDate: [new Date(), [Validators.required]],
    });

    this.positions = this.positions.filter(p => !this.isTakenPosition(p.name));
  }

  openEditMemberPopup(id: string) {
    this.memberToEdit = this.committee()!.find(m => m.id === id);
    this.isEditCommitteePopupVisible = true;

    this.memberToEdit = this.committee()!.find(m => m.id === id);
    this.setEditMemberFormValues();
  }

  removeCommitteeMember(id: string) {
    // remove member
  }

  handleCancelEditCommitteeMember() {
    this.isEditCommitteePopupVisible = false;
    this.isEditCommitteePopupLoading = false;
    this.memberToEdit = null;
    this.clearEditMemberForm();
  }

  handleOkEditCommitteeMember() {

  }

  setEditMemberFormValues() {
    console.table(this.memberToEdit);
    this.editCommitteeMemberForm!.get('name')?.disable();
    this.editCommitteeMemberForm!.get('name')?.setValue(this.memberToEdit!.name);
    this.editCommitteeMemberForm!.get('position')?.setValue(this.memberToEdit!.position);
    this.editCommitteeMemberForm!.get('startDate')?.setValue(this.memberToEdit!.startDate);
  }

  clearEditMemberForm() {
    this.editCommitteeMemberForm!.reset();
  }

  isTakenPosition(position: string): boolean {
    const lowerCaseValue = position.toLowerCase().trim();
    return this.committee()!.some(e => e.position.toLowerCase() === lowerCaseValue);
  }

  isPositionFound(position: string): boolean {
    const lowerCaseValue = position.toLowerCase().trim();
    return this.positions.some(p => p.name.toLowerCase() === lowerCaseValue) || this.isTakenPosition(lowerCaseValue);
  }

  onSearchPositions(value: string): void {
    const lowerCaseValue = value.toLowerCase().trim();
    this.displayedPositions = this.positions.filter(p => p.name.toLowerCase().includes(lowerCaseValue));

    if (!this.isPositionFound(value) && value.trim()) {
      this.displayedPositions.push({ name: `Add "${value.trim()}"` });
    }
  }

  onSelectPosition(selectedValue: string): void {
    if (selectedValue.startsWith('Add "')) {
      const newPositionName = selectedValue.replace(/^Add "(.*)"$/, '$1').trim();

      const newPosition = { name: newPositionName };
      this.positions.push(newPosition);
      this.displayedPositions = [...this.positions];

      this.editCommitteeMemberForm!.get('position')?.setValue(newPositionName);

      this.messageService.success(`Position "${newPositionName}" has been added and selected.`);
    }
  }


  isFormValid(): boolean {
    return this.editCommitteeMemberForm!.valid && this.editCommitteeMemberForm!.dirty && this.editCommitteeMemberForm!.touched;
  }

  getFormValue(): { studentId: string, position: string, startDate: Date } {
    return this.editCommitteeMemberForm!.value;
  }
}
