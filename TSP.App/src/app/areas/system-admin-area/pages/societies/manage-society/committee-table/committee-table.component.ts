import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzTableModule } from 'ng-zorro-antd/table';

@Component({
  selector: 'app-committee-table',
  imports: [
    DatePipe,
    NzBreadCrumbModule,
    NzButtonModule,
    NzDividerModule,
    NzIconModule,
    NzTableModule,],
  templateUrl: './committee-table.component.html',
  styleUrl: './committee-table.component.css'
})
export class CommitteeTableComponent {
  committee = [
    {
      id: '23fs-sdf',
      name: 'Suhaib Saleh',
      position: 'President',
      imageUrl: 'https://randomuser.me/api/portraits/lego/1.jpg',
      startDate: '2024-01-01',
    },
    {
      id: '23fs-sdf',
      name: 'Amer Khaleel',
      position: 'Vice President',
      imageUrl: 'https://randomuser.me/api/portraits/lego/2.jpg',
      startDate: '2024-01-01',
    },
    {
      id: '23fs-sdf',
      name: 'Noor Aldeen',
      position: 'Treasure',
      imageUrl: 'https://randomuser.me/api/portraits/lego/3.jpg',
      startDate: '2024-01-01',
    }
  ];

}
