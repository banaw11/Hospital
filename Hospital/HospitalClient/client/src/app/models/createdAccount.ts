import { Profession, Specialization } from "./enums";

export interface CreatedAccount{
    login: string,
    name: string,
    profession: Profession,
    specialization: Specialization
}