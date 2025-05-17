import { Routes } from "@angular/router";

export const routes: Routes = [
    {
        path: '',
        redirectTo: 'login',
        pathMatch: 'full'
    },
    {
        path: 'login',
        loadComponent: () => import('./login/login.component').then(m => m.LoginComponent)
    },
    {
        path: 'register',
        loadComponent: () => import('./register/register.component').then(m => m.RegisterComponent)
    },
    {
        path: 'super-admin/login',
        loadComponent: () => import('./super-admin/login/login.component').then(m => m.LoginComponent)
    },
    {
        path: '**',
        loadComponent: () => import('./login/login.component').then(m => m.LoginComponent)
    }
];