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
        path: 'societies',
        loadComponent: () => import('./pages/societies/societies.component').then(m => m.SocietiesComponent),
        loadChildren: () => import('./pages/societies/societies.routes').then(m => m.routes),
        data: { breadcrumb: 'Societies' }
    },
    {
        path: '**',
        loadComponent: () => import('../../components/not-found.component').then(m => m.NotFoundComponent),
        data: { breadcrumb: { skip: true } }
    }
];