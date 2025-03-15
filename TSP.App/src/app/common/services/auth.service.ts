import { inject, Injectable, signal } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { SecureLocalStorageService } from './secure-local-storage.service';
import { DbService } from './db.service';
import { NzMessageService } from 'ng-zorro-antd/message';
import { Router } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class AuthService {

    currentUser = signal<User | null>(null);

    model = 'Authentication';

    db = inject(DbService);
    router = inject(Router);
    jwtHelper = inject(JwtHelperService);
    messageService = inject(NzMessageService);
    localStorageService = inject(SecureLocalStorageService);

    isAuthenticated(): boolean {
        const token = this.localStorageService.getItem('token');
        return !this.jwtHelper.isTokenExpired(token);
    }

    isUserIsTheCurrentUser(id: string): boolean {
        return this.currentUser()!.id === id;
    }

    login(request: LoginRequest, userType: UserType) {
        if(userType === 'Guest') return;

        this.db.postRequest<LoginResponse, LoginRequest>(`${this.model}/${userType}/Login`, request).subscribe({
            next: (res) => {
                this.localStorageService.setItem('token', res.token);
                this.setCurrentUser(res);
                this.navigateToHome(res.userType);
            }
        });
    }

    registerStudent(request: StudentRegisterRequest) {
        this.db.postRequest(`${this.model}/Student/Register`, request).subscribe({
            next: () => {
                this.messageService.success('Registration successful!');
                this.navigateToLogin();
            }
        });
    }

    registerFaculty(request: FacultyRegisterRequest) {
        this.db.postRequest(`${this.model}/FacultyMember/Register`, request).subscribe({
            next: () => {
                this.messageService.success('Registration successful!');
                this.navigateToLogin();
            }
        });
    }

    logout(){
        this.localStorageService.removeItem('token');
        this.currentUser.set(null);
        this.router.navigate(['authentication']);
    }

    setCurrentUser(userInfo: { id: string, fullName: string, email: string, profileImageId?: string }) {
        this.currentUser.set({
            id: userInfo.id,
            name: userInfo.fullName,
            email: userInfo.email,
            profileImageId: userInfo.profileImageId
        });
    }

    isCurrentUserHasRole(role: string) {
        const token = this.localStorageService.getItem('token');
        const tokenPayload = this.jwtHelper.decodeToken(token);
        return tokenPayload['rle'] === role;
    }

    private navigateToHome(userType: string) {
        if(userType === 'FacultyMember') {
            this.router.navigate(['admin-area']);
        }else {
            this.router.navigate(['student-area']);
        }
    }

    private navigateToLogin() {
        this.router.navigate(['authentication/login']);
    }
}

export interface LoginRequest {
    email: string;
    password: string;
}

export interface LoginResponse {
    token: string;
    userType: string;
    id: string;
    fullName: string;
    email: string;
    profileImageId: string;
}

export interface User {
    id: string;
    name: string;
    email: string;
    profileImageId?: string;
}

export type UserType = 'FacultyMember' | 'Student' | 'Guest';

export interface BaseRegisterRequest {
    email: string;
    password: string;
    firstName: string;
    lastName: string;
    gender: string;
    departmentId: number;
  }
  
  export interface FacultyRegisterRequest extends BaseRegisterRequest {
    employeeNumber: string;
    rankId: number;
  }
  
  export interface StudentRegisterRequest extends BaseRegisterRequest {
    universityNumber: string;
  }
  