import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { DbService } from '../../../common/services/db.service';

export interface SocietyMembersCount {
  name: string;
  count: number;
}

export interface EventAttendanceCount {
  eventName: string;
  count: number;
  id: string;
}

export interface SocietyData {
  id: string;
  societyName: string;
  description: string;
  logoUrl: string;
  members: number;
  events: number;
}

export interface EventsPerMonth {
  month: string;
  eventCount: number;
}

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {
  private baseUrl = `AdminArea/Statistics`;

  http = inject(DbService);

  getTopSocietiesByMembers(numberOfSocieties: number): Observable<SocietyMembersCount[]> {
    return this.http.getRequest<SocietyMembersCount[]>(`${this.baseUrl}/TopSocietiesByMembers?numberOfSocities=${numberOfSocieties}`);
  }

  getTopEventsByAttendance(numberOfEvents: number): Observable<EventAttendanceCount[]> {
    return this.http.getRequest<EventAttendanceCount[]>(`${this.baseUrl}/TopEventsByAttendence?numberOfEvents=${numberOfEvents}`);
  }

  getTopSocieties(numberOfSocieties: number): Observable<SocietyData[]> {
    return this.http.getRequest<SocietyData[]>(`${this.baseUrl}/TopSocities?numberOfSocities=${numberOfSocieties}`);
  }

  getEventsByMonth(numberOfMonths: number): Observable<EventsPerMonth[]> {
    return this.http.getRequest<EventsPerMonth[]>(`${this.baseUrl}/EventsByMonth?numberOfMonths=${numberOfMonths}`);
  }
} 