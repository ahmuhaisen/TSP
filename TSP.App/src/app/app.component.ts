import { Component, inject } from '@angular/core';
import { NavigationEnd, NavigationStart, Router, RouterOutlet } from '@angular/router';

import { ProgressbarLoaderService } from './common/services/progressbar-loader.service';
import { GlobalLoaderComponent } from "./components/global-loader.component";

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    GlobalLoaderComponent
],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {

  router = inject(Router);
  progressbarService = inject(ProgressbarLoaderService);

  ngOnInit() {
    this.router.events.subscribe((event) => {
      if(event instanceof NavigationStart) {
        this.progressbarService.start();
      } else if (event instanceof NavigationEnd) {
        this.progressbarService.stop();
      }
    })
  }

}
