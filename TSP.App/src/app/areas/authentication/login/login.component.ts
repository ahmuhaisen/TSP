import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSegmentedModule } from 'ng-zorro-antd/segmented';

@Component({
  selector: 'app-login',
  imports: [
    ReactiveFormsModule, NzButtonModule, NzCheckboxModule, NzFormModule, NzInputModule,NzSegmentedModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  selectedUserType: 'facultyMember' | 'student' = 'student';
  userTypes = ['Student', 'Faculty Member'];

  fb = new FormBuilder();

  loginForm = this.fb.group({
    email: this.fb.control('', [Validators.required, Validators.email]),
    password: this.fb.control('', [Validators.required]),
  });

  submitForm(): void {
    if (this.loginForm.valid) {
      console.log('submit', this.loginForm.value);
    } else {
      Object.values(this.loginForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }

  handleUserTypeChange(e: string | number): void {
    this.selectedUserType = e === 'facultyMember' ? 'facultyMember' : 'student';
  }
}
