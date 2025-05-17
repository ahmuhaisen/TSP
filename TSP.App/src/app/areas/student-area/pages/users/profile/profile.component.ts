import { Component, inject } from '@angular/core';
import { NzDividerComponent } from 'ng-zorro-antd/divider';
import { ContainerBlockComponent } from "../../../../../components/container-block.component";
import { ProfilesService, UserProfile } from '../../../../../common/services/profiles.service';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { TruncatePipe } from '../../../../../common/pipes/truncate.pipe';
import { CapitalizeFirstPipe } from '../../../../../common/pipes/capitalize-first.pipe';
import { DatePipe } from '@angular/common';
import { NzSkeletonModule } from 'ng-zorro-antd/skeleton';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { AuthService } from '../../../../../common/services/auth.service';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzUploadModule, NzUploadFile } from 'ng-zorro-antd/upload';
import { NzMessageService } from 'ng-zorro-antd/message';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Observable, Observer } from 'rxjs';
import { NzPopoverModule } from 'ng-zorro-antd/popover';
import { environment } from '../../../../../../environments/environment';
import { GenProfileComponent } from '../../../../../components/gen-profile/gen.profile.component';

export interface SuggestedPerson {
  id: string;
  fullName: string;
  userType: string;
  department: string;
  profileImageId?: string;
  mutualSocieties: number;
}

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
    NzButtonModule,
    NzModalModule,
    NzFormModule,
    NzInputModule,
    NzUploadModule,
    NzPopoverModule,
    NzDividerComponent,
    ReactiveFormsModule,
    ContainerBlockComponent,
    RouterModule,
    GenProfileComponent
],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent {
  baseProfileImageUrl = environment.gitHubUsersPicturesURL;


  profilesService = inject(ProfilesService);
  authService = inject(AuthService);
  router = inject(Router);
  activatedRoute = inject(ActivatedRoute);
  fb = inject(FormBuilder);
  messageService = inject(NzMessageService);

  userProfile: UserProfile | null = null;
  isLoading = false;
  isEditProfileModalVisible = false;
  isEditProfileLoading = false;
  profileForm!: FormGroup;
  fileList: NzUploadFile[] = [];
  uploadedImageUrl: string | null = null;
  isImageUploading = false;
  showRemovePhotoPopover = false;
  suggestedPeople: SuggestedPerson[] = [];
  isSuggestedPeopleLoading = false;

  ngOnInit() {
    this.activatedRoute.params.subscribe(params => {
      const id = params['id'];
      const userType = this.activatedRoute.snapshot.queryParamMap.get('userType') ?? 'Student';
      console.log(userType)
      console.table(userType, id);


      this.isLoading = true;
      this.profilesService.find(id, userType).subscribe({
        next: res => {
          this.userProfile = res;
          console.table(res);
          this.isLoading = false;
          this.initForm();
        },
        error: err => {
          console.error(err);
          this.isLoading = false;
          this.router.navigate(['/404']);
        }
      });
    });
  }

  initForm(): void {
    this.profileForm = this.fb.group({
      fullName: [this.userProfile?.fullName, [Validators.required]],
      email: [this.userProfile?.email, [Validators.required, Validators.email]],
      number: [this.userProfile?.number, [Validators.required]],
      department: [{ value: this.userProfile?.department, disabled: true }],
      school: [{ value: this.userProfile?.school, disabled: true }]
    });
  }

  showEditProfileModal(): void {
    this.isEditProfileModalVisible = true;
    this.uploadedImageUrl = null;
    this.showRemovePhotoPopover = false;
    if (this.userProfile?.profileImageId) {
      this.fileList = [
        {
          uid: '-1',
          name: 'profile-image.png',
          status: 'done',
          url: this.userProfile.profileImageId
        }
      ];
    } else {
      this.fileList = [];
    }
  }

  handleCancelEditProfile(): void {
    this.isEditProfileModalVisible = false;
    this.fileList = [];
    this.uploadedImageUrl = null;
    this.showRemovePhotoPopover = false;
  }

  handleOkEditProfile(): void {
    if (this.profileForm.valid) {
      this.isEditProfileLoading = true;

      if (this.userProfile) {
        const updatedProfile: Partial<UserProfile> = {
          fullName: this.profileForm.value.fullName,
          email: this.profileForm.value.email,
          number: this.profileForm.value.number
        };

        // If a new profile image was uploaded, include it in the update
        if (this.uploadedImageUrl) {
          updatedProfile.profileImageId = this.uploadedImageUrl;
        } else if (this.fileList.length === 0 && this.userProfile.profileImageId) {
          // If the user removed their profile image
          updatedProfile.profileImageId = "";
        }

        // Log the form data to the console
        console.log('Form data to be submitted:', {
          ...updatedProfile,
          profileImageId: updatedProfile.profileImageId ?
            `Base64 image string (${updatedProfile.profileImageId.substring(0, 30)}...)` :
            'No image provided'
        });
        console.log('Form is valid:', this.profileForm.valid);
        console.log('Form values:', this.profileForm.value);

        // In a real application, you would first upload the image to a server
        // and get back a URL to store in the profile

        this.profilesService.update(this.userProfile.id, (this.activatedRoute.snapshot.queryParamMap.get('userType') ?? 'Student') as 'Faculty' | 'Student', updatedProfile)
          .subscribe({
            next: (response) => {
              // First close the modal and stop loading
              this.isEditProfileLoading = false;
              this.isEditProfileModalVisible = false;

              // Then update the local userProfile object with the response
              this.messageService.success('Profile updated successfully');

              // Reset the uploaded image URL
              this.uploadedImageUrl = null;
              this.fileList = [];
              this.showRemovePhotoPopover = false;
            },
            error: (error) => {
              console.error('Error updating profile:', error);
              this.isEditProfileLoading = false;
              this.messageService.error('Failed to update profile. Please try again.');
            }
          });
      }
    } else {
      Object.values(this.profileForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }

  beforeUpload = (file: NzUploadFile): boolean => {
    const isJpgOrPng = file.type === 'image/jpeg' || file.type === 'image/png';
    if (!isJpgOrPng) {
      this.messageService.error('You can only upload JPG or PNG files!');
      return false;
    }

    const isLt2M = (file.size || 0) / 1024 / 1024 < 2;
    if (!isLt2M) {
      this.messageService.error('Image must be smaller than 2MB!');
      return false;
    }

    // Handle the file upload
    this.handleImageUpload(file);

    return false; // Prevent automatic upload
  };

  handleImageUpload(file: NzUploadFile): void {
    this.isImageUploading = true;

    // Create a reader to preview the image
    const reader = new FileReader();
    reader.onload = (e: any) => {
      // Get the base64 string (this is what will be sent to the backend)
      const base64String = e.target.result;

      // Store the base64 string for later use
      this.uploadedImageUrl = base64String;

      // Update the file list for display
      this.fileList = [
        {
          uid: file.uid || '-1',
          name: file.name || 'image.png',
          status: 'done',
          url: base64String
        }
      ];

      this.isImageUploading = false;
      this.messageService.success('Image uploaded successfully');

      // Log the base64 string length to console
      console.log('Base64 image string length:', base64String.length);
    };

    // Read the file as a data URL (this will give us a base64 string)
    if (file instanceof File) {
      reader.readAsDataURL(file);
    } else if (file.originFileObj) {
      reader.readAsDataURL(file.originFileObj);
    }
  }

  removeProfileImage(): void {
    this.fileList = [];
    this.uploadedImageUrl = null;
    this.showRemovePhotoPopover = false;
    this.authService.setCurrentUserProfileImageId('');
    this.messageService.success('Profile image removed');
    console.log('Profile image removed by user');
  }
}
