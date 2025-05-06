import { inject, Injectable } from '@angular/core';
import { DbService } from '../../../common/services/db.service';

@Injectable({
  providedIn: 'root'
})
export class AccountsService {

  dbService = inject(DbService);
  model = 'accounts';

  getAllPendingRequests() {
    return this.dbService.getRequest<PendingAccountRequest[]>(`${this.model}/pending`);
  }

  acceptRequest(id: string, userType: string) {
    return this.dbService.putRequest(`${this.model}/approve/${id}?userType=${userType}`, {});
  }

  rejectRequest(id: string, userType: string) {
    return this.dbService.putRequest(`${this.model}/reject/${id}?userType=${userType}`, { });
  }
}

export interface PendingAccountRequest {
  id: string;
  fullName: string;
  email: string;
  userType: string;
  departmentName: string;
  registeredAt: Date;
  rank?: string;
}