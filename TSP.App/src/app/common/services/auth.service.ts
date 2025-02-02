import { inject, Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { SecureLocalStorageService } from './secure-local-storage.service';

@Injectable({ providedIn: 'root' })
export class AuthService {
    localStorageService = inject(SecureLocalStorageService);
    jwtHelper = inject(JwtHelperService);

    public isAuthenticated(): boolean {
        this.localStorageService.setItem('token', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiIxNjdjOGU3Yy02NTRhLTRkNzctYTNmYy0xM2ZlMGNjZWJhYjMiLCJlbWFpbCI6ImZAZ21haWwuY29tIiwibmFtZSI6InN0cmluZyBzdHJpbmciLCJ1aWQiOiI4ZWM1M2MyZC04YWJlLTRjNTUtZTgxMC0wOGRkM2ZiOGQ2MGEiLCJVc2VyVHlwZSI6IkZhY3VsdHlNZW1iZXIiLCJleHAiOjE3MzgzNDEzNTEsImlzcyI6IlRoZVNvY2lldGllc1BvcnRhbCIsImF1ZCI6IlRoZVNvY2lldGllc1BvcnRhbCJ9.vcueRB_oEqlDrSttU8-4_Ax-UGRSrAQOdWg_MuZ8ebs');
        const token = this.localStorageService.getItem('token');
        return !this.jwtHelper.isTokenExpired(token);
    }
}