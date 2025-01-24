import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { TruncatePipe } from '../../../../../common/pipes/truncate.pipe';


@Component({
  selector: 'app-societies-list',
  imports: [
    RouterLink,
    TruncatePipe,
    NzBreadCrumbModule,
    NzButtonModule,
    NzIconModule,
    NzDividerModule
  ],
  templateUrl: './societies-list.component.html',
  styleUrl: './societies-list.component.css'
})
export class SocietiesListComponent {

  advisedSocieties = [
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

}
