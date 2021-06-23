export interface CallendaryDay{
    date : Date,
    dayOfWeek : {value: number, name: string | undefined}
    dayNumber : number,
    isDuty : boolean
}