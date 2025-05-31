import { inject, Injectable, signal, Signal } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { SecureLocalStorageService } from './secure-local-storage.service';
import { DbService } from './db.service';
import { NzMessageService } from 'ng-zorro-antd/message';
import { Router } from '@angular/router';
import { LoaderService } from './loader.service';
import { Observable, catchError, of, tap } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';
export interface User {
    id: string;
    name: string;
    email: string;
    profileImageId?: string;
    number?: string;
    userType?: string;
    departmentId?: number;
}

export interface CurrentUserDto {
    id: string;
    fullName: string;
    email: string;
    number: string;
    profileImageId?: string;
    userType: string;
    departmentId: number;
}

export type UserType = 'FacultyMember' | 'Student' | 'Guest' | 'SuperAdmin';

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
export interface ResetPasswordResponse {
    id: string;
    token: string;

}
@Injectable({ providedIn: 'root' })
export class AuthService {

    private _currentUser = signal<User | null>(null);
    private cookieSerivce = inject(CookieService);
    // Public read-only access to currentUser signal
    get currentUser(): Signal<User | null> {
        return this._currentUser.asReadonly();
    }

    model = 'Authentication';

    db = inject(DbService);
    router = inject(Router);
    jwtHelper = inject(JwtHelperService);
    messageService = inject(NzMessageService);
    localStorageService = inject(SecureLocalStorageService);
    loader = inject(LoaderService);

    isAuthenticated(): boolean {
        const token = this.localStorageService.getItem('token');
        return token && !this.jwtHelper.isTokenExpired(token);
    }

    isUserIsTheCurrentUser(id: string): boolean {
        return this._currentUser()?.id === id;
    }

    login(request: LoginRequest, userType: UserType) {
        this.loader.show();
        if (userType === 'Guest') return;

        this.db.postRequest<LoginResponse, LoginRequest>(`${this.model}/${userType}/Login`, request).subscribe({
            next: (res) => {
                this.localStorageService.setItem('token', res.token);



                this.setCurrentUser(res);
                this.cookieSerivce.set("profile_image", this.currentUser()?.profileImageId || "", {
                    path: '/'
                })

                this.navigateToHome(res.userType);
                this.loader.hide();

            },
            error: (err) => {
                this.loader.hide();
            }
        });
    }

    tryLogIn(): boolean {
        const token = this.localStorageService.getItem('token');
        if (token && !this.jwtHelper.isTokenExpired(token)) {
            try {
                // First set basic user info from token as a fallback
                const decodedToken = this.jwtHelper.decodeToken(token);
                console.log('Decoded token:', decodedToken);

                const basicUserInfo = {
                    id: decodedToken.sub || decodedToken.nameid || '',
                    fullName: decodedToken.name || decodedToken.fullName || 'User',
                    email: decodedToken.email || 'user@example.com',
                    profileImageId: decodedToken.profileImageId || ''
                };

                // Set current user from token data as initial data
                this.setCurrentUser(basicUserInfo);

                // Then fetch more detailed user info from API
                this.fetchCurrentUserInfo().subscribe({
                    next: (userInfo) => {
                        if (userInfo) {
                            console.log('Fetched detailed user info:', userInfo);
                            this.setCurrentUserDetailed(userInfo);
                        }
                    },
                    error: (error) => {
                        console.error('Error fetching current user info:', error);
                    }
                });

                return true;
            } catch (error) {
                console.error('Error decoding token:', error);
                return false;
            }
        }
        return false;
    }

    fetchCurrentUserInfo(): Observable<CurrentUserDto | null> {
        return this.db.getRequest<CurrentUserDto>('Profiles')
            .pipe(
                tap(response => console.log('Current user info response:', response)),
                catchError(error => {
                    console.error('Error fetching current user info:', error);
                    return of(null);
                })
            );
    }

    setCurrentUserDetailed(userInfo: CurrentUserDto) {
        this._currentUser.set({
            id: userInfo.id,
            name: userInfo.fullName,
            email: userInfo.email,
            profileImageId: userInfo.profileImageId,
            number: userInfo.number,
            userType: userInfo.userType,
            departmentId: userInfo.departmentId
        });
    }

    registerStudent(request: StudentRegisterRequest) {
        this.loader.show();
        this.db.postRequest(`${this.model}/Student/Register`, request).subscribe({
            next: () => {
                this.messageService.success('Registration successful!');
                this.navigateToLogin();
                this.loader.hide();
            },
            error: (err) => {
                this.loader.hide();
            }
        });
    }

    registerFaculty(request: FacultyRegisterRequest) {
        this.loader.show();
        this.db.postRequest(`${this.model}/FacultyMember/Register`, request).subscribe({
            next: () => {
                this.messageService.success('Registration successful!');
                this.navigateToLogin();
                this.loader.hide();
            }
            , error: (err) => {
                this.loader.hide();
            }
        });
    }

    logout() {
        this.localStorageService.removeItem('token');
        this.cookieSerivce.delete('username', '/');

        this._currentUser.set(null);
        this.router.navigate(['authentication']);
    }

    setCurrentUser(userInfo: { id: string, fullName: string, email: string, profileImageId?: string }) {
        this._currentUser.set({
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

    setCurrentUserProfileImageId(profileImageId: string) {
        const currentUser = this._currentUser();
        if (currentUser) currentUser.profileImageId = profileImageId;
    }

    // Helper methods to check user type more easily
    isFacultyMember(): boolean {
        return this._currentUser()?.userType?.toUpperCase() === 'FACULTY';
    }

    isStudent(): boolean {
        return this._currentUser()?.userType?.toUpperCase() === 'STUDENT';
    }

    private navigateToHome(userType: string) {
        if (userType === 'FacultyMember') {
            this.router.navigate(['admin-area']);
        } else if (userType === 'Student') {
            this.router.navigate(['student-area']);
        }
        else if (userType === 'SuperAdmin') {
            this.router.navigate(['super-admin']);
        }
    }

    private navigateToLogin() {
        this.router.navigate(['authentication/login']);
    }
    public getResetTokenAndId(email: string, url: string) {
        return this.db.getRequest<boolean>(`${this.model}/reset?email=${email}&url=${url}`)
    }
}
