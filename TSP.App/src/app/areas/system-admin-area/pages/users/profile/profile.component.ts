import { Component } from '@angular/core';
import { NzDividerComponent } from 'ng-zorro-antd/divider';
import { ContainerBlockComponent } from "../../../../../components/container-block.component";

@Component({
  selector: 'app-profile',
  imports: [
    NzDividerComponent,
    ContainerBlockComponent
],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent {

}
