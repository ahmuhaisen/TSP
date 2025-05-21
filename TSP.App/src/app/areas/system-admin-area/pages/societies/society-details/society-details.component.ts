import { Component, inject, signal } from '@angular/core';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { GenSocietyDetailsComponent } from '../../../../../components/society-details/gen-society-details.component';
import { ActivatedRoute } from '@angular/router';
import { PageMode } from '../../../../../common/types/presentaion.types';

@Component({
  selector: 'app-society-details',
  imports: [
    NzIconModule,
    NzButtonModule,
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
        this.pageMode.set('ADMIN_MANAGE');
      }
      else {
        this.pageMode.set('VIEW_ONLY');
      }
    });
  }
}
