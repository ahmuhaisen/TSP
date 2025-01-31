import { NgIf } from '@angular/common';
import { Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { IUserBasicDetails } from '../../common/types/user.types';

@Component({
  selector: 'app-account-details',
  imports: [
    NgIf,
    NzIconModule,
    RouterLink
  ],
  templateUrl: './account-details.component.html',
  styleUrl: './account-details.component.css'
})
export class AccountDetailsComponent {
  isAccountDetailsDropdownOpen = false;

  user = input.required<IUserBasicDetails>();

  toggleAccountDetailsDropdown(): void {
    this.isAccountDetailsDropdownOpen = !this.isAccountDetailsDropdownOpen;
  }
}
