import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';

import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { TruncatePipe } from '../../../../../common/pipes/truncate.pipe';
import { SocietiesService } from '../../../services/societies.service';
import { SocietyBasicDetails } from '../../../api-interfaces/society.types';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { NzEmptyModule } from 'ng-zorro-antd/empty';


@Component({
  selector: 'app-societies-list',
  imports: [
    RouterLink,
    TruncatePipe,
    NzBreadCrumbModule,
    NzButtonModule,
    NzIconModule,
    NzDividerModule,
    NzAvatarModule,
    NzEmptyModule
  ],
  templateUrl: './societies-list.component.html',
  styleUrl: './societies-list.component.css'
})
export class SocietiesListComponent {

  advisorSocieties: SocietyBasicDetails[] = [];
  otherSocieties: SocietyBasicDetails[] = [];

  societiesService = inject(SocietiesService);

  ngOnInit() {
    this.societiesService.advisorSocieties().subscribe(res => this.advisorSocieties = res);
    this.societiesService.otherSocieties().subscribe(res => this.otherSocieties = res);
  }

}
