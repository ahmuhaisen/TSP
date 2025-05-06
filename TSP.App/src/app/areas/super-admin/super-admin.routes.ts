import { Routes } from "@angular/router";


export const superAdminRoutes: Routes = [
    {
        path: "",
        redirectTo: "home",
        pathMatch: "full",
    },
    {
        path: "home",
        loadComponent: () => import("./pages/home/home.component").then(m => m.HomeComponent),
    },
    {
        path: "users",
        loadComponent: () => import("./pages/users/users.component").then(m => m.UsersComponent),
    },
]