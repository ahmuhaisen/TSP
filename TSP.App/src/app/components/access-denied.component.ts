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
  <section class="bg-gradient-to-b from-white to-gray-50 min-h-screen flex items-center">
    <div class="container mx-auto px-4 py-16">
      <div class="max-w-2xl mx-auto text-center">
        <div class="relative">
          <h1 class="text-9xl font-black tracking-tighter bg-gradient-to-r from-red-500 to-pink-600 bg-clip-text text-transparent 
                     animate-pulse">401</h1>
          <div class="absolute -bottom-1 left-0 w-full h-1 bg-gradient-to-r from-red-500 to-pink-600 opacity-70"></div>
        </div>
        
        <h2 class="mt-8 text-3xl font-bold text-gray-800">Access Denied</h2>
        
        <p class="mt-4 text-gray-500 max-w-md mx-auto">
          Sorry, you don't have permission to access this page. Please contact your administrator if you need assistance.
        </p>
        
        <div class="mt-10 flex justify-center items-center space-x-4">
          <app-back-button />
        </div>
      </div>
    </div>
  </section>
  `,
  styles: [`
    :host {
      display: block;
    }
    
    .animate-pulse {
      animation: pulse 2s cubic-bezier(0.4, 0, 0.6, 1) infinite;
    }
    
    @keyframes pulse {
      0%, 100% {
        opacity: 1;
      }
      50% {
        opacity: 0.7;
      }
    }
  `]
})
export class AccessDeniedComponent {
}
