import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserService } from '../services/user.service';
import { User } from '../models/user';
import { map, take } from 'rxjs/operators';
import { AccountService } from '../services/account.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private accountService : AccountService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {

    this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
      let token : string = '';
      if(user){
        token = user.token;
      }
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      })
    })
    return next.handle(request);
  }
}
