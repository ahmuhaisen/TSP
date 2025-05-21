import { Component } from '@angular/core';

import { GenProfileComponent } from '../../../../../components/gen-profile/gen.profile.component';

export interface SuggestedPerson {
  id: string;
  fullName: string;
  userType: string;
  department: string;
  profileImageId?: string;
  mutualSocieties: number;
}

@Component({
  selector: 'app-profile',
  imports: [
    GenProfileComponent
],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent {
  
}
