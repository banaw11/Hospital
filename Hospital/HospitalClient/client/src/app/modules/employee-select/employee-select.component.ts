import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { BasicEmployeDetails } from 'src/app/models/basicEmployeDetails';
import { Profession, Specialization } from 'src/app/models/enums';
import { EmployeeService } from 'src/app/services/employee.service';

@Component({
  selector: 'app-employee-select',
  templateUrl: './employee-select.component.html',
  styleUrls: ['./employee-select.component.scss']
})
export class EmployeeSelectComponent implements OnInit {

  @Output() userSelected = new EventEmitter<string>()

  professions : {value: Profession, name: string}[] = [
    {value: Profession.ADMINISTRATOR, name: 'ADMINISTRATOR'},
    {value: Profession.DOCTOR, name: 'LEKARZ'},
    {value: Profession.NURSE, name: 'Pielęgniarka'}
  ]

  specializations: {value: Specialization, name: string}[] =[
    {value: Specialization.NULL, name: 'brak'},
    {value: Specialization.CARDIOLOGIST, name: 'Kardiolog'},
    {value: Specialization.UROLOGIST, name: 'Urolog'},
    {value: Specialization.NEUROLOGIST, name: 'Neurolog'},
    {value: Specialization.LARYNGOLOGIST, name: 'Laryngolog'}
  ]


  employees : BasicEmployeDetails[] =[];
  hidden : boolean = true;
  profession: Profession = Profession.ADMINISTRATOR;
  constructor(private employeeService: EmployeeService) { }

  ngOnInit(): void {
  }

  onSelect(login: string){
    this.userSelected.emit(login);
    console.log(login);
  }

  professionSelected(prof: Profession){
    this.profession = prof;
    if(prof != 1){
      this.employeeService.getEmployees(prof).subscribe(empl => {
        if(empl){
          this.employees = empl;
          this.hidden = true;
        }
      })
    }
    else{
      this.hidden = false;
    }
  }

  specializationSelected(spec: Specialization){
    if(this.profession == 1){
      this.employeeService.getEmployees(this.profession, spec).subscribe(empl => {
        if(empl){
          this.employees = empl;
        }
      })
    }
  }

}
