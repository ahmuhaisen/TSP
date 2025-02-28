import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { BackButtonComponent } from "./back-button.component";


@Component({
  selector: 'app-not-found',
  imports: [
    RouterLink,
    NzButtonModule,
    NzIconModule,
    BackButtonComponent
],
  template: `
  <section class="bg-white">
      <div class="py-8 px-4 mx-auto max-w-screen-xl lg:py-16 lg:px-6">
          <div class="mx-auto max-w-screen-sm text-center">
              <h1 class="mb-4 text-7xl tracking-tight font-extrabold lg:text-9xl bg-gradient-to-r from-red-500 to-red-400 bg-clip-text text-transparent">401</h1>
              <p class="mb-4 text-3xl tracking-tight font-bold text-primary-dark md:text-4xl">Access Denied</p>
              <p class="mb-4 text-md font-light text-gray-500">Sorry, you don't have access to this page.</p>
              <app-back-button class="mr-2"/>
          </div>
      </div>
  </section>
  `,
})
export class AccessDeniedComponent {
}
