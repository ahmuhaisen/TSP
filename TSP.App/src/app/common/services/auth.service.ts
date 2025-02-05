import { inject, Injectable, signal } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { SecureLocalStorageService } from './secure-local-storage.service';
import { DbService } from './db.service';
import { NzMessageService } from 'ng-zorro-antd/message';

@Injectable({ providedIn: 'root' })
export class AuthService {

    currentUser = signal<User | null>(null);

    model = 'Authentication';

    db = inject(DbService);
    jwtHelper = inject(JwtHelperService);
    messageService = inject(NzMessageService);
    localStorageService = inject(SecureLocalStorageService);

    isAuthenticated(): boolean {
        this.localStorageService.setItem('token', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiIxNjdjOGU3Yy02NTRhLTRkNzctYTNmYy0xM2ZlMGNjZWJhYjMiLCJlbWFpbCI6ImZAZ21haWwuY29tIiwibmFtZSI6InN0cmluZyBzdHJpbmciLCJ1aWQiOiI4ZWM1M2MyZC04YWJlLTRjNTUtZTgxMC0wOGRkM2ZiOGQ2MGEiLCJVc2VyVHlwZSI6IkZhY3VsdHlNZW1iZXIiLCJleHAiOjE3MzgzNDEzNTEsImlzcyI6IlRoZVNvY2lldGllc1BvcnRhbCIsImF1ZCI6IlRoZVNvY2lldGllc1BvcnRhbCJ9.vcueRB_oEqlDrSttU8-4_Ax-UGRSrAQOdWg_MuZ8ebs');
        const token = this.localStorageService.getItem('token');
        return !this.jwtHelper.isTokenExpired(token);
    }

    login(request: LoginRequest, userType: UserType) {
        if(userType === 'Guest') return;
        this.db.postRequest<{token: string}, LoginRequest>(`${this.model}/${userType}/Login`, request).subscribe({
            next: (res) => {
                console.log(res);
                this.localStorageService.setItem('token', res!.token);
                // Get user details
            },
            error: () => {
                this.messageService.error("Invalid email or password.");
            }
        });
    }
}

export interface LoginRequest {
    email: string;
    password: string;
}

export interface User {
    id: string;
    name: string;
    email: string;
    imageUrl: string;
}

export type UserType = 'FacultyMember' | 'Student' | 'Guest';