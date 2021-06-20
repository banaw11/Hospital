export class EmployeePaginationQuery{
    filterColumn : string;
    searchPhrase : string;
    sortBy : string;
    pageSize: number;
    pageNumber: number;
    
    constructor(){
        this.filterColumn = '';
        this.searchPhrase = '';
        this.sortBy = '';
        this.pageSize = 5;
        this.pageNumber = 1;
    }
}