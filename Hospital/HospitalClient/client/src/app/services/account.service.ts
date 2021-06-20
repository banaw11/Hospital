import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Account } from '../models/account';
import { map, take } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/user';
import { UserService } from './user.service';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private currentUserSource = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSource.asObservable();
  isAdministrator : boolean = false;
  apiUrl = environment.apiUrl;
  
  constructor(private http: HttpClient, protected userService: UserService) { }

  signIn(model : Account){
    return this.http.post<User>(this.apiUrl + 'account/signin', model).pipe(
      map((response : User) => {
        if(response){
          this.setCurentUserData(response);
        }
      })
    )
  }

  logOut(){
    this.currentUserSource.next(null)
  }

  setCurentUserData(user: User){
    if(user){
      this.isAdministrator = this.setUserRole(user.token);
      this.currentUserSource.next(user);
    }
  }

  setUserRole(token : string) : boolean{
    const role = JSON.parse(
      window.atob(
        token.split('.')[1]
        ))[environment.jwtRole];

    if(role === "ADMINISTRATOR"){
      return true;
    }
    return false;
  }

}
