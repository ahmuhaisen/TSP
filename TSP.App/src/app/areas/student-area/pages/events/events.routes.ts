import { Routes } from "@angular/router";

export const routes: Routes = [
    {
        path: '',
        loadComponent: () => import('./events-list/events-list.component').then(m => m.EventsListComponent)
    },
    {
        path: ':id',
        loadComponent: () => import('./gen-event-request-details/gen-event-request-details.component').then(m => m.GenEventRequestDetailsComponent),
        data: { breadcrumb: { alias: 'eventName' } }
    }
] ;