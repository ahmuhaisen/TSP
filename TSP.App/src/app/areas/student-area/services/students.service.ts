import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { DbService } from "../../../common/services/db.service";
import { MemberAssociatedSociety, SocietyJoinRequest, Society } from "../api-interfaces/society.types";
import { AuthService } from "../../../common/services/auth.service";

@Injectable({
    providedIn: 'root'
})
export class StudentsService {
    private db = inject(DbService);
    private baseUrl = `StudentArea/Students`;
    authService = inject(AuthService);
    userId = this.authService.currentUser()?.id;
    getBelongingSocieties(): Observable<MemberAssociatedSociety[]> {
        return this.db.getRequest<MemberAssociatedSociety[]>(`${this.baseUrl}/AllSocieties`);
    }

    getOtherSocieties(): Observable<Society[]> {
        return this.db.getRequest<Society[]>(`${this.baseUrl}/OtherSocieties`);
    }

    getJoinRequests(): Observable<SocietyJoinRequest[]> {
        return this.db.getRequest<SocietyJoinRequest[]>(`${this.baseUrl}/MembershipRequests`);
    }
    getCommitteeSocieties(): Observable<MemberAssociatedSociety[]> {
        return this.db.getRequest<MemberAssociatedSociety[]>(`${this.baseUrl}/AllCommitteeSocieties`)
    }

    isStudentACommitteeMember(studentId: string): Observable<boolean> {
        return this.db.getRequest<boolean>(`${this.baseUrl}/${studentId}/isCommittee`);
    }
}