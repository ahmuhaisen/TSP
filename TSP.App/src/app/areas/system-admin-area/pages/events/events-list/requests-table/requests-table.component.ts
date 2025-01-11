import { CommonModule, DatePipe } from '@angular/common';
import { Component, NgModule } from '@angular/core';
import { FormsModule, NgModel } from '@angular/forms';
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
    RouterLink,
    DatePipe,
    NzBreadCrumbModule,
    NzButtonModule,
    NzIconModule,
    NzDividerModule,
    NzTableModule,
    NzTagModule,
    NzInputModule,
    NzDropdownMenuComponent,
    NzPopconfirmModule,
    NzToolTipModule
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
export class RequestsTableComponent {
  searchValue = '';
  visible = false;
  expandSet = new Set<number>();
  constructor(private nzMessageService: NzMessageService) {}

  
  onExpandChange(id: number, checked: boolean): void {
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


  eventsRequests: Request[] = [
    {
      id: 1,
      name: 'Event 1',
      society: 'Society 1',
      date: '2024-01-01 11:30 AM',
      location: 'Location 1',
      status: 'Pending',
      description: 'Event Description 1',
    },
    {
      id: 2,
      name: 'Event 2',
      society: 'Society 2',
      date: '2024-01-02 12:00 PM',
      location: 'Location 2',
      status: 'Accepted',
      description: 'Event Description 2',
    },
    {
      id: 3,
      name: 'Event 3',
      society: 'Society 3',
      date: '2024-01-03 1:30 PM',
      location: 'Location 3',
      status: 'Rejected',
    },
    {
      id: 4,
      name: 'Event 4',
      society: 'Society 1',
      date: '2024-01-01 11:30 AM',
      location: 'Location 1',
      status: 'Pending',
      description: 'Event Description 1',
    },
    {
      id: 5,
      name: 'Event 5',
      society: 'Society 2',
      date: '2024-01-02 12:00 PM',
      location: 'Location 2',
      status: 'Accepted',
      description: 'Event Description 2',
    },
    {
      id: 6,
      name: 'Event 6',
      society: 'Society 3',
      date: '2024-01-03 1:30 PM',
      location: 'Location 3',
      status: 'Accepted',
    },
  ];

  listOfDisplayData = [...this.eventsRequests];

  reset(): void {
    this.searchValue = '';
    this.search();
  }

  search(): void {
    this.visible = false;
    this.listOfDisplayData = this.eventsRequests.filter((item: Request) => item.name.indexOf(this.searchValue) !== -1);
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
    //this.nzMessageService.info('click cancel');
  }

  confirmAcceptRequest(): void {
    this.nzMessageService.success('Event accepted successfully.');
  }
}
