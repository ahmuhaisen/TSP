import { Component, input } from '@angular/core';
import { INotification } from '../../../../../../common/types/notification.types';
import { DatePipe } from '@angular/common';
import { TruncatePipe } from '../../../../../../common/pipes/truncate.pipe';
import { CapitalizeFirstPipe } from '../../../../../../common/pipes/capitalize-first.pipe';

@Component({
  selector: 'app-notification-line',
  imports: [
    DatePipe,
    TruncatePipe,
    CapitalizeFirstPipe
  ],
  templateUrl: './notification-line.component.html',
  styleUrl: './notification-line.component.css'
})
export class NotificationLineComponent {
  content = input.required<INotification>();
}
