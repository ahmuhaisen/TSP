import { Routes } from "@angular/router";

export const routes: Routes = [
    {
        path: '',
        loadComponent: () => import('./societies-list/societies-list.component').then(m => m.SocietiesListComponent)
    },
    {
        path: 'create',
        loadComponent: () => import('./create-society/create-society.component').then(m => m.CreateSocietyComponent),
        data: { breadcrumb: 'Create' }
    },
    {
        path: ':id',
        loadComponent: () => import('./society-details/society-details.component').then(m => m.SocietyDetailsComponent),
        data: { breadcrumb: { alias: 'societyName' } }
    },
    {
        path: ':id/manage',
        loadComponent: () => import('./society-details/society-details.component').then(m => m.SocietyDetailsComponent),
        data: { breadcrumb: { alias: 'societyName' } }
    },
    {
        path: '**',
        redirectTo: '',
    }
]