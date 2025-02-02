import { Component, HostListener } from '@angular/core';
import { INavbarItem } from '../../../../common/types/navbar.types';
import { NotificationsComponent } from "../../../../components/notifications/notifications.component";
import { AccountDetailsComponent } from "../../../../components/account-details/account-details.component";
import { RouterLink, RouterLinkActive } from '@angular/router';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NgIf } from '@angular/common';
import { IUserBasicDetails } from '../../../../common/types/user.types';

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

  user: IUserBasicDetails = {
    id: '12da-1876',
    name: 'Suhaib Saleh',
    email: 'suh0211111@ju.edu.jo',
    imageUrl: 'https://robohash.org/Suhaib@ju.edu.jo?bgset=bg2'
  }

  navbarItems: INavbarItem[] = [
    { name: 'Home', targetPagePath: 'home', iconName: 'home' },
    { name: 'My Societies', targetPagePath: 'societies', iconName: 'product' },
    { name: 'All Events', targetPagePath: 'events', iconName: 'project' },
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

