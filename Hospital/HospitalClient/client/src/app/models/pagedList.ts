export class PagedList<T>{
    currentPage : number;
    totalPages: number;
    pageZise: number;
    totalCount: number;
    items: T[];

    constructor(
        _currentPage?: number, _totalPages?: number, _pageSize?: number, _totalCount?: number, _items?: T[]){
            this.currentPage = _currentPage ? _currentPage : 1;
            this.totalPages = _totalPages ? _totalPages : 0
            this.pageZise = _pageSize ? _pageSize : 0;
            this.totalCount = _totalCount ? _totalCount : 0;
            this.items = _items ? _items : [];
        }
}