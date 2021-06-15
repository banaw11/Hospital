import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Account } from '../models/account';
import { map } from 'rxjs/operators';
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
  apiUrl = environment.apiUrl;
  
  constructor(private http: HttpClient, protected userService: UserService) { }

  signIn(model : Account){
    return this.http.post<User>(this.apiUrl + 'account/signin', model).pipe(
      map((response : User) => {
        if(response){
          this.currentUserSource.next(response);
        }
      })
    )
  }


  logOut(){
    this.currentUserSource.next(null)
  }

}
