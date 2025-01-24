import { Component } from '@angular/core';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { RequestsTableComponent } from "./requests-table/requests-table.component";
import { EventsScheduleComponent } from "./events-schedule/events-schedule.component";
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';


@Component({
  selector: 'app-events-list',
  imports: [
    NzBreadCrumbModule,
    NzButtonModule,
    NzIconModule,
    NzDividerModule,
    NzToolTipModule,
    NzDropDownModule,
    RequestsTableComponent,
    EventsScheduleComponent
],
  templateUrl: './events-list.component.html',
  styleUrl: './events-list.component.css'
})
export class EventsListComponent {
 

}