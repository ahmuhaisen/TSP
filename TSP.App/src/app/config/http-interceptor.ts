import { throwError } from 'rxjs';
import { inject } from '@angular/core';
import { retry, catchError } from 'rxjs/operators';
import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';

import { NzMessageService } from 'ng-zorro-antd/message';
import { ApiError } from '../common/types/api.types';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  let messageService = inject(NzMessageService);

  return next(req).pipe(
    retry(0),
    catchError((error: HttpErrorResponse) => {
      if (
        error instanceof HttpErrorResponse &&
        error.error &&
        error.error.responseData
      ) {
        const apiErrors = error.error.responseData.errors;

        apiErrors.forEach((err: ApiError) => {
          messageService.error(err.message);
        });

      } else {
        if(error.status === 401){
        }
        else {
          messageService.error('An unknown error occurred');
        }
      }

      return throwError(error);
    })
  );
};
