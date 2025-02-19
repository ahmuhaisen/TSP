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
    NzToolTipModule
  ],
  templateUrl: './events-list.component.html',
  styleUrl: './events-list.component.css'
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


  openEventRequestModal() {
    this.isEventRequestModalVisible = true;
  }

  handleEventRequestModalCancel() {
    this.isEventRequestModalVisible = false;
  }

  pre(): void {
    this.currentStep -= 1;
    //this.changeContent();
  }

  next(): void {
    if (this.currentStep === 0) {
      if (this.selectedSocietyId === 0) {
        this.messageService.error('Please select a society');
        return;
      }
    }

    this.currentStep += 1;
    //this.changeContent();
  }

  done(): void {
    console.log('done');
    this.isEventRequestModalVisible = false;
  }

  selectSociety(societyId: number) {
    this.selectedSocietyId = societyId;
  }
}
