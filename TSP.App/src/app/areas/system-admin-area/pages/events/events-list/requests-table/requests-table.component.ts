import { CommonModule, DatePipe } from '@angular/common';
import { Component, inject, NgModule } from '@angular/core';
import { FormControl, FormGroup, FormsModule, NgModel, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzDropdownMenuComponent } from 'ng-zorro-antd/dropdown';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzTableFilterFn, NzTableFilterList, NzTableModule, NzTableSortFn, NzTableSortOrder } from 'ng-zorro-antd/table';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzAlertModule } from 'ng-zorro-antd/alert';
import { EventRequestDecision, EventSimpleRequest } from '../../../../api-interfaces/event.types';
import { OnInit } from '@angular/core';
import { EventsService } from '../../../../services/events.service';

interface Request {
  id: number;
  name: string;
  description?: string | undefined;
  society: string;
  date: string;
  status: string;
  location: string;
}

interface ColumnItem {
  name: string;
  sortOrder: NzTableSortOrder | null;
  sortFn: NzTableSortFn<Request> | null;
  listOfFilter: NzTableFilterList;
  filterFn: NzTableFilterFn<Request> | null;
  filterMultiple: boolean;
  sortDirections: NzTableSortOrder[];
}

@Component({
  selector: 'app-requests-table',
  imports: [
    FormsModule,
    ReactiveFormsModule,
    RouterLink,
    DatePipe,
    NzFormModule,
    NzInputModule,
    NzBreadCrumbModule,
    NzButtonModule,
    NzIconModule,
    NzDividerModule,
    NzTableModule,
    NzTagModule,
    NzInputModule,
    NzDropdownMenuComponent,
    NzPopconfirmModule,
    NzToolTipModule,
    NzModalModule,
    NzAlertModule
  ],
  templateUrl: './requests-table.component.html',
  styles: `
  .search-box {
        padding: 8px;
      }

      .search-box input {
        width: 188px;
        margin-bottom: 8px;
        display: block;
      }

      .search-box button {
        width: 90px;
      }

      .search-button {
        margin-right: 8px;
      }
  `
})
export class RequestsTableComponent implements OnInit {
  searchValue = '';
  visible = false;
  expandSet = new Set<string>();
  eventService = inject(EventsService);
  decision?: EventRequestDecision;

  constructor(private nzMessageService: NzMessageService) { }

  isRejectPopupVisible = false;

  onExpandChange(id: string, checked: boolean): void {
    if (checked) {
      this.expandSet.add(id);
    } else {
      this.expandSet.delete(id);
    }
  }

  private static localeSortFn<T>(key: keyof T) {
    return (a: T, b: T) => {
      const aValue = a[key] as unknown as string;
      const bValue = b[key] as unknown as string;
      return aValue.localeCompare(bValue);
    };
  }


  private static dateSortFn(a: Request, b: Request): number {
    return new Date(a.date).getTime() - new Date(b.date).getTime();
  }

  private static filterFn<T>(key: keyof T) {
    return (list: string[], item: T) => {
      const value = item[key] as unknown as string;
      return typeof value === 'string' && list.some(filterValue => value.includes(filterValue));
    };
  }

  requestsTableColumns: ColumnItem[] = [
    {
      name: 'Name',
      sortOrder: null,
      sortFn: RequestsTableComponent.localeSortFn<Request>('name'),
      sortDirections: ['ascend', 'descend', null],
      filterMultiple: false, // Single search term at a time
      listOfFilter: [], // No predefined values, input-based filtering
      filterFn: (search: string, item: Request) =>
        item.name.toLowerCase().includes(search.toLowerCase()), // Case-insensitive filter
    },
    {
      name: 'Organizer/Society',
      sortOrder: null,
      sortFn: RequestsTableComponent.localeSortFn<Request>('society'),
      sortDirections: ['ascend', 'descend', null],
      filterMultiple: true,
      listOfFilter: [],
      filterFn: RequestsTableComponent.filterFn<Request>('society')
    },
    {
      name: 'Date and Time',
      sortOrder: null,
      sortFn: RequestsTableComponent.dateSortFn,
      sortDirections: ['ascend', 'descend', null],
      filterMultiple: true,
      listOfFilter: [],
      filterFn: (list: string[], item: Request) =>
        list.some(date => new Date(item.date).toDateString() === new Date(date).toDateString())
    },
    {
      name: 'Location',
      sortOrder: null,
      sortFn: RequestsTableComponent.localeSortFn<Request>('location'),
      sortDirections: ['ascend', 'descend', null],
      filterMultiple: true,
      listOfFilter: [],
      filterFn: RequestsTableComponent.filterFn<Request>('location')
    },
    {
      name: 'Status',
      sortOrder: null,
      sortFn: RequestsTableComponent.localeSortFn<Request>('status'),
      sortDirections: ['ascend', 'descend', null],
      filterMultiple: true,
      listOfFilter: [
        { text: 'Pending', value: 'Pending' },
        { text: 'Accepted', value: 'Accepted' },
        { text: 'Rejected', value: 'Rejected' },
      ],
      filterFn: (list: string[], item: Request) => list.some(status => item.status === status)
    },
    {
      name: 'Actions',
      sortOrder: null,
      sortFn: null,
      sortDirections: [],
      filterMultiple: false,
      listOfFilter: [],
      filterFn: null
    },
  ];

