import { formatDate } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { Callendar } from 'src/app/models/callendar';
import { CallendaryDay } from 'src/app/models/callendaryDay';
import { EmployeeService } from 'src/app/services/employee.service';
import { ScheduleService } from 'src/app/services/schedule.service';

@Component({
  selector: 'app-callendar',
  templateUrl: './callendar.component.html',
  styleUrls: ['./callendar.component.scss']
})
export class CallendarComponent implements OnInit {
  @Input() employeeLogin! : string;
  isDefault : boolean = true;
  months: {index: number, name: string}[]= []; 
  monthNames: {name: string}[] = [
    {name: "STYCZEŃ"},
    {name: "LUTY"},
    {name: "MARZEC"},
    {name: "KWIECIEŃ"},
    {name: "MAJ"},
    {name: "CZERWIEC"},
    {name: "LIPIEC"},
    {name: "SIERPIEŃ"},
    {name: "WRZESIEŃ"},
    {name: "PAŹDZIERNIK"},
    {name: "LISTOPAD"},
    {name: "GRUDZIEŃ"},
  ]
  currentMonth = new Date().getMonth();
  year: number = new Date().getFullYear();
  
  daynames = [
    {value:1, name: "Poniedziałek"},
    {value:2, name: "Wtorek"},
    {value:3, name: "Środa"},
    {value:4, name: "Czwartek"},
    {value:5, name: "Piątek"},
    {value:6, name: "Sobota"},
    {value:0, name: "Niedziela"}
  ];
  calendarTemplateWeek: {dayOfWeek: number,calendarDay?: CallendaryDay}[] =
  [
    {dayOfWeek : 1},
    {dayOfWeek : 2},
    {dayOfWeek : 3},
    {dayOfWeek : 4},
    {dayOfWeek : 5},
    {dayOfWeek : 6},
    {dayOfWeek : 0},
  ];
  calendarTemplateMonth : {dayOfWeek: number,calendarDay?: CallendaryDay}[][] = [];

  callendar = new Callendar(new Date().getMonth(), new Date().getFullYear());


  constructor(private scheduleService: ScheduleService) { 
    for(let i=1;i<=12;i++){
      this.months.push({index: new Date(this.year,i,0).getMonth(), name: this.monthNames[i-1].name})
    }
    this.createCalendar();
  }

  ngOnInit(): void {
    this.scheduleService.getSchedules(this.employeeLogin, this.currentMonth).subscribe(schedules =>{
      if(schedules){
        for(let schedule of schedules){
          let index = this.callendar.days
            .findIndex(x => 
              formatDate(x.date, 'yyyy-MM-dd', 'en_US') == formatDate(schedule.date, 'yyyy-MM-dd', 'en_US')
            );
          if(index > -1){
            this.callendar.days[index].isDuty = true;
          }  
        }
      }
    })
  }

  getNewCalendar(event: number){
    this.isDefault = false;
    this.callendar = new Callendar(event, this.year);
    this.createCalendar();
    this.scheduleService.getSchedules(this.employeeLogin, event).subscribe(schedules =>{
      if(schedules){
        for(let schedule of schedules){
          let index = this.callendar.days
            .findIndex(x => 
              formatDate(x.date, 'yyyy-MM-dd', 'en_US') == formatDate(schedule.date, 'yyyy-MM-dd', 'en_US')
            );
          if(index > -1){
            this.callendar.days[index].isDuty = true;
          }  
        }
      }
    })
  }


  createCalendar(){
    this.calendarTemplateMonth = [];
    let week = this.calendarTemplateWeek;
      for(let i = 0; i<this.callendar.daysInMonth ;i++){
        let day = this.callendar.days[i];
        let index = week.findIndex(_day => _day.dayOfWeek === day.dayOfWeek.value)
        if(day.dayOfWeek.value === 0 || i+1 === this.callendar.daysInMonth){
          week[index].calendarDay = day;
          this.calendarTemplateMonth.push(week);
          week = [
            {dayOfWeek : 1},
            {dayOfWeek : 2},
            {dayOfWeek : 3},
            {dayOfWeek : 4},
            {dayOfWeek : 5},
            {dayOfWeek : 6},
            {dayOfWeek : 0},
          ];
        }else if(day.dayOfWeek.value !== 0){
          week[index].calendarDay = day;
        }
      }
  }

}
