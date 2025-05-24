import { Component, effect, EventEmitter, Input, input, Output, output, signal } from '@angular/core';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDrawerModule } from 'ng-zorro-antd/drawer';
import { NzEmptyComponent } from 'ng-zorro-antd/empty';
import { NzIconModule, NzIconPatchService } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { FormsModule } from '@angular/forms';
import { SearchBasicDTO, SearchService } from '../../common/services/search.service';
import { CommonModule } from '@angular/common';
import { inject } from '@angular/core';
import { NzMessageService } from 'ng-zorro-antd/message';
import { environment } from '../../../environments/environment';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-search-drawer',
  imports: [
    NzDrawerModule,
    NzIconModule,
    NzButtonModule,
    NzSpinModule,
    NzInputModule,
    NzSelectModule,
    NzEmptyComponent,
    FormsModule,
    CommonModule,
    NzAvatarModule,
    RouterModule
  ],
  templateUrl: './search-drawer.component.html',
  styleUrl: './search-drawer.component.css'
})
export class SearchDrawerComponent {

  isSearchLoading = signal(false);
  // Parent component sends the initial visible value.
  @Input() visible: boolean = false;
  // This EventEmitter lets the child notify the parent of changes.
  @Output() visibleChange = new EventEmitter<boolean>();
  constructor(
    private searchService: SearchService
  ) {

  }


  selectedSearchType: string = "";
  searchTerm: string = "";
  searchResults: SearchBasicDTO[] = [];
  messageService = inject(NzMessageService);
  baseUrl: string = "";
  routingDirection: string = "";
  searchType: string = "";
  flag: string = "";
  doSearch() {
    this.flag = "";
    switch (this.selectedSearchType) {
      case "1":
        this.searchType = "Societies";
        this.baseUrl = environment.gitHubSocietiesPicturesURL;
        break;
      case "2":
        this.searchType = "Students";
        this.baseUrl = environment.gitHubUsersPicturesURL;
        break;
      case "3":
        this.searchType = "FacultyMembers";
        this.baseUrl = environment.gitHubUsersPicturesURL;
        break;
      case "4":
        this.searchType = "Events";
        break;
      default: this.messageService.error("please select the search category"); return;
    }
    if (this.searchTerm.length < 1) {
      return;
    }
    this.searchService.getSearchResults(this.searchType, this.searchTerm).subscribe(data => {
      this.searchResults = data;
      if (this.selectedSearchType === "2" || this.selectedSearchType === "3") {
        this.searchType = "users"
        if (this.selectedSearchType === "3")
          this.flag = "?userType=FacultyMember";
      }

    });
    console.log(this.searchResults)

    this.isSearchLoading.set(false);
  }

  close() {
    this.visible = false;
    this.visibleChange.emit(this.visible);
  }
}

