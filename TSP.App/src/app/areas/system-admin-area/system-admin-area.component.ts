import { RouterOutlet } from '@angular/router';
import { Component, inject } from '@angular/core';

import { NzProgressModule } from 'ng-zorro-antd/progress';
import { ProgressbarLoaderComponent } from "../../components/progressbar-loader.component";
import { ProgressbarLoaderService } from '../../common/services/progressbar-loader.service';
import { AdminNavbarComponent } from "./shared-components/admin-navbar/admin-navbar.component";
import { BreadcrumbComponent } from 'xng-breadcrumb';
import { FooterComponent } from "../../components/footer.component";
import { ProfileImageAlertComponent } from './shared-components/profile-image-alert/profile-image-alert.component';

@Component({
  selector: 'app-system-admin-area',
  imports: [
    RouterOutlet,
    AdminNavbarComponent,
    NzProgressModule,
    ProgressbarLoaderComponent,
    BreadcrumbComponent,
    FooterComponent,
    ProfileImageAlertComponent
],
  templateUrl: './system-admin-area.component.html',
  styleUrl: './system-admin-area.component.css'
})
export class SystemAdminAreaComponent {

  progressbarService = inject(ProgressbarLoaderService);

}
