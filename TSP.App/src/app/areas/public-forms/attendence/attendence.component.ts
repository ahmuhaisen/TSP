import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzCascaderModule, NzCascaderOption } from 'ng-zorro-antd/cascader';
import { NzButtonComponent } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';

@Component({
  selector: 'app-attendence',
  imports: [
    NzIconModule,
    NzFormModule,
    NzInputModule,
    NzButtonComponent,
    NzDividerModule,
    ReactiveFormsModule,
    NzCascaderModule
  ],
  templateUrl: './attendence.component.html',
  styleUrl: './attendence.component.css'
})
export class AttendenceComponent {
  currentYear = new Date().getFullYear();
  nzOptions: NzCascaderOption[] = schools;


  fb = inject(FormBuilder);

  form = this.fb.group({
    fullName: [null, [Validators.required]],
    email: [null, [Validators.required, Validators.email]],
    uniNumber: [null, [Validators.required]],
    phone: [null, [Validators.required]],
    department: [null, [Validators.required]],
    notes: [null, [Validators.maxLength(200)]]
  });

  submitForm(): void {
    console.log('submit', this.form.value);
  }
}

const schools: NzCascaderOption[] = [
  {
    value: '11',
    label: 'King Abdullah II School for Information Technology',
    children: [
      {
        value: '1',
        label: 'Computer Science',
        isLeaf: true
      },
      {
        value: '2',
        label: 'Computer Information Systems',
        isLeaf: true
      },
      {
        value: '3',
        label: 'Information Technology',
        isLeaf: true
      },
      {
        value: '4',
        label: 'Artificial Intelligence',
        isLeaf: true
      }
    ]
  },
  {
    value: '22',
    label: 'School of Engineering',
    children: [
      {
        value: '5',
        label: 'Computer Engineering',
        isLeaf: true
      },
      {
        value: '6',
        label: 'Electrical Engineering',
        isLeaf: true
      },
      {
        value: '7',
        label: 'Mechanical Engineering',
        isLeaf: true
      },
      {
        value: '8',
        label: 'Civil Engineering',
        isLeaf: true
      },
      {
        value: '9',
        label: 'Industrial Engineering',
        isLeaf: true
      }
    ]
  },
  {
    value: '33',
    label: 'Other',
    isLeaf: true
  }
];