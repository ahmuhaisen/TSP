import { Component, inject, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CommonModule, DatePipe } from '@angular/common';

import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzSpinModule } from 'ng-zorro-antd/spin';

import { TruncatePipe } from '../../../../../common/pipes/truncate.pipe';
import { SocietiesService } from '../../../services/societies.service';
import { Society, SocietyWithAdvisor } from '../../../../system-admin-area/api-interfaces/society.types';
import { environment } from '../../../../../../environments/environment';

@Component({
  selector: 'app-societies-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    TruncatePipe,
    NzButtonModule,
    NzIconModule,
    NzDividerModule,
    NzAvatarModule,
    NzEmptyModule,
    NzTableModule,
    NzSpinModule,
    DatePipe
  ],
  templateUrl: './societies-list.component.html',
  styleUrl: './societies-list.component.css'
})
export class SocietiesListComponent implements OnInit {
  societiesPicturesBaseUrl = environment.gitHubSocietiesPicturesURL;
  societies: SocietyWithAdvisor[] = [];
  loading = true;

  societiesService = inject(SocietiesService);

  ngOnInit() {
    this.loadSocieties();
  }

  loadSocieties() {
    this.loading = true;
    this.societiesService.all().subscribe(res => {
      this.societies = res;
      this.loading = false;
    });
  }

  delete(id: string) {
    if (confirm('Are you sure you want to delete this society?')) {
      this.societiesService.delete(id).subscribe(() => {
        this.loadSocieties();
      });
    }
  }
} 