import { HttpClient } from '@angular/common/http';
import { Injectable, Signal, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { environment } from '../../../../environments/environment';

export interface StudentEvent {
  id: string;
  title: string;
  society: string;
  date: Date;
  location: string;
  imageUrl: string;
  isMember: boolean;
}

export interface HomeStatistics {
  NumSocieties: number;
  NumAttendedEvents: number;
  // Add other statistics as needed
}

@Injectable({
  providedIn: 'root'
})
export class HomeService {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiURL}studentArea/home`;

  getRecentEvents() {
    return this.http.get<StudentEvent[]>(`${this.baseUrl}/recentEvents`);
  }

  getHomeStatistics() {
    return this.http.get<HomeStatistics>(`${this.baseUrl}/homeStatistics`);
  }
}
