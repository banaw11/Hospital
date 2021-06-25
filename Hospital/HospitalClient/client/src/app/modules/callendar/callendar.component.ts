import { formatDate } from '@angular/common';
import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { NbMenuItem, NbMenuService } from '@nebular/theme';
import { map } from 'rxjs/operators';
import { Callendar } from 'src/app/models/callendar';
import { CallendaryDay } from 'src/app/models/callendaryDay';
import { EmployeeService } from 'src/app/services/employee.service';
import { ScheduleService } from 'src/app/services/schedule.service';

@Component({
  selector: 'app-callendar',
  templateUrl: './callendar.component.html',
  styleUrls: ['./callendar.component.scss']
})
export class CallendarComponent implements OnInit, OnChanges {
  @Input() employeeLogin! : string;
  @Input() isAdministrator: boolean = false;
  @Input() availableDates : Date[] = [];
  @Output() changeEvent = new EventEmitter<any>();
  @Output() deleteEvent = new EventEmitter<any>();
  @Output() selectedDayEvent = new EventEmitter<Date>();
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
  calendarTemplateWeek: { dayOfWeek: number,calendarDay?: CallendaryDay, data?:NbMenuItem[] }[] =
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


  constructor(private scheduleService: ScheduleService, private menuService: NbMenuService) { 
    for(let i=1;i<=12;i++){
      this.months.push({index: new Date(this.year,i,0).getMonth(), name: this.monthNames[i-1].name})
    }
    this.createCalendar();
  }
  ngOnChanges(changes: SimpleChanges): void {
    for (const propName in changes) {
      if (changes.hasOwnProperty(propName)) {
        switch (propName) {
          case 'employeeLogin': {
            if(changes.employeeLogin.currentValue != ''){
              this.getNewCalendar(this.currentMonth);
            }
            break;
          }
          case 'availableDates': {
            if(changes.availableDates.currentValue != undefined && this.employeeLogin != ''){
              this.getNewCalendar(this.currentMonth);
            }
            break;
          }
        }
      }
    }
  }

  ngOnInit(): void {
    if(this.employeeLogin != ''){
      this.scheduleService.getSchedules(this.employeeLogin, this.currentMonth).subscribe(schedules =>{
        if(schedules){
          for(let schedule of schedules){
            let index = this.callendar.days
              .findIndex(x => 
                formatDate(x.date, 'yyyy-MM-dd', 'en_US') == formatDate(schedule.date, 'yyyy-MM-dd', 'en_US')
              );
            if(index > -1){
              this.callendar.days[index].isDuty = true;
              this.callendar.days[index].scheduleData = [
                {title: 'Zmień termin', data: {tag:'change', employeeLogin: schedule.employeeLogin, scheduleId: schedule.id, month : schedule.month}},
                {title: 'Usuń dyżur', data: {tag:'delete', employeeLogin: schedule.employeeLogin, scheduleId: schedule.id, month : schedule.month}}
              ];
            }
          }
          if(this.availableDates.length > 0){
            for(let date of this.availableDates){
              let index = this.callendar.days.findIndex(d => 
                formatDate(d.date, 'yyyy-MM-dd', 'en_US') == formatDate(date, 'yyyy-MM-dd', 'en_US'));
                if(index > -1){
                  this.callendar.days[index].isAvailable = true;
                }
            }
          }
        }
      });
    }
    this.menuService.onItemClick().pipe(
      map(({item: {data}}) => data),
    ).subscribe(data => {
      if(data.tag == 'change'){
        this.changeEvent.emit(data);
      }
      else if(data.tag == 'delete'){
        this.deleteEvent.emit(data);
      }
    })
  }

  getNewCalendar(event: number){
    this.isDefault = false;
    this.callendar = new Callendar(event, this.year);
    this.currentMonth = event;
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
            this.callendar.days[index].scheduleData = [
              {title: 'Zmień termin', data: {tag:'change', employeeLogin: schedule.employeeLogin, scheduleId: schedule.id, month : schedule.month}},
              {title: 'Usuń dyżur', data: {tag:'delete', employeeLogin: schedule.employeeLogin, scheduleId: schedule.id, month : schedule.month}}
            ];
          }
          if(this.availableDates.length > 0){
            let i = this.availableDates.findIndex( d => 
              formatDate(d, 'yyyy-MM-dd', 'en_US') == formatDate(schedule.date, 'yyyy-MM-dd', 'en_US'))
              if(i > -1){
                this.callendar.days[i].isAvailable = true;
              }
          }  
        }
        if(this.availableDates.length > 0){
          for(let date of this.availableDates){
            let index = this.callendar.days.findIndex(d => 
              formatDate(d.date, 'yyyy-MM-dd', 'en_US') == formatDate(date, 'yyyy-MM-dd', 'en_US'));
              if(index > -1){
                this.callendar.days[index].isAvailable = true;
              }
          }
        }
      }
    })
  }


  createCalendar(){
    this.calendarTemplateMonth = [];
    this.calendarTemplateWeek = [
      {dayOfWeek : 1},
      {dayOfWeek : 2},
      {dayOfWeek : 3},
      {dayOfWeek : 4},
      {dayOfWeek : 5},
      {dayOfWeek : 6},
      {dayOfWeek : 0},
    ];
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

  changeSchedule(day: { dayOfWeek: number,calendarDay?: CallendaryDay, data?:NbMenuItem[] }){
    if(day.calendarDay){
      this.selectedDayEvent.emit(day.calendarDay?.date);
    }
  }

}
