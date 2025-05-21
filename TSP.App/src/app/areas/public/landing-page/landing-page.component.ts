import { Component, HostListener, OnInit, OnDestroy } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NgIf, NgFor, NgClass } from '@angular/common';
import { interval, Subscription } from 'rxjs';
import { SharedFooterComponent } from '../../../components/shared-footer.component';
import { SharedHeaderComponent } from '../../../components/shared-header.component';

@Component({
  selector: 'app-landing-page',
  standalone: true,
  imports: [
    RouterLink,
    NgIf,
    NgFor,
    NgClass,
    SharedFooterComponent,
    SharedHeaderComponent
  ],
  templateUrl: './landing-page.component.html',
  styleUrl: './landing-page.component.css'
})
export class LandingPageComponent implements OnInit, OnDestroy {
  features = [
    { 
      title: 'Event Management', 
      description: 'Create, manage and track society events with ease',
      icon: 'calendar' 
    },
    { 
      title: 'Member Registration', 
      description: 'Streamlined membership registration and tracking',
      icon: 'users' 
    },
    { 
      title: 'Resource Allocation', 
      description: 'Efficiently manage and allocate resources',
      icon: 'cube' 
    },
    { 
      title: 'Analytics', 
      description: 'Get insights with powerful reporting tools',
      icon: 'chart' 
    }
  ];
  
  // Main hero screenshots
  heroScreenshots = [
    'admin-area-ss.png',
    'student-area-ss.png',
    'super-admin-area-ss.png'
  ];
  currentHeroIndex = 0;
  animating = false;
  
  // Navigation
  mobileMenuOpen = false;
  isScrolled = false;
  activeSection = 'home';
  sections = ['home', 'features', 'portals'];
  
  private slideshowSubscription?: Subscription;

  ngOnInit() {
    // Check scroll position on page load
    this.checkScroll();
    
    // Start the slideshow rotation
    this.slideshowSubscription = interval(8000).subscribe(() => {
      this.rotateScreenshots();
    });
    
    // Initial detection of active section
    this.detectActiveSection();
  }
  
  ngOnDestroy() {
    // Clean up subscription
    if (this.slideshowSubscription) {
      this.slideshowSubscription.unsubscribe();
    }
  }
  
  // Mobile menu methods
  toggleMobileMenu() {
    this.mobileMenuOpen = !this.mobileMenuOpen;
  }
  
  closeMobileMenu() {
    this.mobileMenuOpen = false;
  }
  
  // Scroll to section
  scrollToSection(sectionId: string) {
    this.closeMobileMenu();
    const element = document.getElementById(sectionId);
    if (element) {
      element.scrollIntoView({ behavior: 'smooth' });
      this.activeSection = sectionId;
    }
  }
  
  // Check if a section is the active one
  isActiveSection(sectionId: string): boolean {
    return this.activeSection === sectionId;
  }
  
  // Detect which section is currently in view
  detectActiveSection() {
    const scrollPosition = window.scrollY + 100;
    
    for (const section of this.sections) {
      const element = document.getElementById(section);
      if (element) {
        const offsetTop = element.offsetTop;
        const offsetHeight = element.offsetHeight;
        
        if (scrollPosition >= offsetTop && scrollPosition < offsetTop + offsetHeight) {
          this.activeSection = section;
          break;
        }
      }
    }
  }
  
  rotateScreenshots() {
    if (this.animating) return;
    
    this.animating = true;
    // Rotate hero screenshot
    this.currentHeroIndex = (this.currentHeroIndex + 1) % this.heroScreenshots.length;
    
    // Reset animating flag after animation completes
    setTimeout(() => {
      this.animating = false;
    }, 1000);
  }
  
  setActiveImage(index: number) {
    if (this.animating || this.currentHeroIndex === index) return;
    
    this.animating = true;
    this.currentHeroIndex = index;
    
    // Reset animating flag after animation completes
    setTimeout(() => {
      this.animating = false;
    }, 1000);
  }

  @HostListener('window:scroll')
  checkScroll() {
    this.isScrolled = window.scrollY > 50;
    this.detectActiveSection();
  }
  
  @HostListener('window:resize')
  checkScreenSize() {
    // Close mobile menu if screen is resized to desktop width
    if (window.innerWidth >= 768 && this.mobileMenuOpen) {
      this.mobileMenuOpen = false;
    }
  }
}
