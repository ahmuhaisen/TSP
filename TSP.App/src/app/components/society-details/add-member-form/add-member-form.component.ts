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
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { SocietiesService } from '../../../areas/system-admin-area/services/societies.service';
import { debounceTime, distinctUntilChanged, Subject, switchMap } from 'rxjs';

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
    NzAlertModule,
    NzAvatarModule
  ],
  templateUrl: './add-member-form.component.html'
})
export class AddMemberFormComponent {
  societyId = input.required<string>();
  
  isStudentsLoading = false;
  displayedStudents: any[] = [];
  private searchSubject = new Subject<string>();

  formBuilder = inject(FormBuilder);
  messageService = inject(NzMessageService);
  societiesService = inject(SocietiesService);

  addMemberForm: FormGroup | undefined;

  students = [
    { id: '1', name: 'Ahmad Muhaisen' },
    { id: '2', name: 'Suhaib Saleh' },
    { id: '3', name: 'Rimawi' },
    { id: '4', name: 'Ahmad Abu Tair' },
    { id: '5', name: 'Mohammad AbuAdas' },
    { id: '6', name: 'Omar Waggad' }
  ];

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
      startDate: [new Date(), [Validators.required]],
    });

    // Setup search debounce
    this.searchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap(searchTerm => {
        this.isStudentsLoading = true;
        return this.societiesService.searchNonMemberStudents(this.societyId(), searchTerm);
      })
    ).subscribe({
      next: (students) => {
        this.displayedStudents = students;
        this.isStudentsLoading = false;
      },
      error: () => {
        this.messageService.error('Failed to load students');
        this.isStudentsLoading = false;
      }
    });
  }

  onSearchStudents(searchTerm: string): void {
    this.searchSubject.next(searchTerm);
  }

  isFormValid(): boolean {
    return this.addMemberForm!.valid && this.addMemberForm!.dirty && this.addMemberForm!.touched;
  }

  getFormValue(): { studentId: string, position: string, startDate: Date } {
    return this.addMemberForm!.value;
  }

  selectPosition(position: string): void {
    this.addMemberForm?.patchValue({ position });
  }
}
