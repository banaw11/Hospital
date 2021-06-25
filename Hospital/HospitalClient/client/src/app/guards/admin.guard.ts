import { Injectable } from '@angular/core';
import {  CanActivate, Router, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { map , take} from 'rxjs/operators';
import { AccountService } from '../services/account.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {
  constructor(private accountService: AccountService, private router: Router){

  }
  
  canActivate(): Observable<boolean | UrlTree>{
    return this.accountService.currentUser$.pipe(take(1), map(user => {
      if(user){
        let isAdministrator = this.accountService.setUserRole(user.token);
        if(isAdministrator){
          return true;
        }
        else{
          return this.router.createUrlTree(['home']);
        }
      }
      else {
        return this.router.createUrlTree(['home/login']);
      }
    }))
  }
  
}
