// global-loader.component.ts
import { Component, inject, signal } from '@angular/core';
import { LoaderService } from '../common/services/loader.service';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-global-loader',
  imports: [
    NzSpinModule
    
  ],
  template: `
  @if(loader.loading()) {
    <nz-spin
      nzTip="Loading..."
      [nzSpinning]="true"
      class="global-loader"
    ></nz-spin>
  }
   
  `,
  styles: [`
    .global-loader {
      position: fixed;
      top: 0;
      left: 0;
      z-index: 10000;
      width: 100vw;
      height: 100vh;
      display: flex;
      align-items: center;
      justify-content: center;
      background-color: rgba(255, 255, 255, 0.3);
      pointer-events: all;
    }
  `]
})
export class GlobalLoaderComponent {
    loader = inject(LoaderService);
}