  rejectForm = new FormGroup({
    reason: new FormControl('', [Validators.maxLength(200)])
  });

  eventsRequests: EventSimpleRequest[] = []
  selectedEventRequestForDecision!: EventSimpleRequest;
  listOfDisplayData = [...this.eventsRequests];
  ngOnInit(): void {
    this.eventService.getEventRequests().subscribe(data => this.eventsRequests = data);

  }
  reset(): void {
    this.searchValue = '';
    this.search();
  }

  search(): void {
    this.visible = false;
    this.listOfDisplayData = this.eventsRequests.filter((item: EventSimpleRequest) =>
      item.eventName.toLowerCase().includes(this.searchValue.toLowerCase())
    );
  }

  getStatusColor(status: string) {
    switch (status) {
      case 'Pending':
        return 'blue';
      case 'Accepted':
        return 'green';
      case 'Rejected':
        return 'red';
      default:
        return 'blue';
    }
  }

  cancelAcceptRequest(): void {

  }

  confirmAcceptRequest(selectedEventRequest: EventSimpleRequest): void {

    const desision: EventRequestDecision = {
      eventRequestId: selectedEventRequest.id,
      isAccepted: true,
      Remark: "Event accepted successfully"
    }

    this.eventService.postEventRequestDecision(desision).subscribe({
      next: (response) => {
        console.log('Decision response:', response);
        this.eventService.getEventRequests().subscribe(data => this.eventsRequests = data);
      },
      error: (err) => {
        console.error('Error submitting decision:', err);
      }
    });



    this.nzMessageService.success('Event accepted successfully.');
  }

  openRejectPopup(selectedEventRequest: EventSimpleRequest) {
    this.selectedEventRequestForDecision = selectedEventRequest;
    this.isRejectPopupVisible = true;
  }

  handleCancelReject() {
    this.isRejectPopupVisible = false;
  }

  handleOkReject() {
    const decision: EventRequestDecision = {
      eventRequestId: this.selectedEventRequestForDecision.id,
      isAccepted: false,
      Remark: this.rejectForm.get("reason")?.value || ""
    }
    this.eventService.postEventRequestDecision(decision).subscribe({
      next: (response) => {
        console.log('Decision response:', response);
        this.eventService.getEventRequests().subscribe(data => this.eventsRequests = data);
      },
      error: (err) => {
        console.error('Error submitting decision:', err);
      }
    });;
    this.nzMessageService.success("Event Request rejected")
    this.isRejectPopupVisible = false;
  }

  exportToCsv(): void {
    if (!this.eventsRequests || this.eventsRequests.length === 0) {
      this.nzMessageService.warning('No data to export');
      return;
    }

    // Define headers for CSV
    const headers = ['Event Name', 'Society', 'Date & Time', 'Location', 'Status', 'Description'];
    
    // Map data to CSV format
    const data = this.eventsRequests.map(request => [
      request.eventName,
      request.eventSociety.societyName,
      new Date(request.startDateTime).toLocaleString(),
      request.locationString,
      request.approvalStatus,
      request.eventDescription || 'No description'
    ]);
    
    // Create CSV content with headers and data
    const csvContent = [
      headers.join(','),
      ...data.map(row => row.map(cell => `"${String(cell).replace(/"/g, '""')}"`).join(','))
    ].join('\n');
    
    // Create a Blob with the CSV content
    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    
    // Create a download link and trigger download
    const link = document.createElement('a');
    const url = URL.createObjectURL(blob);
    link.setAttribute('href', url);
    link.setAttribute('download', `events-requests-${new Date().toISOString().slice(0, 10)}.csv`);
    link.style.visibility = 'hidden';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    
    this.nzMessageService.success('CSV exported successfully');
  }
}
