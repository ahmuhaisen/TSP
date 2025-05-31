import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzMessageService } from 'ng-zorro-antd/message';
import { AuthService } from '../../../common/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [
    RouterLink,
    CommonModule,
    ReactiveFormsModule,
    NzButtonModule,
    NzFormModule,
    NzInputModule,
    NzIconModule,
  ],
  templateUrl: 'forgot-password.component.html',
  styleUrl: 'forgot-password.component.css'
})
export class ForgotPasswordComponent {
  isLoading = false;
  forgotPasswordForm;

  constructor(
    private fb: FormBuilder,
    private message: NzMessageService,
    private authService: AuthService,
    private router: Router
  ) {
    this.forgotPasswordForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

  submitForm(): void {
    if (this.forgotPasswordForm.invalid) {
      Object.values(this.forgotPasswordForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
      return;
    }

    this.isLoading = true;

    // TODO: Call your auth service to request password reset
    // For now, we'll simulate the API call
    const email = this.forgotPasswordForm.get('email')?.value || "";
    const url = `${window.location.origin}/authentication/reset-password/`;
    this.authService.getResetTokenAndId(email, url)
      .subscribe(data => {
        console.log(data)
        this.isLoading = false;
        this.message.success('Password reset instructions have been sent to your email.');
      },
        error => {
          this.isLoading = false;
          this.message.error("something went wrong, try against please.")
        }
      )
  }
} 