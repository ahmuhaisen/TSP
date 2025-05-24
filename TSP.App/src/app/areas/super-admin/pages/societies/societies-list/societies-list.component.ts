import { Component, inject, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

import { NzTableModule } from 'ng-zorro-antd/table';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalModule, NzModalService } from 'ng-zorro-antd/modal';

import { TruncatePipe } from '../../../../../common/pipes/truncate.pipe';

import { SocietiesService } from '../../../services/societies.service';
import { SocietyWithAdvisor } from '../../../../system-admin-area/api-interfaces/society.types';
import { environment } from '../../../../../../environments/environment';

@Component({
  selector: 'app-societies-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    NzTableModule,
    NzDividerModule,
    NzButtonModule,
    NzIconModule,
    NzAvatarModule,
    NzEmptyModule,
    NzSpinModule,
    NzModalModule,
    TruncatePipe
  ],
  templateUrl: './societies-list.component.html',
  styleUrl: './societies-list.component.css'
})
export class SocietiesListComponent implements OnInit {
  loading = true;
  societies: SocietyWithAdvisor[] = [];
  societiesPicturesBaseUrl = environment.gitHubSocietiesPicturesURL;
  profilePictureBaseUrl = environment.gitHubUsersPicturesURL;

  societiesService = inject(SocietiesService);
  messageService = inject(NzMessageService);
  modalService = inject(NzModalService);

  ngOnInit() {
    this.loadSocieties();
  }

  loadSocieties() {
    this.loading = true;
    this.societiesService.all().subscribe({
      next: (data) => {
        this.societies = data;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading societies', error);
        this.loading = false;
      }
    });
  }

  delete(id: string) {
    this.modalService.confirm({
      nzTitle: 'Are you sure you want to delete this society?',
      nzContent: 'This action cannot be undone.',
      nzOkText: 'Yes',
      nzOkType: 'primary',
      nzOkDanger: true,
      nzOnOk: () => this.confirmDelete(id),
      nzCancelText: 'No'
    });
  }

  confirmDelete(id: string) {
    this.societiesService.delete(id).subscribe({
      next: () => {
        this.messageService.success('Society deleted successfully');
        this.loadSocieties();
      },
      error: (error) => {
        console.error('Error deleting society', error);
        this.messageService.error('Failed to delete society');
      }
    });
  }

  exportToCsv() {
    if (this.societies.length === 0) {
      this.messageService.warning('No data to export');
      return;
    }

    // Define the columns and headers for CSV
    const headers = ['Name', 'Creation Date', 'Advisor', 'Number of Members'];

    // Create CSV content
    const csvRows = [];

    // Add headers
    csvRows.push(headers.join(','));

    // Add data rows
    for (const society of this.societies) {
      const values = [
        `"${society.name.replace(/"/g, '""')}"`,  // Escape quotes in CSV
        `"${new Date(society.creationDate).toISOString().split('T')[0]}"`,
        `"${society.advisor?.fullName || 'No advisor'}"`,
        society.numberOfMembers
      ];
      csvRows.push(values.join(','));
    }

    // Combine all rows into a single string with line breaks
    const csvString = csvRows.join('\n');

    // Create a download link
    const blob = new Blob([csvString], { type: 'text/csv;charset=utf-8;' });
    const url = URL.createObjectURL(blob);

    // Create and trigger download
    const link = document.createElement('a');
    link.setAttribute('href', url);
    link.setAttribute('download', `societies_${new Date().toISOString().split('T')[0]}.csv`);
    link.style.visibility = 'hidden';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);

    this.messageService.success('Societies exported successfully');
  }
} 