import { Component, Input, OnInit } from '@angular/core';
import { EmployeeService } from 'src/app/services/employee.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.scss']
})
export class ScheduleComponent implements OnInit {
  currentEmployeeLogin = '';
  constructor(private employeeService: EmployeeService) {
    this.currentEmployeeLogin = this.employeeService.currentUserLogin;
  }

  ngOnInit(): void {
  }

}
