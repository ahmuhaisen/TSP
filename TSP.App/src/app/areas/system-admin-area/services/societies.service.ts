import { inject, Injectable } from '@angular/core';

import { Society, SocietyBasicDetails } from '../api-interfaces/society.types';
import { DbService } from '../../../common/services/db.service';
import { AuthService } from '../../../common/services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class SocietiesService {

  model = 'AdminArea/Societies';

  db = inject(DbService);
  authService = inject(AuthService);
  userId = this.authService.currentUser()?.id;


  advisorSocieties() {
    return this.db.getRequest<SocietyBasicDetails[]>(`AdminArea/Advisor/AdvisorSocieties?advisorIds=${this.userId}`);
  }

  otherSocieties() {
    return this.db.getRequest<SocietyBasicDetails[]>(`AdminArea/Advisor/OtherSocieties?advisorIds=${this.userId}`);
  }

  all() {
    return this.db.getRequest<Society[]>(this.model);
  }

  find(id: string) {
    return this.db.getRequest<Society>(this.getUrlWithId(id));
  }

  create(society: Society) {
    return this.db.postRequest<Society, Society>(this.getUrl(), society);
  }

  update(id: string, society: Society) {
    return this.db.putRequest<Society, Society>(this.getUrlWithId(id), society);
  }

  delete(id: string) {
    return this.db.deleteRequest(this.getUrlWithId(id));
  }

  private getUrl() {
    return `${this.model}`;
  }

  private getUrlWithId(id: string) {
    return `${this.getUrl()}/${id}`;
  }
}

