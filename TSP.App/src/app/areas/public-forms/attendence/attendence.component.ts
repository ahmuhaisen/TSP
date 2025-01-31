import { ActivatedRoute } from '@angular/router';
import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

import { NzFormModule } from 'ng-zorro-antd/form';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzResultModule } from 'ng-zorro-antd/result';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzButtonComponent } from 'ng-zorro-antd/button';
import { NzCascaderModule, NzCascaderOption } from 'ng-zorro-antd/cascader';

import { PostAttendance } from './attendance.types';
import { AttendanceService } from './attendance.service';
import { SchoolService } from '../../../common/services/school.service';
import { SchoolWithDepartmentsBasicDetails } from '../../../common/types/system-tables.types';

@Component({
  selector: 'app-attendence',
  imports: [
    NzIconModule,
    NzFormModule,
    NzInputModule,
    NzButtonComponent,
    NzDividerModule,
    NzCascaderModule,
    NzResultModule,
    ReactiveFormsModule,
    NzCascaderModule,
  ],
  templateUrl: './attendence.component.html',
  providers: [
    AttendanceService
  ]
})
export class AttendenceComponent {
  eventId = '3FA85F64-5717-4562-B3FC-2C963F66AFA6';
  currentYear = new Date().getFullYear();
  nzOptions: NzCascaderOption[] = [];

  fb = inject(FormBuilder);
  activatedRoute = inject(ActivatedRoute);
  schoolService = inject(SchoolService);
  attendanceService = inject(AttendanceService);
  messageService = inject(NzMessageService);
  isSubmitting = false;
  isRegisterSucceeded = false;
  isFormEnabled = true;

  form = this.fb.group({
    fullName: [null, [Validators.required]],
    email: [null, [Validators.required, Validators.email]],
    uniNumber: [null, [Validators.required]],
    phone: [null, []],
    department: [null, [Validators.required]],
    notes: [null, [Validators.maxLength(200)]]
  });


  ngOnInit() {
    this.activatedRoute.paramMap.subscribe(params => {
      this.eventId = params.get('eventId')!;
    });

    if(this.isAttendanceSavedToLocalStorage(this.eventId)) {
      this.isRegisterSucceeded = true;
    }

    this.fetchSchools();
  }

  submitForm(): void {
    if (this.form.invalid) {
      this.messageService.warning('Please fill in all required fields');
      this.form.markAllAsTouched();
      this.form.markAsDirty();
      return;
    }

    this.postAttendance();
  }

  fetchSchools(): void {
    this.schoolService.allSchoolsWithDepartments().subscribe({
      next: (res) => {
        this.nzOptions = this.convertToCascadeOptions(res);
      }
    });
  }

  convertToCascadeOptions(schools: SchoolWithDepartmentsBasicDetails[]): NzCascaderOption[] {
    return schools.map(school => ({
      value: school.id.toString(),
      label: school.name,
      isLeaf: school.departments.length === 0,
      children: school.departments.map(department => ({
        value: department.id.toString(),
        label: department.name,
        isLeaf: true
      }))
    }));
  }

  getPostAttendanceObject() {
    return {
      eventId: this.eventId,
      fullName: this.form.value.fullName!,
      email: this.form.value.email!,
      universityNumber: this.form.value.uniNumber!,
      phoneNumber: this.form.value.phone,
      departmentId: this.form.value.department![1] ?? this.form.value.department![0],
      notes: this.form.value.notes
    } as PostAttendance
  }

  postAttendance(): void {
    this.isSubmitting = true;
    const postObject = this.getPostAttendanceObject();

    this.attendanceService.post(postObject).subscribe({
      next: _ => {
        this.messageService.success('You have successfully submitted your attendance');
        this.isRegisterSucceeded = true;
        this.saveAttendanceToLocalStorage(this.eventId);
      },
      error: _ => {
        this.isSubmitting = false;
      },
      complete: () => {
        this.form.reset();
        this.isSubmitting = false;
      }
    });
  }

  saveAttendanceToLocalStorage(eventId: string) {
    // save attendance to local storage, the saved item is an array of events ids which the user successfully registered
    const savedAttendances = localStorage.getItem('savedAttendances');
    if (!savedAttendances){
      localStorage.setItem('savedAttendances', JSON.stringify([eventId]));
      return;
    }

    const parsedSavedAttendances = JSON.parse(savedAttendances);
    parsedSavedAttendances.push(eventId);
    localStorage.setItem('savedAttendances', JSON.stringify(parsedSavedAttendances));
  }

  isAttendanceSavedToLocalStorage(eventId: string): boolean {
    const savedAttendances = localStorage.getItem('savedAttendances');
    if (!savedAttendances) return false;
    const parsedSavedAttendances = JSON.parse(savedAttendances);
    return parsedSavedAttendances.includes(eventId);
  }
}

