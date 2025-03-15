import { inject, Injectable } from "@angular/core";
import { DbService } from "./db.service";

@Injectable({
    providedIn: 'root'
})
export class ProfilesService {

    model = 'profiles';

    db = inject(DbService);


    find(id: string, userType: string) {
        return this.db.getRequest<UserProfile>(`${this.model}/${id}?userType=${userType}`);
    }

    update(id: string, userType: string, profile: Partial<UserProfile>) {
        return this.db.putRequest<UserProfile, Partial<UserProfile>>(`${this.model}/${id}?userType=${userType}`, profile);
    }

}

export interface UserProfile
{
    id: string;
    userType: string;
    number: string;
    fullName: string;
    email: string;
    profileImageId?: string;
    department?: string;
    school?: string;
    memberships?: MembershipBasicDetails[];
}

export interface MembershipBasicDetails{
    section: string;
    societyName: string;
    societyLogoId: string;
    joinDate: Date;
}