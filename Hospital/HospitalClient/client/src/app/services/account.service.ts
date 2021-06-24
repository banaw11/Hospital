import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Account } from '../models/account';
import { map, take } from 'rxjs/operators';
import { HttpClient, HttpParams } from '@angular/common/http';
import { User } from '../models/user';
import { UserService } from './user.service';
import { BehaviorSubject } from 'rxjs';
import { EmployeeService } from './employee.service';
import { RegisterUser } from '../models/registerUser';
import { CreatedAccount } from '../models/createdAccount';
import { ResetPassword } from '../models/resetPassword';
import { EmployeeDetails } from '../models/employeeDetails';

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

  registerUser(registerUser: RegisterUser){
    return this.http.post<CreatedAccount>(this.apiUrl + 'account/register', registerUser).pipe(
      map((response : CreatedAccount) => {
        if(response){
          return response;
        }
        return
      })
    )
  }

  resetPassword(dto: ResetPassword){
    return this.http.put<ResetPassword>(this.apiUrl + 'account/password', dto).pipe(
      map(response => {
        return response;
      })
    )
  }

  updateProfile(dto: EmployeeDetails){
    return this.http.put<EmployeeDetails>(this.apiUrl + 'account', dto).pipe(
      map(response => {
        return response;
      })
    )
  }

  delete(login: string){
    let params = new HttpParams();
    params = params.append('login', login);
    return this.http.delete(this.apiUrl + 'account', {params: params}).pipe(
      map(response => {
        return response;
      })
    )
  }


}
