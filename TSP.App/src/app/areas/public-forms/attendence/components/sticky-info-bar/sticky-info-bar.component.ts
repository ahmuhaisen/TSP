import { Component, Input, OnInit, AfterViewInit, Renderer2, HostListener } from '@angular/core';
import { EventDetails } from '../../models/event-details.model';
import { CommonModule, DatePipe } from '@angular/common';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzButtonModule } from 'ng-zorro-antd/button';

@Component({
  selector: 'app-sticky-info-bar',
  standalone: true,
  imports: [
    CommonModule,
    NzIconModule,
    NzButtonModule,
    DatePipe
  ],
  template: `
    <div class="fixed top-0 left-0 right-0 z-50 transition-all duration-300 transform -translate-y-full" id="stickyInfoBar">
      <div class="bg-white shadow-md border-b border-gray-200">
        <div class="container mx-auto px-4 md:px-8 py-3">
          <!-- Desktop View (md and up) -->
          <div class="hidden md:flex items-center justify-between">
            <div class="flex items-center">
              <img src="tsp-logo.png" alt="TSP Logo" class="h-8 mr-3">
              <h3 class="font-medium text-gray-800 mr-6 line-clamp-1">
                Event Registration |
                {{event?.name}}
              </h3>
            </div>
            <div class="flex items-center space-x-6">
              <div class="flex items-center">
                <i nz-icon nzType="calendar" nzTheme="outline" class="text-purple-600 mr-2"></i>
                <span class="text-sm text-gray-600">{{event?.date | date:'mediumDate'}}</span>
              </div>
              <div class="flex items-center">
                <i nz-icon nzType="clock-circle" nzTheme="outline" class="text-pink-600 mr-2"></i>
                <span class="text-sm text-gray-600">{{event?.startTime | date: 'shortTime'}} - {{event?.endTime | date:'shortTime'}}</span>
              </div>
              <div class="flex items-center">
                <i nz-icon nzType="environment" nzTheme="outline" class="text-indigo-600 mr-2"></i>
                <span class="text-sm text-gray-600">{{event?.location}}</span>
              </div>
            </div>
          </div>
          
          <!-- Mobile View (sm and below) -->
          <div class="md:hidden">
            <div class="flex items-center justify-center mb-2">
              <img src="tsp-logo.png" alt="TSP Logo" class="h-6 mr-2">
              <h3 class="font-medium text-gray-800 text-sm">
                Event Registration |
                {{event?.name}}
              </h3>
            </div>
            <div class="grid grid-cols-3 gap-2 text-xs">
              <div class="flex items-center">
                <i nz-icon nzType="calendar" nzTheme="outline" class="text-purple-600 mr-1 text-xs"></i>
                <span class="text-gray-600 truncate">{{event?.date | date:'shortDate'}}</span>
              </div>
              <div class="flex items-center">
                <i nz-icon nzType="clock-circle" nzTheme="outline" class="text-pink-600 mr-1 text-xs"></i>
                <span class="text-gray-600 truncate">{{event?.startTime}}</span>
              </div>
              <div class="flex items-center">
                <i nz-icon nzType="environment" nzTheme="outline" class="text-indigo-600 mr-1 text-xs"></i>
                <span class="text-gray-600 truncate">{{event?.location}}</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  `
})
export class StickyInfoBarComponent implements AfterViewInit {
  @Input() event: EventDetails | undefined;
  
  constructor(private renderer: Renderer2) {}
  
  ngAfterViewInit(): void {
    // Initialize the sticky header in hidden state
    const stickyInfoBar = document.getElementById('stickyInfoBar');
    if (stickyInfoBar) {
      this.renderer.setStyle(stickyInfoBar, 'transform', 'translateY(-100%)');
    }
  }
  
  @HostListener('window:scroll', [])
  onWindowScroll() {
    const stickyInfoBar = document.getElementById('stickyInfoBar');
    if (!stickyInfoBar) return;
    
    // Show the sticky header after scrolling down 300px
    if (window.scrollY > 300) {
      this.renderer.setStyle(stickyInfoBar, 'transform', 'translateY(0)');
    } else {
      this.renderer.setStyle(stickyInfoBar, 'transform', 'translateY(-100%)');
    }
  }
} 