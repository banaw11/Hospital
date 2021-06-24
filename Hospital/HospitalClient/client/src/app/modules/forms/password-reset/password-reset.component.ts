import { Component, OnInit, Output } from '@angular/core';
import { ResetPassword } from 'src/app/models/resetPassword';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-password-reset',
  templateUrl: './password-reset.component.html',
  styleUrls: ['./password-reset.component.scss']
})
export class PasswordResetComponent implements OnInit {
  resetPassword: ResetPassword = {
    password:'', confirmPassword:'', login:''
  };
  submitted: boolean = false;
  statusMessage? : boolean = true;
  message? : string;
  errors: string[]=[];
  hidden : boolean = true;

  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
  }


  showForm(event: string){
    this.hidden = false;
    this.resetPassword= {
      password:'', confirmPassword:'', login: event
    }
  }
  reset(){
    this.submitted = true;
    this.errors = [];
    this.accountService.resetPassword(this.resetPassword).subscribe(() => {
      this.statusMessage = true;
      this.message = "Hasło zostało zmienione";
      setTimeout(() => {
        this.submitted = false;
        this.errors = [];
      }, 1000);
    }, error => {
      this.errors.push(error.error);
      this.statusMessage = false;
      this.message= "Hasło nie zostało zmienione";
    })
  }
}
