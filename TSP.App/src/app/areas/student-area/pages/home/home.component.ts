import { DatePipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { TruncatePipe } from '../../../../common/pipes/truncate.pipe';
import { AuthService, User } from '../../../../common/services/auth.service';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { HomeService, HomeStatistics, StudentEvent } from '../../services/home.service';
import { NzPopoverModule } from 'ng-zorro-antd/popover';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    RouterLink,
    DatePipe,
    NzIconModule,
    NzButtonModule,
    NzDividerModule,
    NzAvatarModule,
    NzTagModule,
    NzPopoverModule
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  private authService = inject(AuthService);
  private homeService = inject(HomeService);

  userInfo = signal<User | null>(null);
  events: StudentEvent[] = [];
  statistics : HomeStatistics | null = null;

  ngOnInit() {
    this.userInfo.set(this.authService.currentUser());

    this.homeService.getRecentEvents().subscribe(res => this.events = res);
    this.homeService.getHomeStatistics().subscribe(res => this.statistics = res);

    console.log('Statistics:', this.statistics, 'Events:', this.events);
  }

  users = [
    {
      id: '3yz-4ab-5cd-6ef',
      name: 'Suhaib Saleh',
      department: 'Computer Science',
      imageUrl: 'https://robohash.org/suhaib@ju.edu.jo?bgset=bg2',
      joinDate: '2024-12-15',
      societies: ['Waves JU', 'ACM University of Jordan Student Chapter', 'IEEE CS JU']
    },
    {
      id: '1ab-2cd-3ef-4gh',
      name: 'Ahmad Muhaisen',
      department: 'Computer Science',
      imageUrl: 'https://robohash.org/ahmad@ju.edu.jo?bgset=bg2',
      joinDate: '2024-11-20',
      societies: ['ACM University of Jordan Student Chapter', 'IEEE CS JU']
    },
    {
      id: '5ij-6kl-7mn-8op',
      name: 'Rana Alsharif',
      department: 'Information Technology',
      imageUrl: 'https://robohash.org/Sara@ju.edu.jo?bgset=bg2',
      joinDate: '2024-11-15',
      societies: ['Waves JU', 'IEEE CIS JU']
    },
    {
      id: '9qr-0st-1uv-2wx',
      name: 'Mohammad Alzoubi',
      department: 'Computer Engineering',
      imageUrl: 'https://robohash.org/Mohammad@ju.edu.jo?bgset=bg1',
      joinDate: '2024-08-01',
      societies: ['Linux Society JU']
    }
  ]
}
