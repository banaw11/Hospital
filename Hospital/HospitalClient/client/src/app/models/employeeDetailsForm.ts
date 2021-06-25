import { Profession, Specialization } from "./enums";

export class EmployeeDetailsForm{
    filterColumn: string;
    columnNamesList: any[] =[
        {value: "PROFESSION", name: "Zawód"},
        {value: "SPECIALIZATION", name: "Specializacja"}
    ];
    professionList: any[] = [
        {value : Profession.DOCTOR, name: "Lekarz"},
        {value: Profession.NURSE, name: "Pielęgniarka"}
    ];
    specializationList: any[] = [
        {value : Specialization.CARDIOLOGIST, name: "Kardiolog" },
        {value : Specialization.UROLOGIST, name: "Urolog" },
        {value : Specialization.NEUROLOGIST, name: "Neurolog" },
        {value : Specialization.LARYNGOLOGIST, name: "Laryngolog" }
    ];
    searchPhrase: string;

    constructor(){
        this.filterColumn = '';
        this.searchPhrase = '';
    }

}