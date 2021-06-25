import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Schedule } from '../models/schedule';

@Injectable({
  providedIn: 'root'
})
export class ScheduleService {

  apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getSchedules(userLogin: string, month: number): Observable<Schedule[]>{
    let params = new HttpParams();
      params = params.append('login', userLogin);
      params = params.append('month', month+1);
      return this.http.get<Schedule[]>(this.apiUrl + 'Schedule', {params: params}).pipe(
        map(schedule => {
          return schedule;
        })
      )
  }

  getAvailableDays(scheduleId: number, employeeLogin: string, month: number): Observable<Date[]>{
    let params = new HttpParams();
    params = params.append('scheduleId', scheduleId);
    params = params.append('employeeLogin', employeeLogin);
    params = params.append('month', month);
    return this.http.get<Date[]>(this.apiUrl + 'schedule/GetAvailableDays', {params: params}).pipe(
      map(dates => {
        return dates
      })
    )
  }

  updateSchedule(scheduleId: number, date: Date): Observable<Object>{
    let params = new HttpParams();
    params = params.append('scheduleId', scheduleId);
    params = params.append('day', date.getDate());
    return this.http.get(this.apiUrl + 'schedule/Update', {params: params}).pipe(
      map(response => {
        return response;
      })
    )
  }

  deleteSchedule(scheduleId: number): Observable<Object>{
    let params= new HttpParams();
    params = params.append('scheduleId', scheduleId);
    return this.http.delete(this.apiUrl + 'schedule', {params:  params}).pipe(
      map(response => {
        return response;
      })
    )
  }
}
