import { Component, inject, input, OnChanges, SimpleChanges } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzColorPickerComponent } from 'ng-zorro-antd/color-picker';
import { NzUploadChangeParam, NzUploadModule } from 'ng-zorro-antd/upload';
import { NzMessageService } from 'ng-zorro-antd/message';
import { SocietyWithAdvisor } from '../../../areas/system-admin-area/api-interfaces/society.types';
import { facultyMemberBasicDetails, FacultyMembersService } from '../../../common/services/faculty-member.service';
import { SocietiesService } from '../../../areas/system-admin-area/services/societies.service';

@Component({
  selector: 'app-edit-society-info-form',
  imports: [
    NzBreadCrumbModule,
    NzButtonModule,
    NzIconModule,
    NzDividerModule,
    ReactiveFormsModule,
    NzFormModule,
    NzInputModule,
    NzDatePickerModule,
    NzColorPickerComponent,
    NzUploadModule,
    NzSelectModule
  ],
  templateUrl: './edit-society-info-form.component.html',
  styles: [`
    .avatar-uploader {
      display: flex;
      justify-content: center;
      align-items: center;
      margin-bottom: 10px;
      width: 100%;
    }

    :host ::ng-deep .ant-upload.ant-upload-select-picture-card {
      width: 150px;
      height: 100px;
      border-radius: 12px;
      overflow: hidden;
    }

    :host ::ng-deep .ant-upload-list-picture-card-container {
      width: 144px;
      height: 144px;
      border-radius: 12px;
      overflow: hidden;
    }

    :host ::ng-deep .ant-form-item-label {
      text-align: left;
    }

    :host ::ng-deep .ant-form-vertical .ant-form-item-label {
      padding-bottom: 4px;
    }

    :host ::ng-deep .ant-form-vertical .ant-form-item {
      margin-bottom: 16px;
    }
  `]
})
export class EditSocietyInfoFormComponent implements OnChanges {

  society = input<SocietyWithAdvisor>();

  formBuilder = inject(FormBuilder);
  messageService = inject(NzMessageService);
  facultyMembersService = inject(FacultyMembersService);

  createSocietyForm: FormGroup | undefined;

  ngOnInit() {
    console.log('EditSocietyInfoFormComponent ngOnInit called');
    let tempThemeColor: string = "";
    if (this.society() == null || this.society()?.themeColor == null) {
      tempThemeColor = "#030000";
    } else {
      tempThemeColor = this.society()?.themeColor || "";
    }
    this.createSocietyForm = this.formBuilder.group({
      name: ['', [Validators.required]],
      description: ['', [Validators.required, Validators.maxLength(200)]],
      creationDate: [new Date(), [Validators.required]],
      themeColor: [tempThemeColor, []],
      logo: ['', [Validators.required]],
      advisorId: [null, [Validators.required]]
    });

    // Load faculty members first
    this.facultyMembersService.all().subscribe(res => {
      console.log('Faculty members loaded:', res.length);
      this.facultyMembers = res;
      this.displayedFacultyMembers = [...this.facultyMembers];
      console.log(this.displayedFacultyMembers)
      // Apply society values after faculty members are loaded
      if (this.society()) {
        console.log('Setting form values after faculty members loaded');
        this.setFormValues();
      }
    });
  }

  ngOnChanges(changes: SimpleChanges) {
    console.log('EditSocietyInfoFormComponent ngOnChanges:', changes);
    if (changes['society'] && this.createSocietyForm) {
      console.log('Society changed, updating form values');
      this.setFormValues();
    }
  }

  isFacultyMembersLoading = false;

  facultyMembers: facultyMemberBasicDetails[] = [];

  displayedFacultyMembers: facultyMemberBasicDetails[] = [];

  setFormValues() {
    if (!this.society() || !this.createSocietyForm) {
      console.log('Cannot set form values: society or form is not available');
      return;
    }

    const societyData = this.society()!;
    console.log('Setting form values with society data:', societyData);
    console.log('Advisor ID to set:', societyData.advisor?.id);

    this.createSocietyForm.get('name')?.setValue(societyData.name);
    this.createSocietyForm.get('description')?.setValue(societyData.description);
    this.createSocietyForm.get('creationDate')?.setValue(societyData.creationDate);
    this.createSocietyForm.get('themeColor')?.setValue(societyData.themeColor);
    this.createSocietyForm.get('logo')?.setValue(societyData.logoId);

    if (societyData.advisor && societyData.advisor.id) {
      console.log('Setting advisor ID to:', societyData.advisor.id);
      this.createSocietyForm.get('advisorId')?.setValue(societyData.advisor.id);

      // Force form update after a brief delay to ensure the UI catches up
      setTimeout(() => {
        this.createSocietyForm?.get('advisorId')?.updateValueAndValidity({ onlySelf: false, emitEvent: true });
        console.log('Current advisor value after update:', this.createSocietyForm?.get('advisorId')?.value);
      }, 100);
    } else {
      console.log('No advisor data found in society');
    }
  }

  onSearchFacultyMembers(value: string): void {
    console.log('Searching faculty members:', value);
    this.displayedFacultyMembers = this.facultyMembers.filter(
      member => member.fullName.toLowerCase().includes(value.toLowerCase())
    );
  }

  removeLogo(): void {
    this.createSocietyForm!.get('logo')?.setValue('');
    this.messageService.success('Logo removed successfully');
  }

  handleImageUpload({ file }: NzUploadChangeParam): void {
    if (file.status === 'done' || file.originFileObj) {
      const reader = new FileReader();
      reader.readAsDataURL(file.originFileObj as Blob);
      reader.onload = () => {
        const base64 = reader.result as string;
        if (this.isValidImageType(base64)) {
          this.createSocietyForm!.get('logo')?.setValue(base64);
        } else {
          this.messageService.error('Please upload a valid image file.');
        }
      };
    }
  }

  isValidImageType(base64: string): boolean {
    const mimeType = base64.split(';')[0].split(':')[1];
    return mimeType === 'image/png' || mimeType === 'image/jpeg' || mimeType === 'image/jpg' || mimeType === 'image/bmp';
  }

}
