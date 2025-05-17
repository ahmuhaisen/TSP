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
    {
        path: "societies",
        loadComponent: () => import("./pages/societies/societies.component").then(m => m.SocietiesComponent),
        loadChildren: () => import("./pages/societies/societies.routes").then(m => m.routes),
    },
]