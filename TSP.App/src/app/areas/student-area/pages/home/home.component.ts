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

@Component({
  selector: 'app-home',
  imports: [
    RouterLink,
    DatePipe,
    NzIconModule,
    NzButtonModule,
    NzDividerModule,
    NzAvatarModule,
    NzTagModule
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

  userInfo = signal<User | null>(null);

  authService = inject(AuthService);

  ngOnInit() {
    this.userInfo.set(this.authService.currentUser());
  }

  events = [
    {
      id: '1ab-2cd-3ef-4gh',
      title: 'Junior to Solver 6.0',
      society: 'ACM University of Jordan Student Chapter',
      date: '2024-07-01 9:00 AM',
      location: 'KASIT Auditorium',
      imageUrl: 'https://robohash.org/event1?bgset=bg2',
      isMember: true,
      isFinished: true,
    },
    {
      id: '5ij-6kl-7mn-8op',
      title: 'Hackathon 2024',
      society: 'IEEE CS JU',
      date: '2024-07-15 10:00 AM',
      location: 'ProgressSoft Lab, KASIT',
      imageUrl: 'https://robohash.org/event2?bgset=bg1',
      isMember: true,
      isFinished: false,
    },
    {
      id: '9qr-0st-1uv-2wx',
      title: 'Tech Talk: Linux in 2024',
      society: 'Linux Society JU',
      date: '2024-08-01 3:30 PM',
      location: 'Hall 101, KASIT',
      imageUrl: 'https://robohash.org/event3?bgset=bg2',
      isMember: false,
      isFinished: false,
    },
    {
      id: '3yz-4ab-5cd-6ef',
      title: 'Catch the Flag competition',
      society: 'Hackerspace JU',
      date: '2024-08-15 12:00 PM',
      location: 'Lab 203, KASIT',
      imageUrl: 'https://robohash.org/event4?bgset=bg1',
      isMember: false,
      isFinished: false,
    },
  ];

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
