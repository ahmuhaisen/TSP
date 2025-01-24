import { Component } from '@angular/core';
import { DatePipe } from '@angular/common';
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

interface Member {
  id: string;
  name: string;
  position: string;
  memberSince: string;
  imageUrl: string;
}

interface ColumnItem {
  name: string;
  sortOrder: NzTableSortOrder | null;
  sortFn: NzTableSortFn<Member> | null;
  listOfFilter: NzTableFilterList;
  filterFn: NzTableFilterFn<Member> | null;
  filterMultiple: boolean;
  sortDirections: NzTableSortOrder[];
}


@Component({
  selector: 'app-members-table',
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
  templateUrl: './members-table.component.html',
  styleUrl: './members-table.component.css'
})
export class MembersTableComponent {
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


  private static dateSortFn(a: Member, b: Member): number {
    return new Date(a.memberSince).getTime() - new Date(b.memberSince).getTime();
  }

  private static filterFn<T>(key: keyof T) {
    return (list: string[], item: T) => {
      const value = item[key] as unknown as string;
      return typeof value === 'string' && list.some(filterValue => value.includes(filterValue));
    };
  }

  MembersTableColumns: ColumnItem[] = [
    {
      name: 'Name',
      sortOrder: null,
      sortFn: MembersTableComponent.localeSortFn<Member>('name'),
      sortDirections: ['ascend', 'descend', null],
      filterMultiple: false, // Single search term at a time
      listOfFilter: [], // No predefined values, input-based filtering
      filterFn: (search: string, item: Member) =>
        item.name.toLowerCase().includes(search.toLowerCase()), // Case-insensitive filter
    },
    {
      name: 'Section / Position',
      sortOrder: null,
      sortFn: MembersTableComponent.localeSortFn<Member>('position'),
      sortDirections: ['ascend', 'descend', null],
      filterMultiple: true,
      listOfFilter: [],
      filterFn: MembersTableComponent.filterFn<Member>('position')
    },
    {
      name: 'Member since',
      sortOrder: null,
      sortFn: MembersTableComponent.dateSortFn,
      sortDirections: ['ascend', 'descend', null],
      filterMultiple: true,
      listOfFilter: [],
      filterFn: (list: string[], item: Member) =>
        list.some(date => new Date(item.memberSince).toDateString() === new Date(date).toDateString())
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


  allMembers: Member[] = [
    {
      id: 'fe324b6d-8b5b-4a9f-9a5b-8b5bfe324b6d',
      name: 'Suhaib Ahmed',
      position: 'media',
      memberSince: '2024-01-01 11:30 AM',
      imageUrl: 'https://randomuser.me/api/portraits/lego/1.jpg'
    },
    {
      id: 'fe324b6d-8b5b-4a9f-9a5b-8b5bfe324b6d',
      name: 'Amer Khaleel',
      position: 'problem solving',
      memberSince: '2024-01-01 11:30 AM',
      imageUrl: 'https://randomuser.me/api/portraits/lego/5.jpg'
    },
    {
      id: 'fe324b6d-8b5b-4a9f-9a5b-8b5bfe324b6d',
      name: 'Noor Aldeen',
      position: 'media',
      memberSince: '2024-01-01 11:30 AM',
      imageUrl: 'https://randomuser.me/api/portraits/lego/4.jpg'
    },
    {
      id: 'fe324b6d-8b5b-4a9f-9a5b-8b5bfe324b6d',
      name: 'Noor Aldeen',
      position: 'podcast',
      memberSince: '2024-01-01 11:30 AM',
      imageUrl: 'https://randomuser.me/api/portraits/lego/3.jpg'
    },
    {
      id: 'fe324b6d-8b5b-4a9f-9a5b-8b5bfe324b6d',
      name: 'Noor Aldeen',
      position: 'magazine',    
      memberSince: '2024-01-01 11:30 AM',
      imageUrl: 'https://randomuser.me/api/portraits/lego/2.jpg'
    },
    {
      id: 'fe324b6d-8b5b-4a9f-9a5b-8b5bfe324b6d',
      name: 'Suhaib Ahmed',
      position: 'media',
      memberSince: '2024-01-01 11:30 AM',
      imageUrl: 'https://randomuser.me/api/portraits/lego/1.jpg'
    },
    {
      id: 'fe324b6d-8b5b-4a9f-9a5b-8b5bfe324b6d',
      name: 'Amer Khaleel',
      position: 'problem solving',
      memberSince: '2024-01-01 11:30 AM',
      imageUrl: 'https://randomuser.me/api/portraits/lego/5.jpg'
    },
    {
      id: 'fe324b6d-8b5b-4a9f-9a5b-8b5bfe324b6d',
      name: 'Noor Aldeen',
      position: 'media',
      memberSince: '2024-01-01 11:30 AM',
      imageUrl: 'https://randomuser.me/api/portraits/lego/4.jpg'
    },
    {
      id: 'fe324b6d-8b5b-4a9f-9a5b-8b5bfe324b6d',
      name: 'Noor Aldeen',
      position: 'podcast',
      memberSince: '2024-01-01 11:30 AM',
      imageUrl: 'https://randomuser.me/api/portraits/lego/3.jpg'
    },
    {
      id: 'fe324b6d-8b5b-4a9f-9a5b-8b5bfe324b6d',
      name: 'Noor Aldeen',
      position: 'magazine',    
      memberSince: '2024-01-01 11:30 AM',
      imageUrl: 'https://randomuser.me/api/portraits/lego/2.jpg'
    },
    {
      id: 'fe324b6d-8b5b-4a9f-9a5b-8b5bfe324b6d',
      name: 'Suhaib Ahmed',
      position: 'media',
      memberSince: '2024-01-01 11:30 AM',
      imageUrl: 'https://randomuser.me/api/portraits/lego/1.jpg'
    },
    {
      id: 'fe324b6d-8b5b-4a9f-9a5b-8b5bfe324b6d',
      name: 'Amer Khaleel',
      position: 'problem solving',
      memberSince: '2024-01-01 11:30 AM',
      imageUrl: 'https://randomuser.me/api/portraits/lego/5.jpg'
    },
    {
      id: 'fe324b6d-8b5b-4a9f-9a5b-8b5bfe324b6d',
      name: 'Noor Aldeen',
      position: 'media',
      memberSince: '2024-01-01 11:30 AM',
      imageUrl: 'https://randomuser.me/api/portraits/lego/4.jpg'
    },
    {
      id: 'fe324b6d-8b5b-4a9f-9a5b-8b5bfe324b6d',
      name: 'Noor Aldeen',
      position: 'podcast',
      memberSince: '2024-01-01 11:30 AM',
      imageUrl: 'https://randomuser.me/api/portraits/lego/3.jpg'
    },
    {
      id: 'fe324b6d-8b5b-4a9f-9a5b-8b5bfe324b6d',
      name: 'Noor Aldeen',
      position: 'magazine',    
      memberSince: '2024-01-01 11:30 AM',
      imageUrl: 'https://randomuser.me/api/portraits/lego/2.jpg'
    }
  ];

  listOfDisplayData = [...this.allMembers];

  reset(): void {
    this.searchValue = '';
    this.search();
  }

  search(): void {
    this.visible = false;
    this.listOfDisplayData = this.allMembers.filter((item: Member) => item.name.indexOf(this.searchValue) !== -1);
  }

  removeMember(id: string): void {
    this.nzMessageService.success(`Removed member with ID: ${id} from the society.`);
  }
}
