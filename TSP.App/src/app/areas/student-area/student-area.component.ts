import { RouterOutlet } from '@angular/router';
import { Component, inject } from '@angular/core';

import { NzProgressModule } from 'ng-zorro-antd/progress';
import { ProgressbarLoaderComponent } from "../../components/progressbar-loader.component";
import { ProgressbarLoaderService } from '../../common/services/progressbar-loader.service';

import { BreadcrumbComponent } from 'xng-breadcrumb';
import { StudentNavbarComponent } from "./shared-components/student-navbar/student-navbar.component";
import { FooterComponent } from "../../components/footer.component";
import { NotificationHubService } from '../../common/services/notification-hub.service';

@Component({
  selector: 'app-student-area',
  imports: [
    RouterOutlet,
    NzProgressModule,
    ProgressbarLoaderComponent,
    BreadcrumbComponent,
    StudentNavbarComponent,
    FooterComponent
  ],
  templateUrl: './student-area.component.html',
  styleUrl: './student-area.component.css'
})
export class StudentAreaComponent {

  progressbarService = inject(ProgressbarLoaderService);
  notificationHubService = inject(NotificationHubService);

  ngOnInit() {
    this.notificationHubService.startConnection();

    this.notificationHubService.onNotification((data) => {
      console.log('Notification received:', data);
    });

  }
}
