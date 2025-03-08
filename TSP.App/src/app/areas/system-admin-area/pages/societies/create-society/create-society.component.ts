import { Component, inject, ViewChild } from '@angular/core';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzMessageService } from 'ng-zorro-antd/message';
import { Router } from '@angular/router';
import { EditSocietyInfoFormComponent } from '../../../../../components/society-details/edit-society-info-form/edit-society-info-form.component';
import { SocietiesService } from '../../../services/societies.service';
import { PostSociety, Society } from '../../../api-interfaces/society.types';

@Component({
  selector: 'app-create-society',
  imports: [
    NzBreadCrumbModule,
    NzButtonModule,
    NzIconModule,
    NzDividerModule,
    EditSocietyInfoFormComponent,
],
  templateUrl: './create-society.component.html',
  styleUrl: './create-society.component.css'
})
export class CreateSocietyComponent {

  router = inject(Router);
  messageService = inject(NzMessageService);
  societiesService = inject(SocietiesService);


  @ViewChild(EditSocietyInfoFormComponent) editSocietyInfoFormComponent?: EditSocietyInfoFormComponent;

  submitForm(): void {
    if(this.editSocietyInfoFormComponent!.createSocietyForm!.invalid ) {
      this.messageService.error('Please fill in all required fields.');
      return;
    }

    console.log('submit', this.editSocietyInfoFormComponent!.createSocietyForm!.value);

    const society:PostSociety = {
      name: this.editSocietyInfoFormComponent!.createSocietyForm!.value.name,
      description: this.editSocietyInfoFormComponent!.createSocietyForm!.value.description,
      logoBase64: this.editSocietyInfoFormComponent!.createSocietyForm!.value.logo,
      creationDate: this.editSocietyInfoFormComponent!.createSocietyForm!.value.creationDate.toISOString().split('T')[0],
      themeColor: this.editSocietyInfoFormComponent!.createSocietyForm!.value.themeColor,
      advisorId: this.editSocietyInfoFormComponent!.createSocietyForm!.value.advisorId
    }
    this.societiesService.create(society).subscribe({
      next: res => {
        this.messageService.success('Society created successfully.');
        this.router.navigate(['admin-area/societies']);
      }
    });
  }

  cancelForm(): void {
    if(this.editSocietyInfoFormComponent!.createSocietyForm!.dirty) {
      this.messageService.warning('Society creation cancelled.');
    }

    this.editSocietyInfoFormComponent!.createSocietyForm!.reset();
    this.router.navigate(['admin-area/societies']);
  }

}
