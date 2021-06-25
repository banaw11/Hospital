import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { map, take } from 'rxjs/operators';
import { Account } from 'src/app/models/account';
import { AccountService } from 'src/app/services/account.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  user: Account = {
    login: "",
    password: ""
  };

  submitted : boolean = false;
  statusMessage? : boolean = true;
  message? : string;
  errors: string[] = [];
  constructor(private accountService : AccountService, private router : Router) {
    
   }

  ngOnInit(): void {
  }

  signIn(): void {
    this.submitted = true;
    this.errors = [];
    this.accountService.signIn(this.user).subscribe(() => {
      this.statusMessage = true;
      this.message = "Logowanie pomyślne";
      setTimeout(() => {
        this.router.navigateByUrl('/home');
      }, 500);
    }, error => {
      this.errors.push(error.error);
      this.statusMessage = false;
      this.message= "Błąd logowania";
    })
  }

}
