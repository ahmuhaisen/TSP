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
export class RoleGuardService implements CanActivate {

    router = inject(Router);
    authService = inject(AuthService);
    localStorageService = inject(SecureLocalStorageService);

    canActivate(route: ActivatedRouteSnapshot): boolean {

        const expectedUserType = route.data['expectedUserType'];
        const token = this.localStorageService.getItem('token');

        if (!token) {
            this.navigateToLogin();
            return false;
        }

        const tokenPayload = jwtDecode(token) as CustomTokenPayload;

        if (
            !this.authService.isAuthenticated()
            || tokenPayload.utp !== expectedUserType
        ) {
            console.log('unauthenticated');
            this.navigateToLogin();
            return false;
        }

        return true;
    }

    navigateToLogin(): void {
        this.router.navigate(['/authentication/login']);
    }
}

interface CustomTokenPayload extends JwtPayload {
    utp: string;
}