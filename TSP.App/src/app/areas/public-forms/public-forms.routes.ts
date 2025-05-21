import { Routes } from "@angular/router";

export const routes: Routes = [
    {
        path: '',
        redirectTo: 'coming-soon',
        pathMatch: 'full'
    },
    {
        path: 'event-registration/:eventId',
        loadComponent: () => import('./attendence/attendence.component').then(m => m.AttendenceComponent)
    },
    {
        path: 'event-feedback/:eventId',
        loadComponent: () => import('./event-feedback/event-feedback.component').then(m => m.EventFeedbackComponent)
    }
]