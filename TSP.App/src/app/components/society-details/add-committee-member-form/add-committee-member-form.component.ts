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

  isMembersLoading = false;
  isPositionsLoading = false;

  formBuilder = inject(FormBuilder);
  messageService = inject(NzMessageService);

  addCommitteeMemberForm: FormGroup | undefined;

  positions = committeePositions;

  displayedPositions = [...this.positions];

  members = [
    { id: 1, name: 'Ahmad Muhaisen' },
    { id: 2, name: 'Suhaib Saleh' },
    { id: 3, name: 'Rimawi' },
    { id: 4, name: 'Ahmad Abu Tair' },
    { id: 5, name: 'Mohammad AbuAdas' },
    { id: 6, name: 'Omar Waggad' },
    { id: 7, name: 'Ahmad Muhaisen' },
    { id: 8, name: 'Suhaib Saleh' },
    { id: 9, name: 'Rimawi' },
    { id: 10, name: 'Ahmad Abu Tair' },
    { id: 11, name: 'Mohammad AbuAdas' },
    { id: 12, name: 'Omar Waggad' },
    { id: 13, name: 'Ahmad Muhaisen' },
    { id: 14, name: 'Suhaib Saleh' },
    { id: 15, name: 'Rimawi' },
    { id: 16, name: 'Ahmad Abu Tair' },
    { id: 17, name: 'Mohammad AbuAdas' },
    { id: 18, name: 'Omar Waggad' }
  ];

  displayedMembers = [...this.members];


  ngOnInit() {
    this.addCommitteeMemberForm = this.formBuilder.group({
      studentId: ['', [Validators.required]],
      position: ['', [Validators.required]],
      startDate: [new Date(), [Validators.required]],
    });

    this.members = this.members.filter(m => !this.existingCommitteeMembers()!.some(e => e.name === m.name));
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
    //this.isMembersLoading = true;
    this.displayedMembers = this.members.filter(member => member.name.toLowerCase().includes(value.toLowerCase()));
  }

  isFormValid(): boolean {
    return this.addCommitteeMemberForm!.valid && this.addCommitteeMemberForm!.dirty && this.addCommitteeMemberForm!.touched;
  }

  getFormValue(): { studentId: string, position: string, startDate: Date } {
    return this.addCommitteeMemberForm!.value;
  }
}
