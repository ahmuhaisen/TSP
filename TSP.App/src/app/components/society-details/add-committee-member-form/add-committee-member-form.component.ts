import { Component, inject, input } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzAlertModule } from 'ng-zorro-antd/alert';
import { committeePositions } from '../../../common/constants/committee-positions.constant';

@Component({
  selector: 'app-add-committee-member-form',
  imports: [
    ReactiveFormsModule,
    NzButtonModule,
    NzIconModule,
    NzDividerModule,
    NzFormModule,
    NzInputModule,
    NzDatePickerModule,
    NzSelectModule,
    NzAlertModule
  ],
  templateUrl: './add-committee-member-form.component.html',
})
export class AddCommitteeMemberFormComponent {
  existingCommitteeMembers = input.required<any[]>();
  societyMembers = input.required<any[]>();

  isMembersLoading = false;
  isPositionsLoading = false;

  formBuilder = inject(FormBuilder);
  messageService = inject(NzMessageService);

  addCommitteeMemberForm: FormGroup | undefined;

  positions = committeePositions;

  displayedPositions = [...this.positions];
  displayedMembers: any[] = [];

  ngOnInit() {
    this.addCommitteeMemberForm = this.formBuilder.group({
      studentId: ['', [Validators.required]],
      position: ['', [Validators.required]],
      startDate: [new Date(), [Validators.required]],
    });

    this.displayedMembers = this.societyMembers().filter(
      member => !this.existingCommitteeMembers()!.some(
        committee => committee.id === member.id
      )
    );
    this.positions = this.positions.filter(p => !this.isTakenPosition(p.name));
  }

  isTakenPosition(position: string): boolean {
    const lowerCaseValue = position.toLowerCase().trim();
    return this.existingCommitteeMembers()!.some(e => e.position.toLowerCase() === lowerCaseValue);
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

      this.addCommitteeMemberForm!.get('position')?.setValue(newPositionName);

      this.messageService.success(`Position "${newPositionName}" has been added and selected.`);
    }
  }

  onSearchMembers(value: string): void {
    this.displayedMembers = this.societyMembers().filter(
      member => !this.existingCommitteeMembers()!.some(
        committee => committee.id === member.id
      ) && 
      (member.firstName + ' ' + member.lastName).toLowerCase().includes(value.toLowerCase())
    );
  }

  isFormValid(): boolean {
    return this.addCommitteeMemberForm!.valid && this.addCommitteeMemberForm!.dirty && this.addCommitteeMemberForm!.touched;
  }

  getFormValue(): { studentId: string, position: string, startDate: Date } {
    return this.addCommitteeMemberForm!.value;
  }
}
