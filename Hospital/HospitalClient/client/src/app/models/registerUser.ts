import { Profession, Specialization } from "./enums";

export interface RegisterUser{
    password: string,
    confirmPassword: string,
    firstName: string,
    lastName: string,
    personalId: string,
    profession?: Profession,
    specialization?: Specialization
    rtPPNumber?: string
}