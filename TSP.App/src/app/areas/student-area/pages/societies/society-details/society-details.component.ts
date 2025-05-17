import { ActivatedRoute } from '@angular/router';
import { Component, inject, signal } from '@angular/core';

import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';

import { PageMode } from '../../../../../common/types/presentaion.types';
import { GenSocietyDetailsComponent } from '../../../../../components/society-details/gen-society-details.component';

@Component({
  selector: 'app-society-details',
  imports: [
    NzIconModule,
    NzDividerModule,
    GenSocietyDetailsComponent
  ],
  templateUrl: './society-details.component.html',
  styleUrl: './society-details.component.css'
})
export class SocietyDetailsComponent {
  activatedRoute = inject(ActivatedRoute);

  pageMode = signal<PageMode>('VIEW_ONLY');
  societyId = signal<string>('');

  ngOnInit() {
    this.activatedRoute.url.subscribe(url => {
      this.societyId.set(this.activatedRoute.snapshot.params['id']);
      if (url.some(u => u.path === 'manage')) {
        this.pageMode.set('STUDENT_MANAGE');
      }
      else {
        this.pageMode.set('VIEW_ONLY');
      }
    })
  }
}
