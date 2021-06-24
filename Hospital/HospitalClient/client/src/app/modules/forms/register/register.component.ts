import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Profession, Specialization } from 'src/app/models/enums';
import { RegisterUser } from 'src/app/models/registerUser';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  registerUser: RegisterUser = {
    password: '',
    confirmPassword: '',
    firstName: '',
    lastName: '',
    personalId: ''
  }

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

  submitted: boolean = false;
  statusMessage? : boolean = true;
  message? : string;
  errors: string[]=[];

  constructor(private accountService: AccountService, private router: Router) { }

  ngOnInit(): void {
  }

  register(): void{
    this.submitted = true;
    this.errors = [];
    this.accountService.registerUser(this.registerUser).subscribe(() => {
      this.statusMessage = true;
      this.message = "Rejestracja pomyślna";
      setTimeout(() => {
        this.router.navigateByUrl('/home');
      }, 500);
    }, error => {
      if(error.error.errors.Password){
        this.errors.push(error.error.errors.Password);
      }
      if(error.error.errors.ConfirmPassword){
        this.errors.push(error.error.errors.ConfirmPassword);
      }
      if(error.error.errors.LastName){
        this.errors.push(error.error.errors.LastName);
      }
      if(error.error.errors.FirstName){
        this.errors.push(error.error.errors.FirstName);
      }
      if(error.error.errors.PersonalId){
        this.errors.push(error.error.errors.PersonalId);
      }
      if(error.error.errors.Profession){
        this.errors.push(error.error.errors.Profession);
      }
      if(error.error.errors.Specialization){
        this.errors.push(error.error.errors.Specialization);
      }
      if(error.error.errors.RtPPNumber){
        this.errors.push(error.error.errors.RtPPNumber);
      }
      this.statusMessage = false;
      this.message= "Błąd rejestracji";
    })
  }

}
