import { Component, EventEmitter, inject, Output } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSegmentedModule } from 'ng-zorro-antd/segmented';
import { AuthService, LoginRequest, UserType } from '../../../common/services/auth.service';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { signal } from '@angular/core';
import { NgModel } from '@angular/forms';
@Component({
  selector: 'app-login',
  imports: [
    RouterLink,
    CommonModule,
    ReactiveFormsModule, NzButtonModule, NzCheckboxModule, NzFormModule, NzInputModule, NzSegmentedModule, NzIconModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
  providers: [
    NgModel
  ]
})
export class LoginComponent {
  passwordVisible = false;
  password?: string;

  selectedUserType = signal<UserType>('FacultyMember');
  userTypes = ['Student', 'FacultyMember'];
  @Output() handleUserTypeChange = new EventEmitter<UserType>();

  fb = new FormBuilder();
  authService = inject(AuthService);
  constructor(
    private route: ActivatedRoute
  ) {
  }
  loginForm = this.fb.group({
    email: this.fb.control('', [Validators.required, Validators.email]),
    password: this.fb.control('', [Validators.required]),
  });
  ngOnInit() {
    this.selectedUserType.set(this.route.snapshot.queryParamMap.get('userType') as UserType);

  }
  submitForm(): void {
    if (this.loginForm.invalid) {
      Object.values(this.loginForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }

    const request = this.loginForm.value as LoginRequest;

    this.authService.login(request, this.selectedUserType());
  }

  onUserTypeChange(e: string | number): void {
    this.selectedUserType.set(e === 'FacultyMember' ? 'FacultyMember' : 'Student');
    this.handleUserTypeChange.emit(this.selectedUserType());
  }
}
