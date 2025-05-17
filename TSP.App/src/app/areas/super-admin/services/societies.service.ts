import { inject, Injectable } from "@angular/core";
import { DbService } from "../../../common/services/db.service";
import { AuthService } from "../../../common/services/auth.service";
import { PostSociety, Society, SocietyWithAdvisor } from "../../system-admin-area/api-interfaces/society.types";



@Injectable({
  providedIn: 'root'
})
export class SocietiesService {

  model = 'SuperAdmin/Societies';

  db = inject(DbService);
  authService = inject(AuthService);
  userId = this.authService.currentUser()?.id;

  all() {
    return this.db.getRequest<SocietyWithAdvisor[]>(this.model);
  }

  find(id: string) {
    return this.db.getRequest<SocietyWithAdvisor>(this.getUrlWithId(id));
  }

  create(society: PostSociety) {
    return this.db.postRequest<PostSociety, PostSociety>(this.getUrl(), society);
  }

  update(id: string, society: { 
    id: string;
    name: string;
    description: string;
    logoBase64: string;
    themeColor?: string;
    creationDate?: string;
    advisorId?: string;
  }) {
    return this.db.putRequest<any, any>(this.getUrlWithId(id), society);
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
