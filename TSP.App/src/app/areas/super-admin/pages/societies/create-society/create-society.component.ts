import { Component, inject, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalModule } from 'ng-zorro-antd/modal';

import { SocietiesService } from '../../../services/societies.service';
import { PostSociety } from '../../../../system-admin-area/api-interfaces/society.types';
import { EditSocietyInfoFormComponent } from '../../../../../components/society-details/edit-society-info-form/edit-society-info-form.component';

@Component({
  selector: 'app-create-society',
  standalone: true,
  imports: [
    CommonModule,
    NzButtonModule,
    NzIconModule,
    NzDividerModule,
    NzModalModule,
    EditSocietyInfoFormComponent
  ],
  templateUrl: './create-society.component.html',
  styleUrl: './create-society.component.css'
})
export class CreateSocietyComponent {
  @ViewChild(EditSocietyInfoFormComponent) societyForm!: EditSocietyInfoFormComponent;
  
  isSubmitting = false;
  isWorkflowModalVisible = false;

  societiesService = inject(SocietiesService);
  router = inject(Router);
  message = inject(NzMessageService);

  showWorkflowModal(): void {
    this.isWorkflowModalVisible = true;
  }

  handleModalCancel(): void {
    this.isWorkflowModalVisible = false;
  }

  submitForm(): void {
    if (this.societyForm.createSocietyForm?.invalid) {
      Object.values(this.societyForm.createSocietyForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity();
        }
      });
      return;
    }

    this.isSubmitting = true;
    
    const formValues = this.societyForm.createSocietyForm?.value;
    if (!formValues) {
      this.message.error('Form values are missing');
      this.isSubmitting = false;
      return;
    }
    
    // Format the date to YYYY-MM-DD format without time
    const date = formValues.creationDate;
    const formattedDate = date instanceof Date ? 
      `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2, '0')}-${String(date.getDate()).padStart(2, '0')}` : 
      new Date().toISOString().split('T')[0];
    
    const society: PostSociety = {
      name: formValues.name,
      description: formValues.description,
      logoBase64: formValues.logo,
      creationDate: formattedDate,
      themeColor: formValues.themeColor,
      advisorId: formValues.advisorId
    };

    this.societiesService.create(society).subscribe({
      next: () => {
        this.message.success('Society created successfully');
        this.router.navigate(['/super-admin/societies']);
      },
      error: () => {
        this.message.error('Failed to create society');
        this.isSubmitting = false;
      }
    });
  }

  cancelForm(): void {
    this.router.navigate(['/super-admin/societies']);
  }
} 