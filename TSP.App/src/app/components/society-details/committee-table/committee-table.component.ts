import { DatePipe } from '@angular/common';
import { Component, inject, input, OnInit, output } from '@angular/core';
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
import { SocietyMember } from '../../../areas/system-admin-area/api-interfaces/society.types';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { SocietiesService } from '../../../areas/system-admin-area/services/societies.service';
import { environment } from '../../../../environments/environment';

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
    NzAvatarModule,
    ReactiveFormsModule,
  ],
  templateUrl: './committee-table.component.html',
})
export class CommitteeTableComponent implements OnInit {

  isViewOnly = input<boolean>(false);
  societyId = input.required<string>();
  committee = input.required<SocietyMember[]>();
  committeeChange = output<SocietyMember[]>();
  
  baseUserUmage:string = environment.gitHubUsersPicturesURL

  isEditCommitteePopupVisible = false;
  isEditCommitteePopupLoading = false;
  memberToEdit: SocietyMember | undefined = undefined;

  displayedCommittee: SocietyMember[] = [];

  messageService = inject(NzMessageService);
  societiesService = inject(SocietiesService);

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

    // Initialize and update displayed committee when input changes
    this.displayedCommittee = [...this.committee()];
    this.positions = committeePositions.filter(p => !this.isTakenPosition(p.name));
  }

  ngOnChanges() {
    if (this.committee()) {
      this.displayedCommittee = [...this.committee()];
      this.positions = committeePositions.filter(p => !this.isTakenPosition(p.name));
    }
  }

  openEditMemberPopup(id: string) {
    this.memberToEdit = this.committee()!.find(m => m.id === id);
    if (this.memberToEdit) {
      this.setEditMemberFormValues();
      this.isEditCommitteePopupVisible = true;
    }
  }

  handleCancelEditCommitteeMember() {
    this.isEditCommitteePopupVisible = false;
    this.memberToEdit = undefined;
    this.editCommitteeMemberForm?.reset();
  }

  handleOkEditCommitteeMember() {
    if (!this.memberToEdit || !this.editCommitteeMemberForm?.valid) {
      this.messageService.error('Please fill in all required fields.');
      return;
    }

    const position = this.editCommitteeMemberForm.get('position')?.value;
    this.isEditCommitteePopupLoading = true;

    this.societiesService.editMember(this.memberToEdit.id, this.societyId(), position).subscribe({
      next: () => {
        this.messageService.success('Committee member position updated successfully');
        // Update the local list
        const updatedCommittee = this.committee().map(member => 
          member.id === this.memberToEdit!.id 
            ? { ...member, position: position }
            : member
        );
        this.committeeChange.emit(updatedCommittee);
        this.handleCancelEditCommitteeMember();
      },
      error: (error: unknown) => {
        this.messageService.error('Failed to update committee member position');
        console.error('Error updating committee member position:', error);
      },
      complete: () => {
        this.isEditCommitteePopupLoading = false;
      }
    });
  }

  setEditMemberFormValues() {
    if (!this.memberToEdit) return;
    
    this.editCommitteeMemberForm!.patchValue({
      name: this.memberToEdit.firstName + ' ' + this.memberToEdit.lastName,
      position: this.memberToEdit.position,
      startDate: this.memberToEdit.joinDate
    });
    this.editCommitteeMemberForm!.get('name')?.disable();
    this.editCommitteeMemberForm!.get('startDate')?.disable();
  }

  removeCommitteeMember(id: string) {
    this.societiesService.removeCommitteeMember(this.societyId(), id).subscribe({
      next: () => {
        this.messageService.success('Committee member removed successfully');
        const updatedCommittee = this.committee().filter(member => member.id !== id);
        this.committeeChange.emit(updatedCommittee);
      },
      error: (error: unknown) => {
        this.messageService.error('Failed to remove committee member');
        console.error('Error removing committee member:', error);
      }
    });
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
