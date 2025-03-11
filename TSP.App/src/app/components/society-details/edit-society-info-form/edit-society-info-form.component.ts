import { Component, inject, input } from '@angular/core';
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
export class EditSocietyInfoFormComponent {

  society = input<SocietyWithAdvisor>();

  formBuilder = inject(FormBuilder);
  messageService = inject(NzMessageService);
  facultyMembersService = inject(FacultyMembersService);

  createSocietyForm: FormGroup | undefined;

  ngOnInit() {
    this.createSocietyForm = this.formBuilder.group({
      name: ['', [Validators.required]],
      description: ['', [Validators.required, Validators.maxLength(200)]],
      creationDate: [new Date(), [Validators.required]],
      themeColor: ['#1677ff', []],
      logo: ['', [Validators.required]],
      advisorId: [null, [Validators.required]]
    });

    if (this.society()) {
      this.setFormValues();
    }

    this.facultyMembersService.all().subscribe(res => this.facultyMembers = res)
  }

  isFacultyMembersLoading = false;

  facultyMembers: facultyMemberBasicDetails[] = [];

  displayedFacultyMembers = [...this.facultyMembers];

  setFormValues() {
    this.createSocietyForm!.get('name')?.setValue(this.society()!.name);
    this.createSocietyForm!.get('description')?.setValue(this.society()!.description);
    this.createSocietyForm!.get('creationDate')?.setValue(this.society()!.creationDate);
    this.createSocietyForm!.get('themeColor')?.setValue(this.society()!.themeColor);
    this.createSocietyForm!.get('logo')?.setValue(this.society()!.logoId);
    this.createSocietyForm!.get('advisorId')?.setValue(this.society()!.advisor.id);
  }

  onSearchFacultyMembers(value: string): void {
    //this.isFacultyMembersLoading = true;
    this.displayedFacultyMembers = this.facultyMembers.filter(member => member.fullName.toLowerCase().includes(value.toLowerCase()));
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
