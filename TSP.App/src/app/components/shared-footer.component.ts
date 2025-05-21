import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-shared-footer',
  standalone: true,
  imports: [RouterLink],
  template: `
    <footer class="bg-gray-900 text-white py-12">
      <div class="container mx-auto px-6">
        <div class="grid grid-cols-1 md:grid-cols-4 gap-8">
          <div class="col-span-1 md:col-span-2">
            <a routerLink="/" class="flex items-center mb-4">
              <img src="tsp_logo_white.png" alt="TSP Logo" class="h-8 mr-3">
              <span class="font-semibold text-xl">The Societies Portal</span>
            </a>
            <p class="text-gray-400 mb-6">Empowering university societies to thrive and grow.</p>
          </div>
          
          <div>
            <h3 class="font-semibold text-lg mb-4 text-white">Quick Links</h3>
            <ul class="space-y-2">
              <li><a routerLink="/" class="text-gray-400 hover:text-white transition-colors duration-300">Home</a></li>
              <li><a routerLink="/" [fragment]="'features'" class="text-gray-400 hover:text-white transition-colors duration-300">Features</a></li>
              <li><a routerLink="/" [fragment]="'portals'" class="text-gray-400 hover:text-white transition-colors duration-300">Areas</a></li>
            </ul>
          </div>
          
          <div>
            <h3 class="font-semibold text-lg mb-4 text-white">Portal Access</h3>
            <ul class="space-y-2">
              <li><a routerLink="/student-area" class="text-gray-400 hover:text-white transition-colors duration-300">Student Area</a></li>
              <li><a routerLink="/admin-area" class="text-gray-400 hover:text-white transition-colors duration-300">Faculty Area</a></li>
              <li><a routerLink="/super-admin" class="text-gray-400 hover:text-white transition-colors duration-300">Admin Area</a></li>
            </ul>
          </div>
        </div>
        
        <div class="border-t border-gray-800 mt-10 pt-8 text-center text-gray-500">
          <div class="flex justify-center space-x-4 mb-4">
            <a routerLink="/privacy-policy" class="text-gray-400 hover:text-white transition-colors duration-300">Privacy Policy</a>
            <span class="text-gray-600">|</span>
            <a routerLink="/terms-of-use" class="text-gray-400 hover:text-white transition-colors duration-300">Terms of Use</a>
          </div>
          <p>&copy; {{currentYear}} The Societies Portal. All rights reserved.</p>
        </div>
      </div>
    </footer>
  `
})
export class SharedFooterComponent {
  currentYear = new Date().getFullYear();
} 