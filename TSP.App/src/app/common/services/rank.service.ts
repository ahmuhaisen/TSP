import { inject, Injectable } from "@angular/core";
import { DbService } from "./db.service";
import { Rank } from "../types/system-tables.types";

@Injectable({ providedIn: 'root' })
export class RankService {
    model = 'ranks';

    db = inject(DbService);

    all() {
        return this.db.getRequest<Rank[]>(this.model);
    }

    private getUrl() {
        return `${this.model}`;
    }

    private getUrlWithId(id: string) {
        return `${this.getUrl()}/${id}`;
    }
}