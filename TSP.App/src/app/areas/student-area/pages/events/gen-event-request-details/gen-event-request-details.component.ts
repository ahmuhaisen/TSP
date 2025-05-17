import { Component } from '@angular/core';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { EventRequestDetailsComponent } from "../../../../../components/event-request-details/event-request-details.component";

@Component({
  selector: 'app-gen-event-request-details',
  imports: [
    NzIconModule,
    EventRequestDetailsComponent
],
  templateUrl: './gen-event-request-details.component.html'
})
export class GenEventRequestDetailsComponent {

}
