import { Routes } from "@angular/router";

export const routes: Routes = [
    {
        path: '',
        loadComponent: () => import('./events-list/events-list.component').then(m => m.EventsListComponent)
    },

] ;