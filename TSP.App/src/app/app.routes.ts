import { Routes } from '@angular/router';
import { ComingSoonComponent } from './pages/coming-soon/coming-soon.component';

export const routes: Routes = [
    {
        path: 'coming-soon',
        loadComponent: () => import('./pages/coming-soon/coming-soon.component').then(m => m.ComingSoonComponent)
    },
    {
        path: '',
        redirectTo: 'coming-soon',
        pathMatch: 'full'
    },
    {
        path: '**',
        redirectTo: 'coming-soon'
    }
];
