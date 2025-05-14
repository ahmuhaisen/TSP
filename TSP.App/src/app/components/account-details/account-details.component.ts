import { NgIf } from '@angular/common';
import { Component, inject, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { IUserBasicDetails } from '../../common/types/user.types';
import { AuthService, User } from '../../common/services/auth.service';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { environment } from '../../../environments/environment';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';

@Component({
  selector: 'app-account-details',
  imports: [
    NzIconModule,
    NzDropDownModule,
    NzAvatarModule,
    RouterLink
  ],
  templateUrl: './account-details.component.html',
  styleUrl: './account-details.component.css'
})
export class AccountDetailsComponent {
  baseProfileImageUrl = environment.gitHubUsersPicturesURL;
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
