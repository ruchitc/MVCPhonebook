import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { LocalStorageService } from '../services/helpers/local-storage.service';
import { LocalStorageKeys } from '../services/helpers/localstoragekeys';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private localStorageHelper: LocalStorageService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const token = this.localStorageHelper.getItem(LocalStorageKeys.TokenName);
    if(token) {
      const clonedReq = request.clone({
        headers: request.headers.set('Authorization', `Bearer ${token}`)
      });

      return next.handle(clonedReq);
    } else {
      return next.handle(request);
    }
  }
}
