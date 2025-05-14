import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { Observable } from "rxjs";
import { DbService } from "../../../common/services/db.service";
import { SocietyWithAdvisor } from "../../system-admin-area/api-interfaces/society.types";
import { MemberAssociatedSociety, SocietyJoinRequest, JoinSocietyRequest, Society } from "../api-interfaces/society.types";
import { AuthService } from "../../../common/services/auth.service";

import { MembershipRequestDTO, UpdateMembershipRequest } from "../api-interfaces/membership.types";
@Injectable({
    providedIn: 'root'
})
export class SocietiesService {
    private db = inject(DbService);
    private baseUrl = `StudentArea/societies`;
    authService = inject(AuthService);
    userId = this.authService.currentUser()?.id;

    leaveSociety(societyId: string): Observable<void> {
        return this.db.deleteRequest<any>(`${this.baseUrl}/${societyId}/Members`);
    }

    joinSociety(request: JoinSocietyRequest): Observable<void> {
        request.studentId = this.userId || "";
        return this.db.postRequest<void, JoinSocietyRequest>(`${this.baseUrl}/${request.societyId}/JoinRequest`, request);
    }
    getJoinRequests(societyId: string) {
        return this.db.getRequest<MembershipRequestDTO[]>(`${this.baseUrl}/${societyId}/Members/Requests`);
    }
    updateMembershipRequests(membershipRequest: UpdateMembershipRequest) {
        return this.db.putRequest<string, any>(`${this.baseUrl}/${membershipRequest.SocietyId}/Members/Requests/${membershipRequest.MembershipRequestId}/${membershipRequest.isAccepted}`, {});
    }

    find(id: string) {
        return this.db.getRequest<SocietyWithAdvisor>(this.getUrlWithId(id));
    }

    private getUrl() {
        return `${this.baseUrl}`;
    }

    private getUrlWithId(id: string) {
        return `${this.getUrl()}/${id}`;
    }
}

