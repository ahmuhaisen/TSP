import { inject, Injectable } from '@angular/core';
import {
    Router,
    CanActivate,
    ActivatedRouteSnapshot
} from '@angular/router';

import { AuthService } from './auth.service';
import { jwtDecode, JwtPayload } from 'jwt-decode';
import { SecureLocalStorageService } from './secure-local-storage.service';

@Injectable({
    providedIn: 'root'
})
export class UserTypeGuardService implements CanActivate {

    router = inject(Router);
    authService = inject(AuthService);
    localStorageService = inject(SecureLocalStorageService);

    canActivate(route: ActivatedRouteSnapshot): boolean {

        const expectedUserRole = route.data['expectedUserRole'];
        const token = this.localStorageService.getItem('token');

        if (!token) {
            this.navigateToLogin();
            return false;
        }

        const tokenPayload = jwtDecode(token) as CustomTokenPayload;

        if (
            !this.authService.isAuthenticated()
            || tokenPayload.rle !== expectedUserRole
        ) {
            this.navigateToLogin();
            return false;
        }

        this.authService.setCurrentUser({
            id: tokenPayload.uid,
            fullName: tokenPayload.name,
            email: tokenPayload.email,
            profileImageId: tokenPayload.pid
        })
        return true;
    }

    navigateToLogin(): void {
        this.router.navigate(['/authentication/login']);
    }
}

export interface CustomTokenPayload extends JwtPayload {
    name: string;
    email: string;
    utp: string;
    rle: string;
    uid: string;
    pid?: string;
}