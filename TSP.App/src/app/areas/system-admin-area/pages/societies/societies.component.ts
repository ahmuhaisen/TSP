import { Component } from '@angular/core';
import { NzBreadCrumbComponent, NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';

@Component({
  selector: 'app-societies',
  imports: [
    NzBreadCrumbModule,
    NzButtonModule,
    NzIconModule,
    
  ],
  templateUrl: './societies.component.html',
  styleUrl: './societies.component.css'
})
export class SocietiesComponent {

}
