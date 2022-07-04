//import  { Sort } from './sort';
import  { Pageable } from './pageable';

export class Page<T> {
  content: Array<T>;
  pageable: Pageable;
  
  private _totalElements: number;
  public get totalElements(): number {
    return this._totalElements;
  }
  public set totalElements(value: number) {
    
    this._totalElements = value;
    this.pageable.PageTotalCount = Math.ceil((this.totalElements/this.pageable.pageSize))
    this.pageable.PageTotalCount = this.pageable.PageTotalCount == 0 ? 1 : this.pageable.PageTotalCount;
  }
  
  public constructor() {
    this.pageable = new Pageable();
  }
}
