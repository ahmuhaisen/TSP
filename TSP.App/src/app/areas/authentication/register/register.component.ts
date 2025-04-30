import { Component, EventEmitter, inject, Output } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSegmentedModule } from 'ng-zorro-antd/segmented';
import { AuthService, FacultyRegisterRequest, StudentRegisterRequest, UserType } from '../../../common/services/auth.service';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { RouterLink } from '@angular/router';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzMessageService } from 'ng-zorro-antd/message';
import { RankService } from '../../../common/services/rank.service';
import { SchoolService } from '../../../common/services/school.service';
import { Rank, SchoolWithDepartmentsBasicDetails } from '../../../common/types/system-tables.types';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  imports: [
    RouterLink, NzSelectModule, NzDividerModule, CommonModule,
    ReactiveFormsModule, NzButtonModule, NzCheckboxModule, NzFormModule, NzInputModule, NzSegmentedModule, NzIconModule
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  passwordVisible = false;
  password?: string;

  selectedUserType: UserType = 'Student';
  userTypes = ['Student', 'FacultyMember'];
  @Output() handleUserTypeChange = new EventEmitter<UserType>();

  ranks: Rank[] = [];
  schoolsAndDepartments: SchoolWithDepartmentsBasicDetails[] = [
    {
      id: 1,
      name: "King Abdullah II School for Information Technology",
      departments: [
        { id: 1, name: "Computer Science" },
        { id: 2, name: "Computer Information Systems" },
        { id: 3, name: "Information Technology" },
        { id: 4, name: "Artificial Intelligence" },
      ]
    },
    {
      id: 2,
      name: "School of Engineering",
      departments: [
        { id: 5, name: "Mechanical Engineering" },
        { id: 6, name: "Civil Engineering" },
        { id: 7, name: "Electrical Engineering" },
        { id: 8, name: "Electronics Engineering" },
      ]
    }
  ]

  fb = new FormBuilder();
  authService = inject(AuthService);
  rankService = inject(RankService);
  schoolService = inject(SchoolService);
  messageService = inject(NzMessageService);

  registerForm = this.fb.group({
    email: this.fb.control('', [Validators.required, Validators.email]),
    password: this.fb.control('', [Validators.required]),
    firstName: this.fb.control('', [Validators.required]),
    lastName: this.fb.control('', [Validators.required]),
    gender: this.fb.control('', [Validators.required]),
    departmentId: this.fb.control('', [Validators.required]),
    employeeNumber: this.fb.control('', []),
    rankId: this.fb.control('', []),
    universityNumber: this.fb.control('', []),
  });

  ngOnInit() {
    this.rankService.all().subscribe({
      next: (res) => {
        this.ranks = res;
      }
    });

    this.schoolService.allSchoolsWithDepartments().subscribe({
      next: (res) => {
        this.schoolsAndDepartments = res;
      }
    })
  }

  submitFacultyForm() {
    if (this.selectedUserType == 'FacultyMember') {
      // check if employeeNumber and rankId are valid
      if (this.registerForm.get('employeeNumber')?.invalid || this.registerForm.get('rankId')?.invalid
          || this.registerForm.get('employeeNumber')?.value == '' || this.registerForm.get('rankId')?.value == ''
          ) {
        this.messageService.warning('Please enter a valid employee number and rank!');
        return;
      }

      this.registerFacultyMember();
    }
    else {
      // check if universityNumber is valid
      if (this.registerForm.get('universityNumber')?.invalid || this.registerForm.get('universityNumber')?.value == '') {
        this.messageService.warning('Please enter a valid university number!');
        return;
      }

      this.registerStudent();
    }
  }

  registerStudent() {
    const studentRegRequest: StudentRegisterRequest = {
      email: this.registerForm.value.email!,
      password: this.registerForm.value.password!,
      firstName: this.registerForm.value.firstName!,
      lastName: this.registerForm.value.lastName!,
      gender: this.registerForm.value.gender!,
      departmentId: +this.registerForm.value.departmentId!,
      universityNumber: this.registerForm.value.universityNumber!
    };

    console.table(studentRegRequest);

    this.authService.registerStudent(studentRegRequest);
  }

  registerFacultyMember() {
    const facultyRegRequest: FacultyRegisterRequest = {
      email: this.registerForm.value.email!,
      password: this.registerForm.value.password!,
      firstName: this.registerForm.value.firstName!,
      lastName: this.registerForm.value.lastName!,
      gender: this.registerForm.value.gender!,
      departmentId: +this.registerForm.value.departmentId!,
      employeeNumber: this.registerForm.value.employeeNumber!,
      rankId: +this.registerForm.value.rankId!
    };

    console.table(facultyRegRequest);

    this.authService.registerFaculty(facultyRegRequest);
  }

  onUserTypeChange(e: string | number) {
    this.selectedUserType = e === 'FacultyMember' ? 'FacultyMember' : 'Student';
    this.handleUserTypeChange.emit(this.selectedUserType);
  }
}

