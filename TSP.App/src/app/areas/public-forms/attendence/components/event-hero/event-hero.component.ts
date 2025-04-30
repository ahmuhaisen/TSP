import { Component, Input } from '@angular/core';
import { EventDetails } from '../../models/event-details.model';
import { CommonModule } from '@angular/common';
import { NzIconModule } from 'ng-zorro-antd/icon';

@Component({
  selector: 'app-event-hero',
  standalone: true,
  imports: [
    CommonModule,
    NzIconModule
  ],
  template: `
    <div
      class="relative min-h-[80vh] flex items-center justify-center overflow-hidden bg-gradient-to-br from-blue-900 via-blue-800 to-blue-600">
      <div class="absolute inset-0 bg-[url('/bg-contest.jpg')] bg-cover bg-center mix-blend-overlay opacity-30"></div>
      <div class="absolute inset-0 bg-gradient-to-b from-black/80 via-black/60 to-transparent backdrop-blur-sm"></div>
      
      <!-- Content -->
      <div class="container relative z-10 mx-auto px-4 md:px-8 text-center text-white">
        <!-- Site Identity -->
        <div class="mb-12 flex justify-center items-center">
            <img src="tsp_logo_white.png" alt="TSP Logo" class="h-12 md:h-16 mr-4">
            <div class="text-left">
                <h2 class="text-xl md:text-2xl font-semibold text-white">The Societies Portal</h2>
                <p class="text-sm md:text-base text-blue-200">Event Registration</p>
            </div>
        </div>
        
        <h1
            class="text-4xl md:text-6xl font-bold mb-4 animate-slide-up bg-clip-text text-transparent bg-gradient-to-r from-white to-blue-100">
            {{event?.name}}</h1>
        <p class="text-xl md:text-2xl text-gray-200 mb-6 animate-slide-up-delay">{{event?.type}}</p>
      </div>
    </div>
  `
})
export class EventHeroComponent {
  @Input() event: EventDetails | undefined;
} 