import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SocietiesService } from '../../common/services/admin/societies.service';
import { Society } from './api-interfaces/society.types';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { NgStyle } from '@angular/common';

@Component({
  selector: 'app-system-admin-area',
  imports: [
    NgStyle,
    RouterOutlet,
    NzEmptyModule
  ],
  templateUrl: './system-admin-area.component.html',
  styleUrl: './system-admin-area.component.css'
})
export class SystemAdminAreaComponent {

  societiesService = inject(SocietiesService);

  societies: Society[] = [];

  ngOnInit() {
    this.societiesService.all().subscribe({
      next: (response) => {
        console.log(response);
        this.societies = response;
      },
      error: (error) => {
        console.log(error);
      }
    });
  }
}
