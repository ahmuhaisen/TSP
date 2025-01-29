import { Routes } from "@angular/router";

export const routes: Routes = [
    {
        path: '',
        redirectTo: '../home',
        pathMatch: 'full'
    },
    {
        path: ':id',
        loadComponent: () => import('./profile/profile.component').then(m => m.ProfileComponent),
        data: {
            breadcrumb: 'Profile'
        }
    }
];