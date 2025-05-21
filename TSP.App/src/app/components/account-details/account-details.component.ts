import { Component, computed, inject, input } from '@angular/core';
import { Router, RouterLink, UrlTree } from '@angular/router';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { AuthService, UserType } from '../../common/services/auth.service';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { environment } from '../../../environments/environment';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { CookieService } from 'ngx-cookie-service';

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
  profileImageId: string = "";
  authService = inject(AuthService);
  router = inject(Router);
  userInfo = this.authService.currentUser;
  constructor(
    private cookieSerivce: CookieService
  ) {
    this.profileImageId = this.cookieSerivce.get("profile_image") || "";
  }
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
