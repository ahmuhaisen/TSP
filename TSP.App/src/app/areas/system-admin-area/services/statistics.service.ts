import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';

export interface SocietyMembersCount {
  societyName: string;
  membersCount: number;
}

export interface EventAttendanceCount {
  eventName: string;
  attendanceCount: number;
}

export interface SocietyData {
  id: string;
  name: string;
  description: string;
  logoUrl: string;
  NoOfMembers: number;
  NoOfEvents: number;
}

export interface EventsPerMonth {
  month: string;
  eventCount: number;
}

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {
  private baseUrl = `${environment.apiURL}AdminArea/Statistics`;

  constructor(private http: HttpClient) { }

  getTopSocietiesByMembers(numberOfSocieties: number): Observable<SocietyMembersCount[]> {
    return this.http.get<SocietyMembersCount[]>(`${this.baseUrl}/TopSocietiesByMembers`, {
      params: { numberOfSocities: numberOfSocieties }
    });
  }

  getTopEventsByAttendance(numberOfEvents: number): Observable<EventAttendanceCount[]> {
    return this.http.get<EventAttendanceCount[]>(`${this.baseUrl}/TopEventsByAttendence`, {
      params: { numberOfEvents: numberOfEvents }
    });
  }

  getTopSocieties(numberOfSocieties: number): Observable<SocietyData[]> {
    return this.http.get<SocietyData[]>(`${this.baseUrl}/TopSocities`, {
      params: { numberOfSocities: numberOfSocieties }
    });
  }

  getEventsByMonth(numberOfMonths: number): Observable<EventsPerMonth[]> {
    return this.http.get<EventsPerMonth[]>(`${this.baseUrl}/EventsByMonth`, {
      params: { numberOfMonths: numberOfMonths }
    });
  }
} 