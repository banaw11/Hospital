import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { BasicEmployeDetails } from '../models/basicEmployeDetails';
import { EmployeePaginationQuery } from '../models/employeePaginationQuery';
import { PagedList } from '../models/pagedList';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  apiUrl = environment.apiUrl

  constructor(private http: HttpClient) { }
  

  getBasicEmployeeDetails(query: EmployeePaginationQuery) : Observable<PagedList<BasicEmployeDetails>>{
    let params = this.setParams(query);
    return this.http.get<PagedList<BasicEmployeDetails>>(this.apiUrl + 'Employee/basicDetails', {params: params} ).pipe(
      map((list)  => {
        return list;
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
}