import { CallendaryDay } from "./callendaryDay";

export class Callendar{
    days: CallendaryDay[];
    month: number;
    year: number;
    daysInMonth: number;

    constructor(month: number, year: number){
        this.month = month;
        this.year = month;
        this.days = [];
        this.daysInMonth = new Date(year,month+1,0).getDate();
        for(let d=1 ; d<=this.daysInMonth; d++){
            let _date = new Date(year, month, d);
            this.days.push({
                date: _date,
                dayOfWeek: {value : _date.getDay(), name : this.nameOfDay(_date.getDay())},
                dayNumber : d,
                isDuty : false
            })
        }
    }

    nameOfDay(day: number) : string | undefined{
        let names = new Map<number, string>();
        names.set(1,'Poniedziałek');
        names.set(2,'Wtorek');
        names.set(3,'Środa');
        names.set(4,'Czwartek');
        names.set(5,'Piątek');
        names.set(6,'Sobota'); 
        names.set(0,'Niedziela');
        return names.get(day);
    }
}