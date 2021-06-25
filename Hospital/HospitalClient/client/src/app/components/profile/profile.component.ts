import { Component, OnInit } from '@angular/core';
import { EmployeeDetails } from 'src/app/models/employeeDetails';
import { EmployeeService } from 'src/app/services/employee.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  employeeDetails: EmployeeDetails = {};

  constructor(private employeeService: EmployeeService) {
    this.employeeService.getProfileData().subscribe(data => {
      this.employeeDetails = data;
    })
   }

  ngOnInit(): void {
  }

}
