import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { MemberService } from '../Services/member.service';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  constructor(private MemberService: MemberService) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    request = request.clone({
      setHeaders: {
        Authorization: `Bearer ${this.MemberService.getHttpToken()}`,
      },
    });

    return next.handle(request);
  }
}
