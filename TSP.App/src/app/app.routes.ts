import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: 'admin-area',
        loadComponent: () => import('./areas/system-admin-area/system-admin-area.component').then(m => m.SystemAdminAreaComponent),
        loadChildren: () => import('./areas/system-admin-area/system-admin-area.routes').then(m => m.routes),
        data: { breadcrumb: 'Admin Area' }
    },
    {
        path: 'student-area',
        loadComponent: () => import('./areas/student-area/student-area.component').then(m => m.StudentAreaComponent),
        loadChildren: () => import('./areas/student-area/student-area.routes').then(m => m.routes),
        data: { breadcrumb: 'Student Area' }
    },
    {
        path: 'authentication',
        loadComponent: () => import('./areas/authentication/authentication.component').then(m => m.AuthenticationComponent),
        loadChildren: () => import('./areas/authentication/authentication.routes').then(m => m.routes)
    },
    {
        path: 'public-forms',
        loadComponent: () => import('./areas/public-forms/public-forms.component').then(m => m.PublicFormsComponent),
        loadChildren: () => import('./areas/public-forms/public-forms.routes').then(m => m.routes),
    },
    {
        path: 'coming-soon',
        loadComponent: () => import('./components/coming-soon.component').then(m => m.ComingSoonComponent)
    },
    {
        path: '',
        redirectTo: 'admin-area',
        pathMatch: 'full'
    },
    {
        path: '**',
        loadComponent: () => import('./components/not-found.component').then(m => m.NotFoundComponent),
        data: { breadcrumb: { skip: true } }
    }
];
