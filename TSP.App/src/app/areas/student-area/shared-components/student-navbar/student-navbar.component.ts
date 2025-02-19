import { Component, HostListener, inject } from '@angular/core';
import { INavbarItem } from '../../../../common/types/navbar.types';
import { NotificationsComponent } from "../../../../components/notifications/notifications.component";
import { AccountDetailsComponent } from "../../../../components/account-details/account-details.component";
import { RouterLink, RouterLinkActive } from '@angular/router';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NgIf } from '@angular/common';
import { IUserBasicDetails } from '../../../../common/types/user.types';
import { AuthService, User } from '../../../../common/services/auth.service';

@Component({
  selector: 'app-student-navbar',
  imports: [
    NgIf,
    RouterLink,
    RouterLinkActive,
    NzIconModule,
    NotificationsComponent,
    AccountDetailsComponent
  ],
  templateUrl: './student-navbar.component.html',
  styleUrl: './student-navbar.component.css'
})
export class StudentNavbarComponent {
  isMenuOpen = false;
  isNotificationsDropdownOpen = false;
  isLargeScreen = window.innerWidth >= 1024;

  authService = inject(AuthService);

  user: User | null = null;

  navbarItems: INavbarItem[] = [
    { name: 'Home', targetPagePath: 'home', iconName: 'home' },
    { name: 'My Societies', targetPagePath: 'societies', iconName: 'product' },
    { name: 'Events', targetPagePath: 'events', iconName: 'project' },
  ];

  ngOnInit(){
    this.user = this.authService.currentUser();
  }

  toggleMenu() {
    this.isMenuOpen = !this.isMenuOpen;
  }

  toggleNotificationsDropdown(): void {
    this.isNotificationsDropdownOpen = !this.isNotificationsDropdownOpen;
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: Event) {
    this.isLargeScreen = window.innerWidth >= 1024;
    if (this.isLargeScreen) {
      this.isMenuOpen = false;
    }
  }
}

