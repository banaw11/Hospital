import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { map, take, tap } from 'rxjs/operators';
import { AccountService } from '../services/account.service';
import { UserService } from '../services/user.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
constructor(private accountService: AccountService, private router : Router){
}



  canActivate(): Observable<boolean | UrlTree>{
    return this.accountService.currentUser$.pipe(take(1), map(user => {
      if(user){
        return true
      }
      else {
        return this.router.createUrlTree(['home/login']);
      }
    }))
  }
  
}
