import { inject, Injectable } from '@angular/core';

import { PostSociety, Society, SocietyBasicDetails, SocietyMember, SocietyWithAdvisor } from '../api-interfaces/society.types';
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
    return this.db.getRequest<SocietyBasicDetails[]>(`AdminArea/FacultyMember/Societies/Advised`);
  }

  otherSocieties() {
    return this.db.getRequest<SocietyBasicDetails[]>(`AdminArea/FacultyMember/Societies/Other`);
  }

  societyMembers(id: string, isCommittee: boolean) {
    return this.db.getRequest<SocietyMember[]>(`AdminArea/Societies/${id}/Members?isCommittee=${isCommittee}`);
  }

  removeCommitteeMember(societyId: string, memberId: string)
  {
    return this.db.deleteRequest(`AdminArea/Societies/${societyId}/Members/${memberId}/Committee`);
  }

  all() {
    return this.db.getRequest<Society[]>(this.model);
  }

  find(id: string) {
    return this.db.getRequest<SocietyWithAdvisor>(this.getUrlWithId(id));
  }

  create(society: PostSociety) {
    return this.db.postRequest<PostSociety, PostSociety>(this.getUrl(), society);
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

