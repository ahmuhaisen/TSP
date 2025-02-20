import { CommonModule, NgClass, NgStyle } from '@angular/common';
import { Component, inject } from '@angular/core';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { DisabledTimeFn, DisabledTimePartial, NzDatePickerModule, NzRangePickerComponent } from 'ng-zorro-antd/date-picker';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzStepsModule } from 'ng-zorro-antd/steps';
import { differenceInCalendarDays, setHours } from 'date-fns';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzSwitchModule } from 'ng-zorro-antd/switch';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-events-list',
  imports: [
    NzFormModule,
    NzInputModule,
    NzSelectModule,
    NzSwitchModule,
    NzDatePickerModule,
    NzIconModule,
    NzButtonModule,
    NzDividerModule,
    NzModalModule,
    NzStepsModule,
    CommonModule,
    NzToolTipModule,
    ReactiveFormsModule
  ],
  templateUrl: './events-list.component.html',
  styleUrl: './events-list.component.css',
  styles: [`
    ::ng-deep .date-time-dropdown {
      max-width: calc(100vw - 32px);
    }
    ::ng-deep .ant-picker-dropdown, ::ng-deep .ant-picker-panel-container {
      max-width: 100%;
    }
    ::ng-deep .ant-picker-panels {
      flex-wrap: wrap;
      gap: 8px;
    }
  `]
})
export class EventsListComponent {

  isEventRequestModalVisible = false;
  currentStep = 0;
  selectedSocietyId: number = 0;

  messageService = inject(NzMessageService);

  today = new Date();
  timeDefaultValue = setHours(new Date(), 8);
  disabledDate = (current: Date): boolean =>
    differenceInCalendarDays(current, this.today) < 0;

  disabledRangeTime: DisabledTimeFn = (_value, type?: DisabledTimePartial) => {
    return {
      nzDisabledHours: () => this.range(0, 8), // Disable hours before 8 AM and after 8 PM
      nzDisabledMinutes: () => [], // Allow selecting all minutes
      nzDisabledSeconds: () => this.range(1, 60) // Remove seconds selection
    };
  };

  range(start: number, end: number): number[] {
    const result: number[] = [];
    for (let i = start; i < end; i++) {
      result.push(i);
    }
    return result;
  }

  allNumbersNotDividedBy5(): number[] {
    const result: number[] = [];
    for (let i = 0; i < 60; i++) {
      if (i % 5 !== 0) {
        result.push(i);
      }
    }
    return result;
  }

  eventRequestForm!: FormGroup;
  private fb = inject(FormBuilder);

  constructor() {
    this.initForm();
  }

  private initForm(): void {
    this.eventRequestForm = this.fb.group({
      societyId: [null, Validators.required],
      title: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      description: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(500)]],
      location: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      eventType: ['', Validators.required],
      dateRange: [null, Validators.required],
      hasAttendanceForm: [false]
    });
  }

  openEventRequestModal() {
    // Set default start time to next day at 9:00 AM
    const startDate = new Date();
    startDate.setDate(startDate.getDate() + 1);
    startDate.setHours(9, 0, 0, 0);
    
    // Set default end time to same day at 11:00 AM
    const endDate = new Date(startDate);
    endDate.setHours(11, 0, 0, 0);

    this.eventRequestForm.patchValue({
      dateRange: [startDate, endDate]
    });
    
    this.isEventRequestModalVisible = true;
  }

  handleEventRequestModalCancel() {
    this.isEventRequestModalVisible = false;
    this.resetForm();
  }

  pre(): void {
    this.currentStep -= 1;
    //this.changeContent();
  }

  next(): void {
    if (this.currentStep === 0) {
      if (!this.eventRequestForm.get('societyId')?.valid) {
        this.messageService.error('Please select a society');
        return;
      }
    }
    this.currentStep += 1;
    //this.changeContent();
  }

  done(): void {
    if (this.eventRequestForm.valid) {
      const formValue = this.eventRequestForm.value;
      
      const eventRequest = {
        societyId: formValue.societyId,
        title: formValue.title,
        description: formValue.description,
        location: formValue.location,
        type: formValue.eventType,
        startDate: formValue.dateRange[0],
        endDate: formValue.dateRange[1],
        hasAttendanceForm: formValue.hasAttendanceForm
      };

      console.log('Event Request:', eventRequest);
      
      this.isEventRequestModalVisible = false;
      this.messageService.success('Event request submitted successfully');
      this.resetForm();
    } else {
      Object.keys(this.eventRequestForm.controls).forEach(key => {
        const control = this.eventRequestForm.get(key);
        if (control?.invalid) {
          control.markAsTouched();
        }
      });
      this.messageService.error('Please fill all required fields correctly');
    }
  }

  selectSociety(societyId: number) {
    this.selectedSocietyId = societyId;
    this.eventRequestForm.patchValue({ societyId });
  }

  private resetForm(): void {
    this.currentStep = 0;
    this.selectedSocietyId = 0;
    this.eventRequestForm.reset();
    this.initForm();
  }
}
