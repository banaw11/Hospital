import { NbMenuItem } from "@nebular/theme";

export interface CallendaryDay{
    date : Date,
    dayOfWeek : {value: number, name: string | undefined}
    dayNumber : number,
    isDuty : boolean,
    isAvailable: boolean,
    scheduleData? : NbMenuItem[];
}