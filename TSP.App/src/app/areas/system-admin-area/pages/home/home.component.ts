import { Component, inject, signal } from '@angular/core';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { NzPopoverModule } from 'ng-zorro-antd/popover';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzDrawerModule, NzDrawerPlacement } from 'ng-zorro-antd/drawer';
import { NzSkeletonModule } from 'ng-zorro-antd/skeleton';
import { DatePipe, NgFor, NgIf } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { HomeService } from '../../services/home.service';
import { HomeStatistics, RecentEvent, RecentlyJoinedMember } from '../../api-interfaces/home.types';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { AuthService, User } from '../../../../common/services/auth.service';
import { SearchDrawerComponent } from "../../../../components/search-drawer/search-drawer.component";
import { environment } from '../../../../../environments/environment';
import { CookieService } from 'ngx-cookie-service';
import { LoaderService } from '../../../../common/services/loader.service';

@Component({
  selector: 'app-home',
  imports: [
    NgIf,
    NgFor,
    DatePipe,
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
    NzEmptyModule,
    NzAvatarModule,
    SearchDrawerComponent
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  baseProfileImageUrl = environment.gitHubUsersPicturesURL;

  cookieService = inject(CookieService);
  profileImageId = this.cookieService.get("profile_image");
  isLoading = false;
  isSearchLoading = false;
  userInfo = signal<User | null>(null);

  recentEvents: RecentEvent[] = [];
  recentlyJoinedMembers: RecentlyJoinedMember[] = [];
  homeStatistics: HomeStatistics | null = null;

  homeService = inject(HomeService);
  authService = inject(AuthService);
  loaderService = inject(LoaderService);

  ngOnInit() {
    this.isLoading = true;
    this.loaderService.show();

    this.homeService.recentEvents().subscribe(res => {
      this.recentEvents = res;
    });
    this.homeService.recentlyJoinedMembers().subscribe(res => {
      this.recentlyJoinedMembers = res;
    });
    this.homeService.homeStatistics().subscribe(res => {
      this.homeStatistics = res;
      this.loaderService.hide();
    });

    this.userInfo.set(this.authService.currentUser());

    this.isLoading = false;
  }

  // Drawer
  isSearchVisible = false;
  openSearch(): void {
    this.isSearchVisible = true;
  }

}
