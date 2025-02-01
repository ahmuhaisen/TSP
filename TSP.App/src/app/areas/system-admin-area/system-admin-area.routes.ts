import { Routes } from '@angular/router';

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
        path: 'events',
        loadComponent: () => import('./pages/events/events.component').then(m => m.EventsComponent),
        loadChildren: () => import('./pages/events/events.routes').then(m => m.routes),
        data: { breadcrumb: 'Events' }
    },
    {
        path: 'statistics',
        loadComponent: () => import('./pages/statistics/statistics.component').then(m => m.StatisticsComponent),
        data: { breadcrumb: 'Statistics & Reports' }
    },
    {
        path: 'users',
        loadComponent: () => import('./pages/users/users.component').then(m => m.UsersComponent),
        loadChildren: () => import('./pages/users/users.routes').then(m => m.routes),
        data: { breadcrumb: 'Users' }
    },
    {
        path: '**',
        loadComponent: () => import('../../components/not-found.component').then(m => m.NotFoundComponent),
        data: { breadcrumb: { skip: true } }
    }
];