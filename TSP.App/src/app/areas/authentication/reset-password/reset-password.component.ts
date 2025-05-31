import { Component, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzMessageService } from 'ng-zorro-antd/message';
import { ProfilesService } from '../../../common/services/profiles.service';
import { SecureLocalStorageService } from '../../../common/services/secure-local-storage.service';
@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NzButtonModule,
    NzFormModule,
    NzInputModule,
    NzIconModule,
  ],
  templateUrl: 'reset-password.component.html',
  styleUrl: 'reset-password.component.css'
})
export class ResetPasswordComponent implements OnInit {
  passwordVisible = false;
  confirmPasswordVisible = false;
  password?: string;
  confirmPassword?: string;
  isLoading = false;
  token: string | null = null;
  resetPasswordForm;
  currentUserId: string = "";
  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private message: NzMessageService,
    private profileService: ProfilesService,
    private localStorageService: SecureLocalStorageService
  ) {
    this.resetPasswordForm = this.fb.group({
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]]
    }, {
      validators: this.passwordMatchValidator
    });
  }

  ngOnInit() {
    this.token = this.route.snapshot.queryParamMap.get('token');
    this.currentUserId = this.route.snapshot.paramMap.get("userId") || "";
    console.log(this.token);
    console.log(this.currentUserId);
    if (!this.token) {
      this.message.error('Invalid or expired reset link.');
      this.router.navigate(['authentication/login']);
    }
    else {

      this.localStorageService.setItem("token", (this.token))
    }
  }

  private passwordMatchValidator(form: any) {
    const password = form.get('password');
    const confirmPassword = form.get('confirmPassword');

    if (password.value !== confirmPassword.value) {
      confirmPassword.setErrors({ passwordMismatch: true });
    } else {
      confirmPassword.setErrors(null);
    }
    return null;
  }

  submitForm(): void {
    if (this.resetPasswordForm.invalid) {
      Object.values(this.resetPasswordForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
      return;
    }

    this.isLoading = true;
    this.profileService.updatePassword(this.currentUserId, this.password || "")
      .subscribe({
        next: data => {
          this.isLoading = false;
          this.message.success("password has been updated")
          this.router.navigate(['authentication/login']);
        },
        error: error => {
          this.isLoading = false;
          this.message.error("Failed to update password. Please try again.");
        }
      })
  }
} 
