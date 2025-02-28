import { inject, Injectable } from "@angular/core";
import { DbService } from "./db.service";
import { Rank, SchoolWithDepartmentsBasicDetails } from "../types/system-tables.types";

@Injectable({ providedIn: 'root' })
export class SchoolService {
    model = 'schools';

    db = inject(DbService);

    allSchoolsWithDepartments() {
        return this.db.getRequest<SchoolWithDepartmentsBasicDetails[]>(this.model);
    }

    private getUrl() {
        return `${this.model}`;
    }

    private getUrlWithId(id: string) {
        return `${this.getUrl()}/${id}`;
    }
}



