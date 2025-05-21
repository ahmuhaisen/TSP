import { Component, inject, signal } from '@angular/core';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { AccountsService, PendingAccountRequest } from '../../services/accounts.service';
import { LoaderService } from '../../../../common/services/loader.service';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzPaginationComponent } from 'ng-zorro-antd/pagination';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NzEmptyComponent } from 'ng-zorro-antd/empty';
import { NzMessageService } from 'ng-zorro-antd/message';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    NzButtonModule,
    NzIconModule,
    NzEmptyComponent,
    NzPaginationComponent,
  ],
  templateUrl: './users.component.html',
  styleUrl: './users.component.css'
})
export class UsersComponent {
  accountService = inject(AccountsService);
  loadingService = inject(LoaderService);
  nzMessageService = inject(NzMessageService);

  selectedUserTypeToShowRequests: 'faculty' | 'student' = 'faculty';

  facultyRequests = signal<PendingAccountRequest[]>([]);
  studentRequests = signal<PendingAccountRequest[]>([]);

  Math = Math; // For using Math functions in the template

  // Pagination
  currentFacultyPage = 1;
  currentStudentPage = 1;
  pageSize = 10;
  
  // Search
  searchTerm = '';

  // Computed properties for displayed data
  get filteredFacultyRequests() {
    const term = this.searchTerm.toLowerCase();
    const filtered = this.facultyRequests().filter(request => 
      request.fullName.toLowerCase().includes(term)
    );
    const startIndex = (this.currentFacultyPage - 1) * this.pageSize;
    return filtered.slice(startIndex, startIndex + this.pageSize);
  }

  get filteredStudentRequests() {
    const term = this.searchTerm.toLowerCase();
    const filtered = this.studentRequests().filter(request => 
      request.fullName.toLowerCase().includes(term)
    );
    const startIndex = (this.currentStudentPage - 1) * this.pageSize;
    return filtered.slice(startIndex, startIndex + this.pageSize);
  }

  // Total counts for pagination
  get totalFilteredFacultyRequests() {
    const term = this.searchTerm.toLowerCase();
    return this.facultyRequests().filter(request => 
      request.fullName.toLowerCase().includes(term)
    ).length;
  }

  get totalFilteredStudentRequests() {
    const term = this.searchTerm.toLowerCase();
    return this.studentRequests().filter(request => 
      request.fullName.toLowerCase().includes(term)
    ).length;
  }

  onFacultyPageChange(page: number) {
    this.currentFacultyPage = page;
  }
  
  onStudentPageChange(page: number) {
    this.currentStudentPage = page;
  }

  onSearchChange() {
    // Reset to first page when search term changes
    this.currentFacultyPage = 1;
    this.currentStudentPage = 1;
  }

  ngOnInit() {
    this.fetchPendingRequests();
  }

  selectUserTypeToShowRequests(userType: 'faculty' | 'student') {
    this.selectedUserTypeToShowRequests = userType;
    // Reset to first page when switching tabs
    this.currentFacultyPage = 1;
    this.currentStudentPage = 1;
  }

  fetchPendingRequests() {
    this.loadingService.show();
    this.accountService.getAllPendingRequests().subscribe({
      next: (res) => {
        this.facultyRequests.set(res.filter((request) => request.userType === 'Faculty Member'));
        this.studentRequests.set(res.filter((request) => request.userType === 'Student'));
        this.loadingService.hide();
      },
      error: (err) => {
        console.error('Error fetching pending requests:', err);
        this.loadingService.hide();
      }
    });
  }

  acceptRequest(id: string, userType: string) {
    this.loadingService.show();
    this.accountService.acceptRequest(id, userType).subscribe({
      next: () => {
        this.nzMessageService.success(`${userType} request accepted successfully!`);
        this.fetchPendingRequests();
        this.loadingService.hide();
      },
      error: (err) => {
        this.loadingService.hide();
      }
    });
  }

  rejectRequest(id: string, userType: string) {
    this.loadingService.show();
    this.accountService.rejectRequest(id, userType).subscribe({
      next: () => {
        this.nzMessageService.success(`${userType} request rejected successfully!`);
        this.fetchPendingRequests();
        this.loadingService.hide();
      },
      error: (err) => {
        console.error('Error rejecting request:', err);
        this.loadingService.hide();
      }
    });
  }
}