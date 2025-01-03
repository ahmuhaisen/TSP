import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { AdminNavbarComponent } from "./shared-components/admin-navbar/admin-navbar.component";
import { AdminFooterComponent } from "./shared-components/admin-footer/admin-footer.component";

@Component({
  selector: 'app-system-admin-area',
  imports: [
    RouterOutlet,
    AdminNavbarComponent,
    AdminFooterComponent
],
  templateUrl: './system-admin-area.component.html',
  styleUrl: './system-admin-area.component.css'
})
export class SystemAdminAreaComponent {

}
