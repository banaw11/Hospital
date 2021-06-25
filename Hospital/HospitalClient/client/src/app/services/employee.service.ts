import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { BasicEmployeDetails } from '../models/basicEmployeDetails';
import { EmployeeDetails } from '../models/employeeDetails';
import { EmployeePaginationQuery } from '../models/employeePaginationQuery';
import { Profession, Specialization } from '../models/enums';
import { PagedList } from '../models/pagedList';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  apiUrl = environment.apiUrl
  currentUserLogin: string = '';

  constructor(private http: HttpClient) { }
  

  getBasicEmployeeDetails(query: EmployeePaginationQuery) : Observable<PagedList<BasicEmployeDetails>>{
    let params = this.setParams(query);
    return this.http.get<PagedList<BasicEmployeDetails>>(this.apiUrl + 'Employee/basicDetails', {params: params} ).pipe(
      map((list)  => {
        return list;
      })
    )
  }

  getProfileData(login?: string) : Observable<EmployeeDetails>{
    if(login == null){
      login = this.currentUserLogin;
    }
    let params : HttpParams = new HttpParams();
    params = params.append('login', login);
    return this.http.get<EmployeeDetails>(this.apiUrl + 'Employee/profile', {params: params}).pipe(
      map(data => {
        return data;
      })
    )
  }

  setParams(query: EmployeePaginationQuery): HttpParams{
    let params : HttpParams = new HttpParams();
    params = params.append('filterColumn', query.filterColumn);
    params = params.append('searchPhrase', query.searchPhrase);
    params = params.append('pageSize', query.pageSize);
    params = params.append('pageNumber', query.pageNumber);
    params = params.append('sortBy', query.sortBy);
    return params;
  }

  getEmployees(prof: Profession, spec?: Specialization): Observable<BasicEmployeDetails[]>{
    let params = new HttpParams();
    params = params.append('profession', prof);
    params = params.append('specialization', spec ? spec : '');
    return this.http.get<BasicEmployeDetails[]>(this.apiUrl +'Employee', {params: params}).pipe(
      map(list => {
        return list;
      })
    )
  }
}
