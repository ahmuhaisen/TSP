import { Component, Input } from '@angular/core';
import { EventDetails } from '../../models/event-details.model';
import { CommonModule, DatePipe } from '@angular/common';
import { NzIconModule } from 'ng-zorro-antd/icon';

@Component({
  selector: 'app-event-info-bar',
  standalone: true,
  imports: [
    CommonModule,
    NzIconModule,
    DatePipe
  ],
  template: `
    <div class="relative -mt-8">
      <div class="container mx-auto px-4 md:px-8">
        <div class="bg-white/90 backdrop-blur-sm rounded-2xl shadow-2xl p-6 animate-slide-up border border-white/20">
          <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
            <div class="flex items-center gap-3">
              <div class="p-2 bg-purple-50 rounded-lg">
                <i nz-icon nzType="calendar" nzTheme="outline" class="text-purple-600"></i>
              </div>
              <div>
                <p class="font-medium text-gray-800">Date</p>
                <p class="text-sm text-gray-600">{{event?.date | date:'fullDate'}}</p>
              </div>
            </div>

            <div class="flex items-center gap-3">
              <div class="p-2 bg-pink-50 rounded-lg">
                <i nz-icon nzType="clock-circle" nzTheme="outline" class="text-pink-600"></i>
              </div>
              <div>
                <p class="font-medium text-gray-800">Time</p>
                <p class="text-sm text-gray-600">{{event?.startTime | date: 'shortTime'}} - {{event?.endTime | date:'shortTime'}}</p>
              </div>
            </div>

            <div class="flex items-center gap-3">
              <div class="p-2 bg-indigo-50 rounded-lg">
                <i nz-icon nzType="environment" nzTheme="outline" class="text-indigo-600"></i>
              </div>
              <div>
                <p class="font-medium text-gray-800">Location</p>
                <p class="text-sm text-gray-600">{{event?.location}}</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  `
})
export class EventInfoBarComponent {
  @Input() event: EventDetails | undefined;
} 