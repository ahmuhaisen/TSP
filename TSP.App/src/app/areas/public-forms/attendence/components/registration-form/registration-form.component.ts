import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzCascaderModule } from 'ng-zorro-antd/cascader';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NzResultModule } from 'ng-zorro-antd/result';
import { NzAlertModule } from 'ng-zorro-antd/alert';

@Component({
  selector: 'app-registration-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NzFormModule,
    NzInputModule,
    NzButtonModule,
    NzIconModule,
    NzCascaderModule,
    NzSpinModule,
    NzResultModule,
    NzAlertModule
  ],
  template: `
    <div class="bg-white/90 backdrop-blur-sm rounded-2xl shadow-2xl p-8 animate-slide-up border border-white/20">
      @if(!isRegisterSucceeded) {
        <div class="mb-8">
          <h2 class="text-2xl font-bold text-gray-800 mb-2">Register for the Event</h2>
          <p class="text-gray-600">Fill in your details below to secure your spot</p>
        </div>

        <!-- Registration Type Selection -->
        <div class="mb-8">
          <h3 class="text-lg font-medium text-gray-700 mb-4">Choose Registration Type</h3>
          <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            <!-- Logged-in User Option -->
            <div (click)="selectRegistrationType('account')"
              [class.bg-blue-50]="registrationType === 'account'"
              [class.border-blue-500]="registrationType === 'account'"
              class="flex items-center gap-4 p-4 rounded-xl border-2 border-gray-200 hover:border-blue-400 transition-colors duration-300 cursor-pointer">
              <div [class.bg-blue-100]="registrationType === 'account'"
                class="p-3 bg-blue-50 rounded-lg">
                <i nz-icon nzType="user" nzTheme="outline" class="text-blue-600 text-xl"></i>
              </div>
              <div class="text-left">
                <h4 class="font-medium text-gray-800">Register with Account</h4>
                <p class="text-sm text-gray-600">Use your existing account details</p>
              </div>
              <div class="ml-auto">
                <div class="h-5 w-5 rounded-full border-2"
                  [class.bg-blue-500]="registrationType === 'account'"
                  [class.border-blue-500]="registrationType === 'account'"
                  [class.border-gray-300]="registrationType !== 'account'">
                  <i *ngIf="registrationType === 'account'" nz-icon nzType="check"
                    class="text-white text-xs flex justify-center items-center h-full"></i>
                </div>
              </div>
            </div>

            <!-- Anonymous Option -->
            <div (click)="selectRegistrationType('anonymous')"
              [class.bg-blue-50]="registrationType === 'anonymous'"
              [class.border-blue-500]="registrationType === 'anonymous'"
              class="flex items-center gap-4 p-4 rounded-xl border-2 border-gray-200 hover:border-blue-400 transition-colors duration-300 cursor-pointer">
              <div [class.bg-blue-100]="registrationType === 'anonymous'"
                class="p-3 bg-blue-50 rounded-lg">
                <i nz-icon nzType="user-add" nzTheme="outline" class="text-blue-600 text-xl"></i>
              </div>
              <div class="text-left">
                <h4 class="font-medium text-gray-800">Register Anonymously</h4>
                <p class="text-sm text-gray-600">Fill in your details manually</p>
              </div>
              <div class="ml-auto">
                <div class="h-5 w-5 rounded-full border-2"
                  [class.bg-blue-500]="registrationType === 'anonymous'"
                  [class.border-blue-500]="registrationType === 'anonymous'"
                  [class.border-gray-300]="registrationType !== 'anonymous'">
                  <i *ngIf="registrationType === 'anonymous'" nz-icon nzType="check"
                    class="text-white text-xs flex justify-center items-center h-full"></i>
                </div>
              </div>
            </div>
          </div>
        </div>

        @if(registrationType === 'account') {
          <!-- Account Registration Summary -->
          <div class="bg-blue-50 rounded-xl p-6 mb-8">
            @if(isLoading) {
              <div class="flex justify-center items-center py-12">
                <nz-spin nzTip="Loading account details..."></nz-spin>
              </div>
            } @else {
              <h3 class="text-lg font-medium text-gray-800 mb-4">Your Registration Details</h3>
              
              
              @if(!userDetails) {
                <div class="p-4 bg-yellow-50 border border-yellow-200 rounded-lg mb-4">
                  <p class="text-yellow-700">You need to be logged in to use this option.</p>
                  <p class="text-sm text-yellow-600 mt-2">Please log in to your account to continue.</p>
                </div>
              } @else {
                <!-- Faculty Warning - Only shown in account section -->
                @if(isFaculty) {
                <nz-alert
                  nzType="warning" 
                  nzMessage="Faculty Registration Not Allowed" 
                  nzDescription="Faculty members cannot register for student events. Please use a student account or register anonymously if you're a student."
                  nzShowIcon
                  class="mb-4">
                </nz-alert>
                }@else {

             
                <div class="flex justify-between items-center mb-4">
                  <span class="text-sm text-gray-600">Your account information</span>
                  <div class="text-xs text-blue-600 flex items-center" *ngIf="isLoading">
                    <i nz-icon nzType="loading" class="mr-1"></i> Refreshing...
                  </div>
                </div>
                <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <div class="flex items-center gap-3">
                    <div class="p-2 bg-white rounded-lg">
                      <i nz-icon nzType="user" nzTheme="outline" class="text-blue-600"></i>
                    </div>
                    <div>
                      <p class="text-sm text-gray-600">Full Name</p>
                      <p class="font-medium text-gray-800">{{userDetails?.fullName}}</p>
                    </div>
                  </div>
                  <div class="flex items-center gap-3">
                    <div class="p-2 bg-white rounded-lg">
                      <i nz-icon nzType="mail" nzTheme="outline" class="text-blue-600"></i>
                    </div>
                    <div>
                      <p class="text-sm text-gray-600">Email</p>
                      <p class="font-medium text-gray-800">{{userDetails?.email}}</p>
                    </div>
                  </div>
                </div>
                
                <!-- Submit Button for Account Registration -->
                <div class="flex justify-end mt-6">
                  <button nz-button nzType="primary" [nzLoading]="isSubmitting || isLoading" (click)="submitRegistration()"
                    [disabled]="isFaculty || isSubmitting || isLoading"
                    class="w-full md:w-auto px-8 h-12 rounded-lg bg-gradient-to-r from-blue-600 to-blue-700 hover:from-blue-700 hover:to-blue-800 text-white font-medium shadow-lg hover:shadow-xl transition-all duration-300 transform hover:-translate-y-0.5">
                    {{isLoading ? 'Loading Account...' : (isSubmitting ? 'Registering...' : 'Register with Account')}}
                  </button>
                </div>
              }
              }
            }
          </div>
        }

        <form nz-form [formGroup]="registrationForm" (ngSubmit)="submitRegistration()" class="space-y-6"
          [class.hidden]="registrationType === 'account'">
          @if(isLoading) {
            <div class="flex justify-center items-center py-12">
              <nz-spin nzTip="Loading form..."></nz-spin>
            </div>
          } @else {
            <!-- Form Grid -->
            <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
              <!-- Full Name -->
              <div class="col-span-2 md:col-span-1">
                <nz-form-item>
                  <nz-form-label nzRequired class="text-gray-700 font-medium">Full Name</nz-form-label>
                  <nz-form-control nzErrorTip="Please enter your full name">
                    <input nz-input formControlName="fullName" placeholder="Enter your full name" 
                      class="rounded-lg border-gray-200 hover:border-blue-400 focus:border-blue-400 focus:ring-2 focus:ring-blue-200" />
                  </nz-form-control>
                </nz-form-item>
              </div>

              <!-- Email -->
              <div class="col-span-2 md:col-span-1">
                <nz-form-item>
                  <nz-form-label nzRequired class="text-gray-700 font-medium">Email</nz-form-label>
                  <nz-form-control nzErrorTip="Please enter a valid email">
                    <input nz-input formControlName="email" type="email" placeholder="Enter your email"
                      class="rounded-lg border-gray-200 hover:border-blue-400 focus:border-blue-400 focus:ring-2 focus:ring-blue-200" />
                  </nz-form-control>
                </nz-form-item>
              </div>

              <!-- University Number -->
              <div class="col-span-2 md:col-span-1">
                <nz-form-item>
                  <nz-form-label nzRequired class="text-gray-700 font-medium">University
                    Number</nz-form-label>
                  <nz-form-control nzErrorTip="Please enter your university number">
                    <input nz-input formControlName="universityNumber"
                      placeholder="Enter your university number"
                      class="rounded-lg border-gray-200 hover:border-blue-400 focus:border-blue-400 focus:ring-2 focus:ring-blue-200" />
                  </nz-form-control>
                </nz-form-item>
              </div>

              <!-- School & Major -->
              <div class="col-span-2 md:col-span-1">
                <nz-form-item>
                  <nz-form-label nzRequired class="text-gray-700 font-medium">School &
                    Major</nz-form-label>
                  <nz-form-control nzErrorTip="Please select your school and major">
                    <nz-cascader [nzOptions]="schoolMajorOptions" formControlName="schoolMajor" 
                      placeholder="Select school and major"
                      class="rounded-lg border-gray-200 hover:border-blue-400 focus:border-blue-400 focus:ring-2 focus:ring-blue-200"></nz-cascader>
                  </nz-form-control>
                </nz-form-item>
              </div>

              <!-- Notes -->
              <div class="col-span-2">
                <nz-form-item>
                  <nz-form-label class="text-gray-700 font-medium">Additional Notes</nz-form-label>
                  <nz-form-control>
                    <textarea nz-input formControlName="notes" rows="4" 
                      placeholder="Any special requirements or notes"
                      class="rounded-lg border-gray-200 hover:border-blue-400 focus:border-blue-400 focus:ring-2 focus:ring-blue-200"></textarea>
                  </nz-form-control>
                </nz-form-item>
              </div>
            </div>
          }

          <!-- Submit Button -->
          <div class="flex justify-end mt-8">
            <button nz-button nzType="primary" type="submit" [nzLoading]="isSubmitting"
              [disabled]="isSubmitting || isLoading || isFaculty" 
              class="w-full md:w-auto px-8 h-12 rounded-lg bg-gradient-to-r from-blue-600 to-blue-700 hover:from-blue-700 hover:to-blue-800 text-white font-medium shadow-lg hover:shadow-xl transition-all duration-300 transform hover:-translate-y-0.5">
              Register Now
            </button>
          </div>
        </form>
      } @else {
        <div class="flex flex-col items-center justify-center py-12">
          <nz-result nzStatus="success" nzTitle="Registration Successful!"
            nzSubTitle="You have successfully registered for this event.">
          </nz-result>
        </div>
      }
    </div>
  `
})
export class RegistrationFormComponent {
  @Input() eventId: string = '';
  @Input() userDetails: any;
  @Input() isLoggedIn: boolean = false;
  @Input() isLoading: boolean = false;
  @Input() isSubmitting: boolean = false;
  @Input() isRegisterSucceeded: boolean = false;
  @Input() registrationType: 'account' | 'anonymous' = 'anonymous';
  @Input() schoolMajorOptions: any[] = [];
  @Input() isFaculty: boolean = false;

