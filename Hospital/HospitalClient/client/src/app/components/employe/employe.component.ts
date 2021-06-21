import { Component, OnInit, ViewChild } from '@angular/core';
import { NbSelectModule } from '@nebular/theme';
import { Observable, of as observableOf } from 'rxjs';
import { map } from 'rxjs/operators';
import { BasicEmployeDetails } from 'src/app/models/basicEmployeDetails';
import { EmployeeDetails } from 'src/app/models/employeeDetails';
import { EmployeeDetailsForm } from 'src/app/models/employeeDetailsForm';
import { EmployeePaginationQuery } from 'src/app/models/employeePaginationQuery';
import { Profession, Specialization } from 'src/app/models/enums';
import { PagedList } from 'src/app/models/pagedList';
import { EmployeeService } from 'src/app/services/employee.service';

@Component({
  selector: 'app-employe',
  templateUrl: './employe.component.html',
  styleUrls: ['./employe.component.scss']
})
export class EmployeComponent implements OnInit {

  pagedList: PagedList<BasicEmployeDetails> = new PagedList<BasicEmployeDetails>();
  query: EmployeePaginationQuery = new EmployeePaginationQuery();
  form: EmployeeDetailsForm = new EmployeeDetailsForm();
  valuesForSearch : any[] = [];
  employeeSortChecked: boolean = false;
  professionSortChecked: boolean = true;
  specializationSortChecked: boolean = false;
  checked = true;
  employeeDetails: EmployeeDetails ={}
  flipped: boolean = false


  pages: {name: string, value: number}[] = [];

  constructor(private employeeService: EmployeeService) {
    this.getPaginationResult();
  }

  ngOnInit(): void {
  }

  checkedChange(event : any, sortBy : string){
    this.query.sortBy = event.target.checked ? sortBy : '';
    this.getPaginationResult();
  }

  changePageSize(size: number){
    if(this.query.pageSize != size){
      this.query.pageSize = size;
      this.getPaginationResult();
    }
  }

  changePageNumber(number: number){
    if(this.query.pageNumber != number && number >0 && number <= this.pagedList.totalPages){
      this.query.pageNumber = number;
      this.getPaginationResult();
    }
  }

  changeSearchPhrase(event: number | null){
    let columnName = this.query.filterColumn;
    let phrase;
    if(columnName != '' && event != null){
      if(columnName === "PROFESSION"){
        phrase = Profession[event];
        this.query.searchPhrase = phrase;
        this.getPaginationResult();
      }
      else if(columnName === "SPECIALIZATION"){
        phrase = Specialization[event];
        this.query.searchPhrase = phrase;
        this.getPaginationResult();
      }
    }
  }

  changeFilterColumn(event : string | null){
    if(this.query.filterColumn != event){
      if(event != null){
        this.query.filterColumn = event;
        if(event === "PROFESSION"){
          this.valuesForSearch = this.form.professionList;
        }
        else if(event === "SPECIALIZATION")
        {
          this.valuesForSearch = this.form.specializationList;
        }
      }
      else{
        this.query.filterColumn = '';
        this.valuesForSearch = [];
        this.getPaginationResult();
      }
    }
  }

  getPaginationResult(){
    this.employeeService.getBasicEmployeeDetails(this.query).subscribe(list => {
      this.pagedList = list;
      this.getPagesList(this.query.pageNumber);
   });
  }

  getPagesList(pageNumber: number){
    let lastNumber = this.pagedList.totalPages;
    let firstNumber = pageNumber;
    let dif = lastNumber - firstNumber;

    if(this.pages.length > 0 && dif <4){
     firstNumber = this.pages[0].value ;
     dif = lastNumber - firstNumber;
    }
    
    if(dif > 4){
      lastNumber = firstNumber + 4;
    }
    this.pages = [];
    for(let i=firstNumber; i<= lastNumber; i++){
      this.pages.push({name: i.toLocaleString(), value: i});
    }
  }

  getEmployeeDetails(login: string){
    this.employeeService.getProfileData(login).subscribe(data => {
      this.employeeDetails = data;
      this.flipped = true;
    })
  }

}
