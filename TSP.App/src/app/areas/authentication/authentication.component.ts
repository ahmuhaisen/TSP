import { Component, inject } from '@angular/core';
import { ActivatedRoute, Router, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { UserType } from '../../common/services/auth.service';

@Component({
  selector: 'app-authentication',
  imports: [
    RouterOutlet,
    CommonModule
  ],
  templateUrl: './authentication.component.html',
  styleUrl: './authentication.component.css'
})
export class AuthenticationComponent {
  year = new Date().getFullYear();
  userType: UserType = 'FacultyMember';

  onOutletActivated(component: any) {
    // Listen for userType changes from child components
    if (component.selectedUserType !== undefined) {
      this.userType = component.selectedUserType;
      
      // Subscribe to future changes
      if (component.handleUserTypeChange && component.handleUserTypeChange.subscribe) {
        component.handleUserTypeChange.subscribe((type: UserType) => {
          this.userType = type;
        });
      }
    }
  }
}
