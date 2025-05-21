import { Component, Input } from '@angular/core';
import { EventDetails } from '../../models/event-details.model';
import { CommonModule } from '@angular/common';
import { NzIconModule } from 'ng-zorro-antd/icon';

export type ColorTheme = 'green' | 'blue' | 'purple' | 'orange' | 'red';

@Component({
  selector: 'app-event-hero',
  standalone: true,
  imports: [
    CommonModule,
    NzIconModule
  ],
  template: `
    <div
      [ngClass]="getThemeClasses()"
      class="relative min-h-[80vh] flex items-center justify-center overflow-hidden">
      <div class="absolute inset-0 bg-[url('/bg-contest.jpg')] bg-cover bg-center mix-blend-overlay opacity-30"></div>
      <div class="absolute inset-0 bg-gradient-to-b from-black/80 via-black/60 to-transparent backdrop-blur-sm"></div>
      
      <!-- Content -->
      <div class="container relative z-10 mx-auto px-4 md:px-8 text-center text-white">
        <!-- Site Identity -->
        <div class="mb-12 flex justify-center items-center">
            <img src="tsp_logo_white.png" alt="TSP Logo" class="h-12 md:h-16 mr-4">
            <div class="text-left">
                <h2 class="text-xl md:text-2xl font-semibold text-white">The Societies Portal</h2>
                <p class="text-sm md:text-base" [ngClass]="getAccentTextClass()">{{PageTitle}}</p>
            </div>
        </div>
        
        <h1
            class="text-4xl md:text-6xl font-bold mb-4 animate-slide-up bg-clip-text text-transparent"
            [ngClass]="getGradientTextClass()">
            {{event?.name}}</h1>
        <p class="text-xl md:text-2xl text-gray-200 mb-6 animate-slide-up-delay">{{event?.type}}</p>
      </div>
    </div>
  `
})
export class EventHeroComponent {
  @Input() event: EventDetails | undefined;
  @Input() PageTitle: string | undefined;
  @Input() theme: ColorTheme = 'green';

  getThemeClasses(): string {
    const themeMap: Record<ColorTheme, string> = {
      'green': 'bg-gradient-to-br from-green-900 via-green-800 to-green-600',
      'blue': 'bg-gradient-to-br from-blue-900 via-blue-800 to-blue-600',
      'purple': 'bg-gradient-to-br from-purple-900 via-purple-800 to-purple-600',
      'orange': 'bg-gradient-to-br from-orange-900 via-orange-800 to-orange-600',
      'red': 'bg-gradient-to-br from-red-900 via-red-800 to-red-600'
    };
    return themeMap[this.theme] || themeMap['green'];
  }

  getGradientTextClass(): string {
    const gradientMap: Record<ColorTheme, string> = {
      'green': 'bg-gradient-to-r from-white to-green-100',
      'blue': 'bg-gradient-to-r from-white to-blue-100',
      'purple': 'bg-gradient-to-r from-white to-purple-100',
      'orange': 'bg-gradient-to-r from-white to-orange-100',
      'red': 'bg-gradient-to-r from-white to-red-100'
    };
    return gradientMap[this.theme] || gradientMap['green'];
  }

  getAccentTextClass(): string {
    const textMap: Record<ColorTheme, string> = {
      'green': 'text-green-200',
      'blue': 'text-blue-200',
      'purple': 'text-purple-200',
      'orange': 'text-orange-200',
      'red': 'text-red-200'
    };
    return textMap[this.theme] || textMap['green'];
  }
}