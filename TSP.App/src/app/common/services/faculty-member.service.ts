import { inject, Injectable } from "@angular/core";
import { DbService } from "./db.service";

@Injectable({ providedIn: 'root' })
export class FacultyMembersService {
    model = 'facultyMember';

    db = inject(DbService);

    all() {
        return this.db.getRequest<facultyMemberBasicDetails[]>("AdminArea/FacultyMember");
    }

    private getUrl() {
        return `${this.model}`;
    }

    private getUrlWithId(id: string) {
        return `${this.getUrl()}/${id}`;
    }
}

export interface facultyMemberBasicDetails {
    id: string;
    fullName: string;
    logoId: string;
}