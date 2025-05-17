import { Component } from '@angular/core';

import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { BackButtonComponent } from "./back-button.component";


@Component({
  selector: 'app-not-found',
  imports: [
    NzButtonModule,
    NzIconModule,
    BackButtonComponent
  ],
  template: `
  <section class="flex items-center">
    <div class="container mx-auto px-4 py-16">
      <div class="max-w-2xl mx-auto text-center">
        <div class="relative">
          <h1 class="text-9xl font-black tracking-tighter bg-gradient-to-r from-green-400 to-secondary bg-clip-text text-transparent ">404</h1>
          <div class="absolute -bottom-1 left-0 w-full h-1 bg-gradient-to-r from-green-400 to-secondary opacity-70"></div>
        </div>
        
        <h2 class="mt-8 text-3xl font-bold text-gray-800">Page Not Found</h2>
        
        <p class="mt-4 text-gray-500 max-w-md mx-auto">
          Sorry, we can't find the page you're looking for. You'll find lots to explore on the home page.
        </p>
        
        <div class="mt-10 flex justify-center items-center space-x-4">
          <app-back-button />
        </div>
      </div>
    </div>
  </section>
  `
})
export class NotFoundComponent {
}
