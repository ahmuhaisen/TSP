import { Component, OnInit, inject, signal } from '@angular/core';
import { RouterLink, Router, NavigationEnd } from '@angular/router';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { ProfilesService } from '../../../../common/services/profiles.service';
import { AuthService } from '../../../../common/services/auth.service';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-profile-image-alert',
  standalone: true,
  imports: [RouterLink, NzIconModule],
  template: `
    @if(shouldShowAlert()) {
      <div class="bg-blue-50 border-blue-200 border-t border-b px-4 py-2 w-full">
        <div class="max-w-screen-xl px-2 sm:px-4 mx-auto flex flex-col sm:flex-row justify-between items-center gap-2">
          <div class="flex items-center">
            <nz-icon nzType="info-circle" nzTheme="fill" class="text-blue-500 mr-2"></nz-icon>
            <span class="text-xs text-blue-800">You haven't set a profile picture yet.</span>
          </div>
          <div class="flex items-center gap-3">
            <a [routerLink]="['/admin-area/users', userId()]" [queryParams]="{userType: 'Faculty'}" class="text-xs font-medium text-blue-600 hover:text-blue-800 whitespace-nowrap">
              Update your profile
              <nz-icon nzType="arrow-right" nzTheme="outline" class="ml-1"></nz-icon>
            </a>
            <button (click)="dismissAlert()" class="text-blue-500 hover:text-blue-700" title="Dismiss">
              <nz-icon nzType="close" nzTheme="outline"></nz-icon>
            </button>
          </div>
        </div>
      </div>
    }
  `,
  styles: []
})
export class ProfileImageAlertComponent implements OnInit {
  private profilesService = inject(ProfilesService);
  private authService = inject(AuthService);
  private router = inject(Router);
  
  shouldShowAlert = signal(false);
  missingProfileImage = signal(false);
  alertDismissed = signal(false);
  
  private get localStorageKey(): string {
    const currentUser = this.authService.currentUser();
    return currentUser ? `profile-alert-dismissed-${currentUser.id}` : '';
  }
  
  ngOnInit() {
    this.checkDismissedState();
    this.checkProfileImage();
    this.setupRouteListener();
  }
  
  userId() {
    const currentUser = this.authService.currentUser();
    return currentUser?.id || '';
  }
  
  dismissAlert() {
    this.alertDismissed.set(true);
    this.updateVisibility();
    
    // Save to localStorage
    if (this.localStorageKey) {
      localStorage.setItem(this.localStorageKey, 'true');
    }
  }
  
  private checkDismissedState() {
    if (this.localStorageKey) {
      const isDismissed = localStorage.getItem(this.localStorageKey) === 'true';
      this.alertDismissed.set(isDismissed);
    }
  }
  
  private setupRouteListener() {
    // Initial check
    this.updateVisibilityBasedOnRoute(this.router.url);
    
    // Listen for route changes
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event) => {
        if (event instanceof NavigationEnd) {
          this.updateVisibilityBasedOnRoute(event.url);
        }
      });
  }
  
  private updateVisibilityBasedOnRoute(url: string) {
    // Check if on home page
    const isHomePage = url === '/admin-area' || url === '/admin-area/home' || url.startsWith('/admin-area/home');
    
    // Check if on profile page
    const isProfilePage = url.includes('/admin-area/users/') && url.includes('userType=Faculty');
    
    // Update visibility
    this.updateVisibility(isHomePage || isProfilePage);
  }
  
  private updateVisibility(onRelevantPage = true) {
    this.shouldShowAlert.set(
      onRelevantPage && 
      this.missingProfileImage() && 
      !this.alertDismissed()
    );
  }
  
  private checkProfileImage() {
    console.log('Checking profile image...');
    this.profilesService.hasProfileImage().subscribe({
      next: (hasImage) => {
        console.log('Has profile image:', hasImage);
        this.missingProfileImage.set(!hasImage);
        this.updateVisibilityBasedOnRoute(this.router.url);
      },
      error: (err) => {
        console.error('Error checking profile image:', err);
        this.missingProfileImage.set(false);
        this.shouldShowAlert.set(false);
      }
    });
  }
} 