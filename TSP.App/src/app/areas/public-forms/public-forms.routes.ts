import { Routes } from "@angular/router";

export const routes: Routes = [
    {
        path: '',
        redirectTo: 'coming-soon',
        pathMatch: 'full'
    },
    {
        path: 'attendance/:eventId',
        loadComponent: () => import('./attendence/attendence.component').then(m => m.AttendenceComponent)
    }
]