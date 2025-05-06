import { NgIf } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { AuthService, User } from '../../../../../common/services/auth.service';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { environment } from '../../../../../../environments/environment';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';

@Component({
  selector: 'app-account-details',
  imports: [
    NzDropDownModule,
    NzIconModule,
    NzAvatarModule,
    RouterLink
  ],
  templateUrl: './account-details.component.html',
  styleUrl: './account-details.component.css'
})
export class AccountDetailsComponent {
  baseProfileImageUrl = environment.gitHubUsersPicturesURL;
  isAccountDetailsDropdownOpen = false;
  userInfo = signal<User | null>(null);

  authService = inject(AuthService);

  ngOnInit() {
    this.userInfo.set(this.authService.currentUser());
  }

  toggleAccountDetailsDropdown(): void {
    this.isAccountDetailsDropdownOpen = !this.isAccountDetailsDropdownOpen;
  }

  logout(){
    this.authService.logout();
  }
}
