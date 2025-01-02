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
              <h1 class="mb-4 text-7xl tracking-tight font-extrabold lg:text-9xl bg-gradient-to-r from-green-400 to-secondary bg-clip-text text-transparent">404</h1>
              <p class="mb-4 text-3xl tracking-tight font-bold text-primary-dark md:text-4xl">Page not found</p>
              <p class="mb-4 text-md font-light text-gray-500">Sorry, we can't find that page. You'll find lots to explore on the home page. </p>
              <app-back-button class="mr-2"/>
              <a nz-button nzType="primary" nzSize="large" routerLink="/">
                Go to Home
              </a>
          </div>
      </div>
  </section>
  `,
})
export class NotFoundComponent {
}
