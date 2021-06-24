import { Component, Input, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { EmployeeDetails } from 'src/app/models/employeeDetails';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-account-editor',
  templateUrl: './account-editor.component.html',
  styleUrls: ['./account-editor.component.scss']
})
export class AccountEditorComponent implements OnInit {
  @Input() employee : Observable<EmployeeDetails> = new Observable<EmployeeDetails>();
  employeeDetails : EmployeeDetails = {};
  submitted: boolean = false;
  statusMessage? : boolean = true;
  message? : string;
  errors: string[]=[];
  constructor(private accountService: AccountService) { 
    
  }

  ngOnInit(): void {
    this.employee.subscribe(data => {
      if(data){
        this.employeeDetails = data;
      }
    })
  }

  save(){
    this.submitted = true;
    this.errors = [];
    this.accountService.updateProfile(this.employeeDetails).subscribe(() => {
      this.statusMessage = true;
      this.message = "Dane zostały zapisane";
      setTimeout(() => {
        this.submitted = false;
        this.errors = [];
      }, 1000);
    }, error => {
      this.errors.push(error.error);
      this.statusMessage = false;
      this.message= "Dane nie zostały zaaktualizowane";
    })
  }

}
