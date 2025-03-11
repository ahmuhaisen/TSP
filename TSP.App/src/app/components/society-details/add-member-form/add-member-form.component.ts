import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzTagModule } from 'ng-zorro-antd/tag';

@Component({
  selector: 'app-add-member-form',
  imports: [
    ReactiveFormsModule,
    NzButtonModule,
    NzIconModule,
    NzDividerModule,
    NzFormModule,
    NzInputModule,
    NzDatePickerModule,
    NzSelectModule,
    NzTagModule
  ],
  templateUrl: './add-member-form.component.html'
})
export class AddMemberFormComponent {
  isMembersLoading = false;

  formBuilder = inject(FormBuilder);
  messageService = inject(NzMessageService);

  addMemberForm: FormGroup | undefined;

  students = [
    { id: '1', name: 'Ahmad Muhaisen' },
    { id: '2', name: 'Suhaib Saleh' },
    { id: '3', name: 'Rimawi' },
    { id: '4', name: 'Ahmad Abu Tair' },
    { id: '5', name: 'Mohammad AbuAdas' },
    { id: '6', name: 'Omar Waggad' }
  ];

  displayedStudents = [...this.students];

  suggestedPositions = [
    'Member',
    'Team Lead',
    'Project Manager',
    'Coordinator',
    'Event Organizer',
    'Technical Lead',
    'Marketing Lead',
    'Content Creator'
  ];

  ngOnInit() {
    this.addMemberForm = this.formBuilder.group({
      studentId: ['', [Validators.required]],
      position: ['', [Validators.required]],
      startDate: [new Date(), [Validators.required]]
    });
  }

  onSearchStudents(value: string): void {
    this.displayedStudents = this.students.filter(student => 
      student.name.toLowerCase().includes(value.toLowerCase())
    );
  }

  isFormValid(): boolean {
    return !!(this.addMemberForm?.valid && this.addMemberForm?.dirty && this.addMemberForm?.touched);
  }

  getFormValue(): { studentId: string, position: string, startDate: Date } {
    return this.addMemberForm!.value;
  }

  selectPosition(position: string): void {
    this.addMemberForm?.patchValue({ position });
  }
}
