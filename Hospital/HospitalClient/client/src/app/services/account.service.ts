import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Account } from '../models/account';
import { map, take } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/user';
import { UserService } from './user.service';
import { BehaviorSubject } from 'rxjs';
import { EmployeeService } from './employee.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private currentUserSource = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSource.asObservable();
  private isAdministartorSource = new BehaviorSubject<boolean>(false);
  isAdministrator = this.isAdministartorSource.asObservable();
  apiUrl = environment.apiUrl;
  
  constructor(private http: HttpClient, protected userService: UserService, private employeeService: EmployeeService) { }

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
      this.isAdministartorSource.next(this.setUserRole(user.token));
      this.currentUserSource.next(user);
      this.employeeService.currentUserLogin = user.login;
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
