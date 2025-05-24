import { Routes } from '@angular/router';
import { UserTypeGuardService as UserTypeGuard } from './common/services/user-type-guard.service';
import { NotFoundComponent } from './components/not-found.component';

export const routes: Routes = [
    {
        path: '',
        loadComponent: () => import('./areas/public/landing-page/landing-page.component').then(m => m.LandingPageComponent),
    },
    {
        path: 'privacy-policy',
        loadComponent: () => import('./areas/public/privacy-policy/privacy-policy.component').then(m => m.PrivacyPolicyComponent),
    },
    {
        path: 'terms-of-use',
        loadComponent: () => import('./areas/public/terms-of-use/terms-of-use.component').then(m => m.TermsOfUseComponent),
    },
    {
        path: 'help-center',
        loadComponent: () => import('./areas/public/help-center/help-center.component').then(m => m.HelpCenterComponent),
    },
    {
        path: 'admin-area',
        loadComponent: () => import('./areas/system-admin-area/system-admin-area.component').then(m => m.SystemAdminAreaComponent),
        loadChildren: () => import('./areas/system-admin-area/system-admin-area.routes').then(m => m.routes),
        data: { breadcrumb: 'Admin Area', expectedUserRole: 'Faculty' },
        canActivate: [UserTypeGuard]
    },
    {
        path: 'student-area',
        loadComponent: () => import('./areas/student-area/student-area.component').then(m => m.StudentAreaComponent),
        loadChildren: () => import('./areas/student-area/student-area.routes').then(m => m.routes),
        data: { breadcrumb: 'Student Area', expectedUserRole: 'Student' },
        canActivate: [UserTypeGuard],
    },
    {
        path: 'super-admin',
        loadComponent: () => import('./areas/super-admin/super-admin.component').then(m => m.SuperAdminComponent),
        loadChildren: () => import('./areas/super-admin/super-admin.routes').then(m => m.superAdminRoutes),
        data: { expectedUserRole: 'SuperAdmin' },
        canActivate: [ UserTypeGuard ]
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
        path: 'qr-viewer',
        loadComponent: () => import('./areas/public/qr-viewer/qr-viewer.component').then(m => m.QrViewerComponent),
    },
    {
        path: '',
        redirectTo: 'admin-area',
        pathMatch: 'full'
    },
    {
        path: 'forbidden',
        loadComponent: () => import('./components/access-denied.component').then(m => m.AccessDeniedComponent),
    },
    {
        path: '**',
        component: NotFoundComponent,
        data: { breadcrumb: { skip: true } }
    }
];
