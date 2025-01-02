import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: '',
        loadComponent: () => import('./system-admin-area.component').then(m => m.SystemAdminAreaComponent)
    }
];