import { inject, Injectable } from "@angular/core";
import { DbService } from "./db.service";
import { map } from "rxjs/operators";
import { Observable } from "rxjs";

// Define the Result interface to match the backend response structure
interface Result<T> {
    isSuccess: boolean;
    value: T;
    error?: {
        code: string;
        message: string;
    };
}

@Injectable({
    providedIn: 'root'
})
export class ProfilesService {

    model = 'profiles';

    db = inject(DbService);


    find(id: string, userType: string) {
        return this.db.getRequest<UserProfile>(`${this.model}/${id}?userType=${userType}`);
    }

    hasProfileImage() {
        return this.db.getRequest<boolean>(`${this.model}/has-profile-image`);
    }

    update(id: string, userType: 'Student' | 'Faculty', profile: Partial<UserProfile>) {
        console.log('Updating profile', id, userType, profile);
        return this.db.putRequest<UserProfile, Partial<UserProfile>>(`${this.model}?userType=${userType}`, profile);
    }
    updatePassword(userId: string, password: string) {
        return this.db.putRequest<boolean, any>(`${this.model}/reset/${userId}?password=${password}`, "")
    }

}

export interface UserProfile {
    id: string;
    userType: 'Student' | 'Faculty';
    number: string;
    fullName: string;
    email: string;
    profileImageId?: string;
    department?: string;
    school?: string;
    memberships?: MembershipBasicDetails[];
    firstName: string;
    lastName: string;


}


export interface MembershipBasicDetails {
    section: string;
    societyName: string;
    societyLogoId: string;
    joinDate: Date;
}