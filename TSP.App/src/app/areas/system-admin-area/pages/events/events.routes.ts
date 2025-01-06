import { Routes } from "@angular/router";

export const routes: Routes = [
    {
        path: '',
        loadComponent: () => import('./events.component').then(m => m.EventsComponent)
    },
    {
        path: ':id',
        loadComponent: () => import('./event-details/event-details.component').then(m => m.EventDetailsComponent)
    },
    {
        path: '**',
        loadComponent: () => import('./events.component').then(m => m.EventsComponent)
    }
];