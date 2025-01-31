import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { TruncatePipe } from '../../../../../common/pipes/truncate.pipe';
import { NzButtonComponent, NzButtonModule } from 'ng-zorro-antd/button';
import { NgClass } from '@angular/common';
import { NzEmptyModule } from 'ng-zorro-antd/empty';

@Component({
  selector: 'app-societies-list',
  imports: [
    NgClass,
    RouterLink,
    TruncatePipe,
    NzIconModule,
    NzDividerModule,
    NzButtonModule,
    NzEmptyModule
  ],
  templateUrl: './societies-list.component.html',
  styleUrl: './societies-list.component.css'
})
export class SocietiesListComponent {
  belongingSocieties = [
    {
      id: '1ab-2cd-3ef-4gh',
      name: 'ACM University of Jordan Student Chapter',
      description: 'A Chapter of the Association for Computing Machinery, interested in computer science and programming.',
      logoUrl: 'https://robohash.org/society1',
      isManager: true,
      position: 'President'
    },
    {
      id: '9qr-0st-1uv-2wx',
      name: 'Linux Society JU',
      description: 'Linux Society of Jordan',
      logoUrl: 'https://robohash.org/society2',
      isManager: false,
      position: 'Media'
    },
    {
      id: '321-1sad-1uv-23sd',
      name: 'Waves JU',
      description: 'Waves Society of Jordan, a student chapter interested in robotics.',
      logoUrl: 'https://robohash.org/society3',
      isManager: false,
      position: 'Technical Team'
    },
  ];

  
  otherSocieties = [
    {
      id: '5ij-6kl-7mn-8op',
      name: 'IEEE CS JU',
      description: 'The IEEE Computer Society of Jordan',
      logoUrl: 'https://robohash.org/society1',
      themeColor: '#1f1f1f',
    },
    {
      id: '1ab-2cd-3ef-4gh',
      name: 'ACM University of Jordan Student Chapter',
      description: 'A Chapter of the Association for Computing Machinery, interested in computer science and programming.',
      logoUrl: 'https://robohash.org/society2',
      themeColor: '#1f1f1f',
    },
    {
      id: '9qr-0st-1uv-2wx',
      name: 'Linux Society JU',
      description: 'Linux Society of Jordan',
      logoUrl: 'https://robohash.org/society3',
      themeColor: '#1f1f1f',
    },
    {
      id: '2345-fghi-0123-4mn',
      name: 'Waves JU',
      description: 'The Waves Society of Jordan',
      logoUrl: 'https://robohash.org/society4',
      themeColor: '#1f1f1f',
    }
  ];

  constructor() { 
    //this.belongingSocieties = [];
  }
}
