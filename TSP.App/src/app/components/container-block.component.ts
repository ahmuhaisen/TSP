import { Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NzIconModule } from 'ng-zorro-antd/icon';

@Component({
  selector: 'app-container-block',
  imports: [
    RouterLink,
    NzIconModule
  ],
  template: `
  <div class="w-Full p-4 text-gray-700 shadow rounded-lg border-t-2 border-primary-light">
    <div class="flex justify-between items-center mb-4">
        <p class="flex items-center font-bold text-lg">
          @if(header().icon){
            <nz-icon [nzType]="header().icon!" class="mr-2" />
          }
          {{header().title}}
        </p>
        @if(header().link){
          <a class="text-primary-light text-xs" [routerLink]="header().link">View all</a>
        }
    </div>

    <ng-content></ng-content>

  </div>
  `,
  styles: `
  
  `
})
export class ContainerBlockComponent {

  header = input.required<{title: string, icon: string | null, link: string | null}>();
}
