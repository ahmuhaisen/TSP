import { inject, Injectable } from '@angular/core';

import { Society } from '../../../areas/system-admin-area/api-interfaces/society.types';
import { DbService } from '../db.service';

@Injectable({
  providedIn: 'root'
})
export class SocietiesService {

  model = 'societies';

  db = inject(DbService);

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
