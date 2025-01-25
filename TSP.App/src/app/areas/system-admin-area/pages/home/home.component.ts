import { Component, TemplateRef, ViewChild } from '@angular/core';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { NzPopoverModule } from 'ng-zorro-antd/popover';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzDrawerModule, NzDrawerPlacement } from 'ng-zorro-antd/drawer';
import { NzSkeletonModule } from 'ng-zorro-antd/skeleton';
import { DatePipe, NgClass, NgFor, NgIf } from '@angular/common';
import { RouterLink } from '@angular/router';
import { TruncatePipe } from '../../../../common/pipes/truncate.pipe';
import { FormsModule } from '@angular/forms';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzTagModule } from 'ng-zorro-antd/tag';

@Component({
  selector: 'app-home',
  imports: [
    NgIf,
    NgFor,
    DatePipe,
    TruncatePipe,
    RouterLink,
    NzEmptyModule,
    NzSpinModule,
    NzBreadCrumbModule,
    NzIconModule,
    NzButtonModule,
    NzDividerModule,
    NzPopoverModule,
    NzSkeletonModule,
    NzDrawerModule, NzTagModule,
    FormsModule,
    NzButtonModule,
    NzInputModule,
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  isLoading = false;
  isSearchLoading = false;

  ngOnInit() {
    this.isLoading = true;

    setTimeout(() => {
      this.isLoading = false;
    }, 500);
  }

  events = [
    {
      id: '1ab-2cd-3ef-4gh',
      title: 'Junior to Solver 6.0',
      society: 'ACM University of Jordan Student Chapter',
      date: '2024-07-01 9:00 AM',
      location: 'KASIT Auditorium',
      imageUrl: 'https://robohash.org/event1?bgset=bg2',
      isAdvised: true,
      isFinished: true,
    },
    {
      id: '5ij-6kl-7mn-8op',
      title: 'Hackathon 2024',
      society: 'IEEE CS JU',
      date: '2024-07-15 10:00 AM',
      location: 'ProgressSoft Lab, KASIT',
      imageUrl: 'https://robohash.org/event2?bgset=bg1',
      isAdvised: true,
      isFinished: false,
    },
    {
      id: '9qr-0st-1uv-2wx',
      title: 'Tech Talk: Linux in 2024',
      society: 'Linux Society JU',
      date: '2024-08-01 3:30 PM',
      location: 'Hall 101, KASIT',
      imageUrl: 'https://robohash.org/event3?bgset=bg2',
      isAdvised: false,
      isFinished: false,
    },
    {
      id: '3yz-4ab-5cd-6ef',
      title: 'Catch the Flag competition',
      society: 'Hackerspace JU',
      date: '2024-08-15 12:00 PM',
      location: 'Lab 203, KASIT',
      imageUrl: 'https://robohash.org/event4?bgset=bg1',
      isAdvised: false,
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

  announcements = [
    {
      id: '1ab-2cd-3ef-4gh',
      title: 'Postponement of the JUCPC competition',
      content: 'Due to the current situation, the JUCPC competition will be postponed to a later date.',
      sender: 'ACM University of Jordan Student Chapter',
      date: '2024-07-01 9:00 AM',
      type: 'Postponement'
    },
    {
      id: '5ij-6kl-7mn-8op',
      title: 'Hackathon 2024',
      content: 'The Hackathon 2024 will be held on 15th of July, 2024 at ProgressSoft Lab, KASIT.',
      sender: 'HackerSpace JU',
      date: '2024-07-15 10:00 AM',
      type: 'Announcement'
    },
    {
      id: '9qr-0st-1uv-2wx',
      title: 'Tech Talk: Linux in 2024',
      content: 'The Tech Talk: Linux in 2024 will be held on 1st of August, 2024 at Hall 101, KASIT.',
      sender: 'Linux Society JU',
      date: '2024-08-01 3:30 PM',
      type: 'Announcement'
    },
    {
      id: '3yz-4ab-5cd-6ef',
      title: 'Catch the Flag competition',
      content: 'The Catch the Flag competition will be held on 15th of August, 2024 at Lab 203, KASIT.',
      sender: 'Hackerspace JU',
      date: '2024-08-15 12:00 PM',
      type: 'Cancellation'
    }
  ]
  // Drawer
  visible = false;
  placement: NzDrawerPlacement = 'bottom';
  open(): void {
    this.visible = true;
  }

  close(): void {
    this.visible = false;
  }

  doSearch() {
    this.isSearchLoading = true;

    setTimeout(() => {
      this.isSearchLoading = false;
    }, 1000);
  }


}
