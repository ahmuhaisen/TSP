import { Component, inject, input } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';

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
import { EditMemberFormComponent } from "./edit-member-form/edit-member-form.component";
import { Member, SocietyMember } from '../../../areas/system-admin-area/api-interfaces/society.types';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';



interface ColumnItem {
  name: string;
  sortOrder: NzTableSortOrder | null;
  sortFn: NzTableSortFn<SocietyMember> | null;
  listOfFilter: NzTableFilterList;
  filterFn: NzTableFilterFn<SocietyMember> | null;
  filterMultiple: boolean;
  sortDirections: NzTableSortOrder[];
}


@Component({
  selector: 'app-members-table',
  imports: [
    FormsModule,
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
    NzToolTipModule,
    NzModalModule,
    NzAvatarModule,
    EditMemberFormComponent
],
  templateUrl: './members-table.component.html',
  styleUrl: './members-table.component.css'
})
export class MembersTableComponent {
  
  isViewOnly = input<boolean>(false);
  allMembers = input<SocietyMember[]>([]);
  listOfDisplayData: SocietyMember[] = [];

  ngOnInit() {
    this.listOfDisplayData = [...this.allMembers()];
    
  }

  isEditMemberPopupVisible = false;
  isEditMemberLoading = false;
  memberToEdit: SocietyMember | null = null;

  searchValue = '';
  visible = false;
  expandSet = new Set<number>();

  nzMessageService = inject(NzMessageService);

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

  private static dateSortFn(a: SocietyMember, b: SocietyMember): number {
    return new Date(a.joinDate).getTime() - new Date(b.joinDate).getTime();
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
      sortFn: MembersTableComponent.localeSortFn<SocietyMember>('firstName'),
      sortDirections: ['ascend', 'descend', null],
      filterMultiple: false,
      listOfFilter: [],
      filterFn: (search: string, item: SocietyMember) =>
        item.firstName.toLowerCase().includes(search.toLowerCase()),
    },
    {
      name: 'Section / Position',
      sortOrder: null,
      sortFn: MembersTableComponent.localeSortFn<SocietyMember>('position'),
      sortDirections: ['ascend', 'descend', null],
      filterMultiple: true,
      listOfFilter: [],
      filterFn: MembersTableComponent.filterFn<SocietyMember>('position')
    },
    {
      name: 'Member since',
      sortOrder: null,
      sortFn: MembersTableComponent.dateSortFn,
      sortDirections: ['ascend', 'descend', null],
      filterMultiple: true,
      listOfFilter: [],
      filterFn: (list: string[], item: SocietyMember) =>
        list.some(date => new Date(item.joinDate).toDateString() === new Date(date).toDateString())
    },
  ];


  reset(): void {
    this.searchValue = '';
    this.search();
  }

  search(): void {
    this.visible = false;
    this.listOfDisplayData = this.allMembers().filter((item: SocietyMember) => item.firstName.indexOf(this.searchValue) !== -1);
  }

  removeMember(id: string): void {
    this.nzMessageService.success(`Removed member with ID: ${id} from the society.`);
  }

  openEditMemberPopup(id: string): void {
    this.isEditMemberPopupVisible = true;
    this.memberToEdit = this.allMembers().find(member => member.id === id) || null;
    this.nzMessageService.info(`Edit member with ID: ${id}.`);
  }

  handleCancelEditMember(): void {
    this.isEditMemberPopupVisible = false;
    this.memberToEdit = null;
  }

  handleOkEditMember(): void {
    this.isEditMemberLoading = true;
    setTimeout(() => {
      this.isEditMemberPopupVisible = false;
      this.isEditMemberLoading = false;
      this.memberToEdit = null;
      this.nzMessageService.success('Member edited successfully.');
    }, 1000);
  }
}
