import { inject, Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { AuthService } from './auth.service';


@Injectable({
  providedIn: 'root'
})
export class AuthGuardService implements CanActivate {

  router = inject(Router);
  authService = inject(AuthService);

  canActivate(): boolean {
    if (this.authService.isAuthenticated()) 
      return true;

    this.router.navigate(['authentication/login']);
    return false;
  }
}