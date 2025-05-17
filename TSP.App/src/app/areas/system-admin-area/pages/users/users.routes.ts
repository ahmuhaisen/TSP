import { Routes } from "@angular/router";

export const routes: Routes = [
    {
        path: '',
        redirectTo: '../home',
        pathMatch: 'full'
    },
    {
        path: ':id',
        loadComponent: () => import('../../../../components/gen-profile/gen.profile.component').then(m => m.GenProfileComponent),
        data: {
            breadcrumb: 'Profile',
        }
    }
];