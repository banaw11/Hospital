import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Observable } from 'rxjs';
import { EmployeeDetails } from 'src/app/models/employeeDetails';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-account-editor',
  templateUrl: './account-editor.component.html',
  styleUrls: ['./account-editor.component.scss']
})
export class AccountEditorComponent implements OnInit, OnChanges {
  @Input() employee : Observable<EmployeeDetails> = new Observable<EmployeeDetails>();
  employeeDetails : EmployeeDetails = {};
  submitted: boolean = false;
  statusMessage? : boolean = true;
  message? : string;
  errors: string[]=[];
  constructor(private accountService: AccountService) { 
    
  }
  ngOnChanges(changes: SimpleChanges): void {
    if(changes.employee){
      changes.employee.currentValue.subscribe((data: EmployeeDetails) => {
        if(data){
          this.employeeDetails = data;
        }
      })
    }
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
