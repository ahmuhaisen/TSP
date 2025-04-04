import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { Observable } from "rxjs";
import { DbService } from "../../../common/services/db.service";
import { SocietyWithAdvisor } from "../../system-admin-area/api-interfaces/society.types";

export interface Society {
    id: string;
    name: string;
    description: string;
    logoId: string;
    creationDate: string;
    isCommittee: boolean;
}

export interface MemberAssociatedSociety extends Society {
    position: string;
}

export interface SocietyJoinRequest {
    id: string;
    societyId: string;
    societyName: string;
    societyLogo: string;
    section: string;
    status: 'pending' | 'approved' | 'rejected';
    submittedAt: string;
    motivation: string;
}

export interface JoinSocietyRequest {
    societyId: string;
    section: string;
    motivation: string;
}

@Injectable({
    providedIn: 'root'
})
export class SocietiesService {
    private db = inject(DbService);
    private baseUrl = `StudentArea/Students`;

    getBelongingSocieties(): Observable<MemberAssociatedSociety[]> {
        return this.db.getRequest<MemberAssociatedSociety[]>(`${this.baseUrl}/AllSocieties`);
    }

    getOtherSocieties(): Observable<Society[]> {
        return this.db.getRequest<Society[]>(`${this.baseUrl}/OtherSocieties`);
    }

    getJoinRequests(): Observable<SocietyJoinRequest[]> {
        return this.db.getRequest<SocietyJoinRequest[]>(`${this.baseUrl}/MembershipRequests`);
    }

    leaveSociety(societyId: string): Observable<void> {
        return this.db.deleteRequest<void>(`$studentArea/societies/${societyId}/Members`);
    }

    joinSociety(request: JoinSocietyRequest): Observable<void> {
        return this.db.postRequest<void, JoinSocietyRequest>(`${this.baseUrl}/JoinSociety`, request);
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