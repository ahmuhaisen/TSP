import { Routes } from "@angular/router";

export const routes: Routes = [
    {
        path: '',
        loadComponent: () => import('./events-list/events-list.component').then(m => m.EventsListComponent)
    },
    {
        path: ':id',
        loadComponent: () => import('./event-details/event-details.component').then(m => m.EventDetailsComponent),
        data: { breadcrumb: { alias: 'eventName' } }
    }
];