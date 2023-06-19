import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable, catchError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private router: Router, private toaster: ToastrService) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error) {
          switch (error.status) {
            case 400:
              var errorsmodelstate = [];
              var e = error.error.errors;
              if (e) {
                for (const key in e) {
                  if (e[key]) {
                    errorsmodelstate.push(e[key]);
                    // console.log(e[key][0]);
                    // this.toaster.error(error.error, e[key][0]);
                  }
                }
                errorsmodelstate = errorsmodelstate.flat();
                for (let i = 0; i < errorsmodelstate.length; i++) {
                  this.toaster.error(errorsmodelstate[i]);
                }
              } else {
                this.toaster.error(error.error, error.status.toString());
              }
              break;
            case 401:
              this.toaster.error('unauthorized', error.status.toString());
              break;
            case 404:
              this.router.navigateByUrl('/not-found');

              this.toaster.error('not found', error.status.toString());
              break;
            case 500:
              this.router.navigateByUrl('/server-error');

              this.toaster.error(
                'internal server error',
                error.status.toString()
              );
              break;
            default:
              this.toaster.error(error.error, error.status.toString());
              break;
          }
        }
        throw error;
      })
    );
  }
}
