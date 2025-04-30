import { NgIf } from '@angular/common';
import { Component, HostListener } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzProgressModule } from 'ng-zorro-antd/progress';
import { INavbarItem } from '../../../../common/types/navbar.types';
import { NotificationsComponent } from '../../../../components/notifications/notifications.component';
import { AccountDetailsComponent } from '../../../../components/account-details/account-details.component';

@Component({
  selector: 'app-admin-navbar',
  imports: [
    RouterLink,
    RouterLinkActive,
    NgIf,
    NzIconModule,
    AccountDetailsComponent,
    NotificationsComponent,
    NzProgressModule
],
  templateUrl: './admin-navbar.component.html',
  styleUrl: './admin-navbar.component.css'
})
export class AdminNavbarComponent {
  isMenuOpen = false;
  isNotificationsDropdownOpen = false;
  isLargeScreen = window.innerWidth >= 1024;

  navbarItems: INavbarItem[] = [
    { name: 'Home', targetPagePath: 'home', iconName: 'home' },
    { name: 'Societies', targetPagePath: 'societies', iconName: 'product' },
    { name: 'Events', targetPagePath: 'events', iconName: 'project' },
    { name: 'Statistics', targetPagePath: 'statistics', iconName: 'pie-chart' },
  ];

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


