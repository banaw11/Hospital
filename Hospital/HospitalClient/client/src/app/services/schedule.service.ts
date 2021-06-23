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
}
