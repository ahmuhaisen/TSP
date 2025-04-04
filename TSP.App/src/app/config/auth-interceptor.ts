import { HttpInterceptorFn } from "@angular/common/http";
import { inject } from "@angular/core";
import { SecureLocalStorageService } from "../common/services/secure-local-storage.service";

export const authInterceptor: HttpInterceptorFn = (req, next) => {
    const localStorageService = inject(SecureLocalStorageService);

    const token = localStorageService.getItem("token");

    if (token) {
        req = req.clone({
            headers: req.headers.set("Authorization", "Bearer " + token)
        }
        );
    }

    return next(req);
};