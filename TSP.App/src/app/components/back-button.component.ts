import { Component, inject } from '@angular/core';
import { Location } from '@angular/common';

import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';

@Component({
  selector: 'app-back-button',
  standalone: true,
  imports: [
    NzButtonModule,
    NzIconModule
  ],
  template: `
    <a nz-button nzType="default" nzSize="large" (click)="goBack($event)">
      <nz-icon nzType="arrow-left" /> Go back
    </a>
  `
})
export class BackButtonComponent {
  location = inject(Location);

  goBack(event: Event) {
    event.preventDefault();
    this.location.back();
  }
}
