import { DatePipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { TruncatePipe } from '../../../../common/pipes/truncate.pipe';
import { AuthService, User } from '../../../../common/services/auth.service';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { HomeService } from '../../services/home.service';
import { NzPopoverModule } from 'ng-zorro-antd/popover';
import { StudentEvent } from '../../api-interfaces/event.types';
import { HomeStatistics } from '../../api-interfaces/statistics.types';
import { environment } from '../../../../../environments/environment';
import { SearchDrawerComponent } from '../../../../components/search-drawer/search-drawer.component';
import { CookieService } from 'ngx-cookie-service';
@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    RouterLink,
    DatePipe,
    NzIconModule,
    NzButtonModule,
    NzDividerModule,
    NzAvatarModule,
    NzTagModule,
    NzPopoverModule,
    SearchDrawerComponent
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  societiesPicturesBaseUrl = environment.gitHubSocietiesPicturesURL;
  baseProfileImageUrl = environment.gitHubUsersPicturesURL
  cookieService = inject(CookieService);
  profileImageId = this.cookieService.get("profile_image");
  private authService = inject(AuthService);
  private homeService = inject(HomeService);

  userInfo = signal<User | null>(null);
  events: StudentEvent[] = [];
  statistics: HomeStatistics | null = null;

  ngOnInit() {
    this.userInfo.set(this.authService.currentUser());
    console.log(this.authService.currentUser())
    this.homeService.getRecentEvents().subscribe(res => {
      this.events = res
      console.log(this.events)

    });
    this.homeService.getHomeStatistics().subscribe(res => this.statistics = res);

    console.log('Statistics:', this.statistics, 'Events:', this.events);
  }
  isSearchVisible = false;
  openSearch(): void {
    this.isSearchVisible = true;
  }

}
