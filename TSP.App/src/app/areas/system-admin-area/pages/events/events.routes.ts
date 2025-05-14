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
    },
    {
        path: ':id/feedback-summary',
        loadComponent: () => import('./event-details/event-feedback-summary/event-feedback-summary.component').then(m => m.EventFeedbackSummaryComponent),
        data: { breadcrumb: 'Feedback Summary' }
    },
    {
        path: '**',
        loadComponent: () => import('./events.component').then(m => m.EventsComponent)
    }
];