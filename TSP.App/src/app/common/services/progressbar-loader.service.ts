import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ProgressbarLoaderService {

  private isLoading = signal(false);

  getProgressBarStatus(): boolean {
    return this.isLoading();
  }

  start() {
    this.isLoading.set(true);
  }

  stop() {
    this.isLoading.set(false);
  }
}
