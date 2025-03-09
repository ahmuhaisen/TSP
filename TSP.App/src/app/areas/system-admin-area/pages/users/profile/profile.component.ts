import { Component, inject } from '@angular/core';
import { NzDividerComponent } from 'ng-zorro-antd/divider';
import { ContainerBlockComponent } from "../../../../../components/container-block.component";
import { ProfilesService, UserProfile } from '../../../../../common/services/profiles.service';
import { ActivatedRoute, Router } from '@angular/router';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { TruncatePipe } from '../../../../../common/pipes/truncate.pipe';
import { CapitalizeFirstPipe } from '../../../../../common/pipes/capitalize-first.pipe';
import { DatePipe } from '@angular/common';
import { NzSkeletonModule } from 'ng-zorro-antd/skeleton';

@Component({
  selector: 'app-profile',
  imports: [
    DatePipe,
    TruncatePipe,
    CapitalizeFirstPipe,
    NzAvatarModule,
    NzDividerComponent,
    NzIconModule,
    NzEmptyModule,
    NzSkeletonModule,
    ContainerBlockComponent
],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent {

  profilesService = inject(ProfilesService);
  router = inject(Router);
  activatedRoute = inject(ActivatedRoute);

  userProfile: UserProfile | null = null;
  isLoading = false;

  ngOnInit() {
    this.activatedRoute.params.subscribe(params => {
      const id = params['id'];
      const userType = this.activatedRoute.snapshot.queryParamMap.get('userType') ?? 'Student';
      console.table(userType, id);
  
      this.isLoading = true;
      this.profilesService.find(id, userType).subscribe({
        next: res => {
          this.userProfile = res;
          console.table(res);
          this.isLoading = false;
        },
        error: err => {
          console.error(err);
          this.isLoading = false;
          this.router.navigate(['/404']);
        }
      });
    });
  }
  
}
