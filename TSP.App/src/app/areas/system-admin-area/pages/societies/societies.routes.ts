import { Routes } from "@angular/router";

export const routes: Routes = [
    {
        path: '',
        loadComponent: () => import('./societies-list/societies-list.component').then(m => m.SocietiesListComponent)
    },
    {
        path: 'create',
        loadComponent: () => import('./create-society/create-society.component').then(m => m.CreateSocietyComponent)
    },
    {
        path: ':id/manage',
        loadComponent: () => import('./manage-society/manage-society.component').then(m => m.ManageSocietyComponent)
    },
    {
        path: '**',
        redirectTo: ''
    }
]