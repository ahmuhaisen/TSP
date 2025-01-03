import { Component, inject } from '@angular/core';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { NgStyle } from '@angular/common';
import { SocietiesService } from '../../../../common/services/admin/societies.service';
import { Society } from '../../api-interfaces/society.types';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzIconModule } from 'ng-zorro-antd/icon';

@Component({
  selector: 'app-home',
  imports: [
    NgStyle,
    NzEmptyModule,
    NzSpinModule,
    NzBreadCrumbModule,
    NzIconModule
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  societiesService = inject(SocietiesService);

  societies: Society[] = [];
  isLoading = false;

  ngOnInit() {
    this.isLoading = true;
    this.societiesService.all().subscribe({
      next: (response) => {
        this.societies = response;
      },
      error: (error) => {
        this.isLoading = false;
      },
      complete: () => {
        this.isLoading = false;
      }
    });
  }
}
