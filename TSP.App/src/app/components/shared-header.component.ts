import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NgClass, NgIf } from '@angular/common';

@Component({
  selector: 'app-shared-header',
  standalone: true,
  imports: [RouterLink, NgClass, NgIf],
  template: `
    <header class="fixed w-full bg-white bg-opacity-95 backdrop-blur-sm z-50 shadow-md" [class.scrolled]="isScrolled">
      <div class="container mx-auto px-6 py-4">
        <div class="flex justify-between items-center">
          <!-- Logo and Brand -->
          <a routerLink="/" class="flex items-center">
            <img src="tsp-logo.png" alt="TSP Logo" class="h-12 mr-3">
            <div class="flex flex-col">
              <span class="font-semibold text-xl text-gray-700">The Societies Portal</span>
              <span class="text-xs text-gray-500 font-light">Empowering University Communities</span>
            </div>
          </a>
          
          <!-- Desktop Navigation -->
          <div class="hidden md:flex items-center space-x-8">
            <nav class="flex space-x-8">
              <a *ngIf="showNavLinks" (click)="scrollToSection('portals')" 
                 [ngClass]="{'text-indigo-600 font-medium': isActiveSection('portals'), 'text-gray-600': !isActiveSection('portals')}"
                 class="hover:text-indigo-600 transition-colors duration-300 cursor-pointer relative nav-link">
                Areas
                <span *ngIf="isActiveSection('portals')" class="active-indicator"></span>
              </a>
              <a *ngIf="showNavLinks" (click)="scrollToSection('features')" 
                 [ngClass]="{'text-indigo-600 font-medium': isActiveSection('features'), 'text-gray-600': !isActiveSection('features')}"
                 class="hover:text-indigo-600 transition-colors duration-300 cursor-pointer relative nav-link">
                Features
                <span *ngIf="isActiveSection('features')" class="active-indicator"></span>
              </a>
            </nav>
            <div class="flex space-x-4">
              <a routerLink="/authentication/login" class="bg-indigo-600 hover:bg-indigo-700 hover:text-white text-white px-6 py-2 rounded-lg shadow-md transition-all duration-300 ease-in-out transform hover:-translate-y-0.5">
                Sign In
              </a>
              <a routerLink="/authentication/register" class="bg-gradient-to-r from-indigo-600 to-purple-600 hover:text-white hover:from-indigo-700 hover:to-purple-700 text-white px-6 py-2 rounded-lg shadow-md transition-all duration-300 ease-in-out transform hover:-translate-y-0.5">
                Sign Up
              </a>
            </div>
          </div>
          
          <!-- Mobile Menu Button -->
          <div class="md:hidden">
            <button (click)="toggleMobileMenu()" class="text-gray-500 hover:text-indigo-600 focus:outline-none">
              <svg *ngIf="!mobileMenuOpen" xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
              </svg>
              <svg *ngIf="mobileMenuOpen" xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
              </svg>
            </button>
          </div>
        </div>
        
        <!-- Mobile Menu -->
        <div *ngIf="mobileMenuOpen" class="md:hidden mt-4 pb-2 border-t border-gray-200">
          <nav *ngIf="showNavLinks" class="flex flex-col space-y-4 mt-4">
            <a (click)="scrollToSection('portals')" 
               [ngClass]="{'text-indigo-600 font-medium': isActiveSection('portals'), 'text-gray-600': !isActiveSection('portals')}"
               class="hover:text-indigo-600 transition-colors duration-300 cursor-pointer">Areas</a>
            <a (click)="scrollToSection('features')" 
               [ngClass]="{'text-indigo-600 font-medium': isActiveSection('features'), 'text-gray-600': !isActiveSection('features')}"
               class="hover:text-indigo-600 transition-colors duration-300 cursor-pointer">Features</a>
          </nav>
          <div class="flex flex-col space-y-2 mt-4">
            <a routerLink="/authentication/login" class="bg-gradient-to-r bg-indigo-600 hover:bg-indigo-700 hover:text-white text-white px-6 py-3 rounded-lg shadow-md text-center transition-all duration-300">
              Sign In
            </a>
            <a routerLink="/authentication/register" class="bg-gradient-to-r from-indigo-600 to-purple-600 hover:text-white text-white px-6 py-3 rounded-lg shadow-md text-center transition-all duration-300">
              Sign Up
            </a>
          </div>
        </div>
      </div>
    </header>
  `
})
export class SharedHeaderComponent {
  @Input() showNavLinks: boolean = true;
  @Input() isScrolled: boolean = false;
  mobileMenuOpen = false;
  activeSection = 'home';

  toggleMobileMenu() {
    this.mobileMenuOpen = !this.mobileMenuOpen;
  }

  scrollToSection(sectionId: string) {
    this.mobileMenuOpen = false;
    const element = document.getElementById(sectionId);
    if (element) {
      element.scrollIntoView({ behavior: 'smooth' });
      this.activeSection = sectionId;
    }
  }

  isActiveSection(sectionId: string): boolean {
    return this.activeSection === sectionId;
  }
} 