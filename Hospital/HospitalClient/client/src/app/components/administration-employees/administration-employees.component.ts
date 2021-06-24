import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { EmployeeDetails } from 'src/app/models/employeeDetails';
import { AccountService } from 'src/app/services/account.service';
import { EmployeeService } from 'src/app/services/employee.service';

@Component({
  selector: 'app-administration-employees',
  templateUrl: './administration-employees.component.html',
  styleUrls: ['./administration-employees.component.scss']
})
export class AdministrationEmployeesComponent implements OnInit {

  selectedUserLogin: string ='';
  submitted: boolean = false;
  statusMessage? : boolean = true;
  message? : string;
  errors: string[]=[];
  hidden : boolean = true;
  employeeData: Observable<EmployeeDetails> = new Observable<EmployeeDetails>();
  constructor(public employeeService: EmployeeService, private accountService: AccountService, private router: Router) { }

  ngOnInit(): void {
  }

  userSelected(event:string){
    this.selectedUserLogin = event;
    this.employeeData = this.employeeService.getProfileData(event)
  }

  delete(){
    this.submitted = true;
    this.errors = [];
    this.accountService.delete(this.selectedUserLogin).subscribe(() => {
      this.statusMessage = true;
      this.message = "Konto zostało usunięte";
      setTimeout(() => {
        this.router.navigateByUrl('/home');
      }, 1000);
    }, error => {
      this.errors.push(error.error);
      this.statusMessage = false;
      this.message= "Konto nie zostało usunięte";
    })
  }

}
