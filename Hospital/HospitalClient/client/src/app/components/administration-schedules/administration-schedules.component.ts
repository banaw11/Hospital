import { Component, OnInit } from '@angular/core';
import { NbMenuItem } from '@nebular/theme';
import { AccountService } from 'src/app/services/account.service';
import { ScheduleService } from 'src/app/services/schedule.service';

@Component({
  selector: 'app-administration-schedules',
  templateUrl: './administration-schedules.component.html',
  styleUrls: ['./administration-schedules.component.scss']
})
export class AdministrationSchedulesComponent implements OnInit {

  flipped = false;
  employeeLogin: string ='';
  availableDates: Date[] = [];
  selectedScheduleId = 0;
  constructor(public accountService: AccountService, private scheduleService: ScheduleService) { }

  ngOnInit(): void {
  }

  userSelected(login: string){
    this.employeeLogin = login;
    this.flipped = true;
    this.availableDates = [];
  }

  changeEvent(data: any){
    this.selectedScheduleId = data.scheduleId; 
    let employeeLogin = data.employeeLogin;
    let month = data.month;
    console.log(data)
    this.scheduleService.getAvailableDays(this.selectedScheduleId, employeeLogin, month).subscribe(dates => {
      if(dates){
        this.availableDates = dates;
      }
    })
  }

  deleteEvent(data: any){
    this.scheduleService.deleteSchedule(data.scheduleId).subscribe(res => {
      if(!res){
        console.log(res)
      }
      this.availableDates = [];
    })
  }

  selectedDay(date: Date){
    this.scheduleService.updateSchedule(this.selectedScheduleId, date).subscribe(res => {
      if(!res){
        console.log(res);
      }
      this.availableDates = [];
    })
  }

}
