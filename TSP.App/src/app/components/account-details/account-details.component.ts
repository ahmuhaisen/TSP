import { NgIf } from '@angular/common';
import { Component, inject, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { IUserBasicDetails } from '../../common/types/user.types';
import { AuthService, User } from '../../common/services/auth.service';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';

@Component({
  selector: 'app-account-details',
  imports: [
    NgIf,
    NzIconModule,
    NzAvatarModule,
    RouterLink
  ],
  templateUrl: './account-details.component.html',
  styleUrl: './account-details.component.css'
})
export class AccountDetailsComponent {
  isAccountDetailsDropdownOpen = false;

  authService = inject(AuthService);
  userInfo = this.authService.currentUser;

  toggleAccountDetailsDropdown(): void {
    this.isAccountDetailsDropdownOpen = !this.isAccountDetailsDropdownOpen;
  }

  logout() {
    this.authService.logout();
  }
}
