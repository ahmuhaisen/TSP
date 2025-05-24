import { Injectable } from "@angular/core"
import { inject } from "@angular/core"
import { DbService } from "./db.service"
import { AuthService } from "./auth.service"


export interface SearchBasicDTO {
    id: string,
    name: string,
    description: string,
    logoId: string,
    isFacultyMember: boolean

}

@Injectable({ providedIn: 'root' })
export class SearchService {
    private db = inject(DbService);
    authService = inject(AuthService);
    private model = `Shared/Search`;
    getSearchResults(searchType: string, searchTerm: string) {
        return this.db.getRequest<SearchBasicDTO[]>(`${this.model}/${searchType}?searchTerm=${searchTerm}`);
    }


}