import { Component, inject } from '@angular/core';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

import { AuthService, LoginRequest } from '../../../../common/services/auth.service';

import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSegmentedModule } from 'ng-zorro-antd/segmented';


@Component({
  selector: 'app-login',
  imports: [
    RouterLink,
    CommonModule,
    ReactiveFormsModule, NzButtonModule, NzCheckboxModule, NzFormModule, NzInputModule,NzSegmentedModule, NzIconModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  passwordVisible = false;
  password?: string;

  fb = new FormBuilder();
  authService = inject(AuthService);

  loginForm = this.fb.group({
    email: this.fb.control('', [Validators.required, Validators.email]),
    password: this.fb.control('', [Validators.required]),
  });

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

    this.authService.login(request, "SuperAdmin");
  }
}
