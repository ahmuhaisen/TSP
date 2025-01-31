import { Routes } from "@angular/router";

export const routes: Routes = [
    {
        path: '',
        redirectTo: 'home',
        pathMatch: 'full'
    },
    {
        path: 'home',
        loadComponent: () => import('./pages/home/home.component').then(m => m.HomeComponent),
        data: { breadcrumb: 'Home' }
    },
    {
        path: '**',
        loadComponent: () => import('../../components/not-found.component').then(m => m.NotFoundComponent),
        data: { breadcrumb: { skip: true } }
    }
];