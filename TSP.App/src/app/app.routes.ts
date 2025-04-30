import { Routes } from '@angular/router';
import { UserTypeGuardService as UserTypeGuard } from './common/services/user-type-guard.service';

export const routes: Routes = [
    {
        path: '',
        loadComponent: () => import('./areas/public/landing-page/landing-page.component').then(m => m.LandingPageComponent),
    },
    {
        path: 'admin-area',
        loadComponent: () => import('./areas/system-admin-area/system-admin-area.component').then(m => m.SystemAdminAreaComponent),
        loadChildren: () => import('./areas/system-admin-area/system-admin-area.routes').then(m => m.routes),
        data: { breadcrumb: 'Admin Area', expectedUserRole: 'Faculty' },
        canActivate: [ UserTypeGuard ]
    },
    {
        path: 'student-area',
        loadComponent: () => import('./areas/student-area/student-area.component').then(m => m.StudentAreaComponent),
        loadChildren: () => import('./areas/student-area/student-area.routes').then(m => m.routes),
        data: { breadcrumb: 'Student Area', expectedUserRole: 'Student' },
        canActivate: [ UserTypeGuard ],
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