  @Output() refreshUserDetailsEvent = new EventEmitter<void>();
  @Output() selectRegistrationTypeEvent = new EventEmitter<'account' | 'anonymous'>();
  @Output() submitRegistrationEvent = new EventEmitter<void>();

  registrationForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.registrationForm = this.fb.group({
      fullName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      universityNumber: ['', [Validators.required]],
      schoolMajor: [[], [Validators.required]],
      notes: ['']
    });
  }

  refreshUserDetails() {
    this.refreshUserDetailsEvent.emit();
  }

  selectRegistrationType(type: 'account' | 'anonymous') {
    this.selectRegistrationTypeEvent.emit(type);
  }

  submitRegistration() {
    console.log('RegistrationFormComponent.submitRegistration called');

    // Validate and mark all fields as touched to show validation errors
    Object.keys(this.registrationForm.controls).forEach(key => {
      const control = this.registrationForm.get(key);
      control?.markAsTouched();
      control?.updateValueAndValidity();
      console.log(`Field ${key}: valid=${control?.valid}, value=${JSON.stringify(control?.value)}, errors=`, control?.errors);
    });

    // Log form values to debug
    console.log('Form values before submission:', this.registrationForm.value);
    console.log('Form validity:', this.registrationForm.valid);

    // Validate schoolMajor if it's anonymous registration
    if (this.registrationType === 'anonymous') {
      const schoolMajorControl = this.registrationForm.get('schoolMajor');
      if (schoolMajorControl && (!schoolMajorControl.value || !schoolMajorControl.value.length)) {
        schoolMajorControl.markAsDirty();
        schoolMajorControl.setErrors({ required: true });
        schoolMajorControl.updateValueAndValidity();
        console.log('School Major validation failed:', schoolMajorControl.errors);
        return;
      }
    }

    // Only emit if the form is valid or if using account-based registration
    if (this.registrationType === 'account' || this.registrationForm.valid) {
      // Pass the form values to the parent component
      sessionStorage.setItem('registrationFormData', JSON.stringify(this.registrationForm.value));
      this.submitRegistrationEvent.emit();
    } else {
      console.log('Form is invalid, not submitting');
    }
  }
} 