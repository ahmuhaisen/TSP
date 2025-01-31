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
})
export class EditSocietyInfoFormComponent {

  society = input<{ id: string, name: string, description: string, creationDate: Date, themeColor: string, logo: string, advisorId: number }>();

  formBuilder = inject(FormBuilder);
  messageService = inject(NzMessageService);

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
  }

  isFacultyMembersLoading = false;

  facultyMembers = [
    { id: 1, name: 'Sami Sarhan' },
    { id: 2, name: 'Heba Sa\'adeh' },
    { id: 3, name: 'Abdalbast Assaf' },
    { id: 4, name: 'Shirinaz Alhaj Baddar' },
    { id: 5, name: 'Osama Harfoshi' },
    { id: 6, name: 'Basma Shqairat' }
  ];

  displayedFacultyMembers = [...this.facultyMembers];

  setFormValues() {
    this.createSocietyForm!.get('name')?.setValue(this.society()!.name);
    this.createSocietyForm!.get('description')?.setValue(this.society()!.description);
    this.createSocietyForm!.get('creationDate')?.setValue(this.society()!.creationDate);
    this.createSocietyForm!.get('themeColor')?.setValue(this.society()!.themeColor);
    this.createSocietyForm!.get('logo')?.setValue(this.society()!.logo);
    this.createSocietyForm!.get('advisorId')?.setValue(this.society()!.advisorId);
  }

  onSearchFacultyMembers(value: string): void {
    //this.isFacultyMembersLoading = true;
    this.displayedFacultyMembers = this.facultyMembers.filter(member => member.name.toLowerCase().includes(value.toLowerCase()));
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
