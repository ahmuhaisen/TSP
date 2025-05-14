import { HttpClient } from '@angular/common/http';
import { Injectable, Signal, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { environment } from '../../../../environments/environment';
import { DbService } from '../../../common/services/db.service';
import { StudentEvent } from '../api-interfaces/event.types';
import { HomeStatistics } from '../api-interfaces/statistics.types';


@Injectable({
  providedIn: 'root'
})
export class HomeService {

  private db = inject(DbService);
  model = "StudentArea/home";

  getRecentEvents() {
    return this.db.getRequest<StudentEvent[]>(`${this.model}/recentEvents`);
  }

  getHomeStatistics() {
    return this.db.getRequest<HomeStatistics>(`${this.model}/homeStatistics`);
  }
}
