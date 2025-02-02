import { Component, input } from '@angular/core';

@Component({
  selector: 'app-edit-member-form',
  imports: [],
  templateUrl: './edit-member-form.component.html',
  styleUrl: './edit-member-form.component.css'
})
export class EditMemberFormComponent {
  member = input.required<Member>();
}
interface Member {
  id: string;
  name: string;
  position: string;
  memberSince: string;
  imageUrl: string;
}