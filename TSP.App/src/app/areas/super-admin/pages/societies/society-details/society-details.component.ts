import { Component, inject, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { RouterLink, Router, ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';

import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzSpinModule } from 'ng-zorro-antd/spin';

import { SocietiesService } from '../../../services/societies.service';
import { SocietyWithAdvisor } from '../../../../system-admin-area/api-interfaces/society.types';
import { EditSocietyInfoFormComponent } from '../../../../../components/society-details/edit-society-info-form/edit-society-info-form.component';

@Component({
  selector: 'app-society-details',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    NzButtonModule,
    NzIconModule,
    NzDividerModule,
    NzSpinModule,
    EditSocietyInfoFormComponent
  ],
  templateUrl: './society-details.component.html',
  styleUrl: './society-details.component.css'
})
export class SocietyDetailsComponent implements OnInit, AfterViewInit {
  @ViewChild(EditSocietyInfoFormComponent) societyForm!: EditSocietyInfoFormComponent;
  
  isSubmitting = false;
  isLoading = true;
  societyId = '';
  society: SocietyWithAdvisor | null = null;
  viewInitialized = false;

  societiesService = inject(SocietiesService);
  router = inject(Router);
  route = inject(ActivatedRoute);
  message = inject(NzMessageService);

  ngOnInit() {
    console.log('Society details component initialized');
    this.societyId = this.route.snapshot.paramMap.get('id') || '';
    if (this.societyId) {
      this.loadSociety();
    } else {
      this.message.error('No society ID provided');
      this.router.navigate(['/super-admin/societies']);
    }
  }

  ngAfterViewInit() {
    console.log('Society details view initialized');
    this.viewInitialized = true;
  }

  loadSociety() {
    console.log('Loading society data for ID:', this.societyId);
    this.isLoading = true;
    this.societiesService.find(this.societyId).subscribe({
      next: (society) => {
        console.log('Society data loaded:', society);
        console.log('Advisor data:', society.advisor);
        this.society = society;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Failed to load society details:', err);
        this.message.error('Failed to load society details');
        this.router.navigate(['/super-admin/societies']);
      }
    });
  }

  submitForm(): void {
    console.log('Submitting form, checking validity');
    
    if (!this.societyForm || this.societyForm.createSocietyForm?.invalid) {
      if (this.societyForm?.createSocietyForm) {
        console.log('Form is invalid, marking dirty fields');
        console.log('Form errors:', this.societyForm.createSocietyForm.errors);
        
        Object.keys(this.societyForm.createSocietyForm.controls).forEach(key => {
          const control = this.societyForm.createSocietyForm?.get(key);
          console.log(`Control ${key} valid:`, control?.valid, 'errors:', control?.errors);
          if (control?.invalid) {
            control.markAsDirty();
            control.updateValueAndValidity();
          }
        });
      } else {
        console.error('Form component not found or initialized');
      }
      return;
    }

    this.isSubmitting = true;
    
    const formValues = this.societyForm.createSocietyForm?.value;
    if (!formValues) {
      this.message.error('Form values are missing');
      this.isSubmitting = false;
      return;
    }
    
    console.log('Form values to submit:', formValues);
    
    // If there's a creation date in the form, format it properly
    let creationDate = undefined;
    if (formValues.creationDate) {
      const date = formValues.creationDate;
      creationDate = date instanceof Date ? 
        `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2, '0')}-${String(date.getDate()).padStart(2, '0')}` : 
        undefined;
    }
    
    const society = {
      id: this.societyId,
      name: formValues.name,
      description: formValues.description,
      logoBase64: formValues.logo,
      themeColor: formValues.themeColor,
      ...(creationDate && { creationDate }),
      advisorId: formValues.advisorId
    };

    console.log('Sending update request with data:', society);

    this.societiesService.update(this.societyId, society).subscribe({
      next: () => {
        this.message.success('Society updated successfully');
        this.router.navigate(['/super-admin/societies']);
      },
      error: (err) => {
        console.error('Failed to update society:', err);
        this.message.error('Failed to update society');
        this.isSubmitting = false;
      }
    });
  }

  cancelForm(): void {
    this.router.navigate(['/super-admin/societies']);
  }
} 