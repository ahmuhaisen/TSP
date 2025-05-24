import { Component, computed, inject, input, OnInit, output } from '@angular/core';
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
import { NzFormModule } from 'ng-zorro-antd/form';
import { Member, SocietyMember } from '../../../areas/system-admin-area/api-interfaces/society.types';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { SocietiesService } from '../../../areas/system-admin-area/services/societies.service';
import { environment } from '../../../../environments/environment';
import { PageMode } from '../../../common/types/presentaion.types';
import { RouterLink } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
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
    NzFormModule,
    RouterLink,


  ],
  templateUrl: './members-table.component.html',
  styleUrl: './members-table.component.css'
})
export class MembersTableComponent implements OnInit {

  pageMode = input<PageMode>('VIEW_ONLY');
  isLoading = input<boolean>(false);
  allMembers = input.required<SocietyMember[]>();
  societyId = input.required<string>();
  membersChange = output<SocietyMember[]>();

  isViewOnly = computed(() => this.pageMode() === 'VIEW_ONLY');
  isStudentManage = computed(() => this.pageMode() === 'STUDENT_MANAGE');

  listOfDisplayData: SocietyMember[] = [];
  searchValue = '';
  visible = false;
  expandSet = new Set<number>();

  messageService = inject(NzMessageService);
  societiesService = inject(SocietiesService);
  baseUserUmage: string = environment.gitHubUsersPicturesURL
  isEditMemberPopupVisible = false;
  isEditMemberLoading = false;
  memberToEdit: SocietyMember | null = null;
  editPosition = '';
  activateRoute = inject(ActivatedRoute)
  routeFirstSegment: string = ""
  ngOnInit() {
    this.listOfDisplayData = [...this.allMembers()];
    this.routeFirstSegment = this.activateRoute.snapshot.pathFromRoot[1]?.url[0]?.path;
  }

  ngOnChanges() {
    if (this.allMembers()) {
      this.listOfDisplayData = [...this.allMembers()];
    }
  }

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
    this.listOfDisplayData = [...this.allMembers()];
  }

  search(): void {
    this.visible = false;
    this.listOfDisplayData = this.allMembers().filter((item: SocietyMember) =>
      item.firstName.toLowerCase().includes(this.searchValue.toLowerCase()) ||
      item.lastName.toLowerCase().includes(this.searchValue.toLowerCase())
    );
  }

  removeMember(id: string): void {
    this.societiesService.removeMember(this.societyId(), id).subscribe({
      next: () => {
        this.messageService.success('Member removed successfully');
        const updatedMembers = this.allMembers().filter(member => member.id !== id);
        this.membersChange.emit(updatedMembers);
      },
      error: (error: unknown) => {
        this.messageService.error('Failed to remove member');
        console.error('Error removing member:', error);
      }
    });
  }

  openEditMemberPopup(id: string): void {
    this.memberToEdit = this.allMembers().find(member => member.id === id) || null;
    if (this.memberToEdit) {
      this.editPosition = this.memberToEdit.position;
      this.isEditMemberPopupVisible = true;
    }
  }

  handleCancelEditMember(): void {
    this.isEditMemberPopupVisible = false;
    this.memberToEdit = null;
    this.editPosition = '';
  }

  handleOkEditMember(): void {
    if (!this.memberToEdit || !this.editPosition.trim()) {
      this.messageService.error('Please enter a valid position');
      return;
    }

    this.isEditMemberLoading = true;
    this.societiesService.editMember(this.memberToEdit.id, this.societyId(), this.editPosition).subscribe({
      next: () => {
        this.messageService.success('Member position updated successfully');
        // Update the local list
        const updatedMembers = this.allMembers().map(member =>
          member.id === this.memberToEdit!.id
            ? { ...member, position: this.editPosition }
            : member
        );
        this.membersChange.emit(updatedMembers);
        this.listOfDisplayData = updatedMembers;
        this.handleCancelEditMember();
      },
      error: (error: unknown) => {
        this.isEditMemberLoading = false;
        this.messageService.error('Failed to update member position');
        console.error('Error updating member position:', error);
      },
      complete: () => {
        this.isEditMemberLoading = false;
      }
    });
  }
}
