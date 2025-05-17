import { NgIf } from '@angular/common';
import { Component, computed, inject, input } from '@angular/core';
import { Router, RouterLink, UrlTree } from '@angular/router';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { IUserBasicDetails } from '../../common/types/user.types';
import { AuthService, User, UserType } from '../../common/services/auth.service';
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
  userType = input<UserType>('FacultyMember');
  baseProfileImageUrl = environment.gitHubUsersPicturesURL;
  isAccountDetailsDropdownOpen = false;

  authService = inject(AuthService);
  router = inject(Router);
  userInfo = this.authService.currentUser;

  profileLink = computed<UrlTree>(() => {
    if (this.userType() === 'FacultyMember') {
      return this.router.createUrlTree([
        'admin-area',
        'users',
        this.userInfo()?.id
      ], { queryParams: { userType: 'FacultyMember' } });
    }
    return this.router.createUrlTree([
      'student-area',
      'users',
      this.userInfo()?.id
    ]);
  });

  toggleAccountDetailsDropdown(): void {
    this.isAccountDetailsDropdownOpen = !this.isAccountDetailsDropdownOpen;
  }

  logout() {
    this.authService.logout();
  }
}
