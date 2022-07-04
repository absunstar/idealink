import { Constants } from 'src/app/constants';

export class Pageable {
  
  PageDisplayCount: number = 5;
  PageDisplayList: Array<number>;
  IsFirst: boolean = true;
  IsLast: boolean = false;

  private _pageSize: number;
  public get pageSize(): number {
    return this._pageSize;
  }
  public set pageSize(value: number) {
    this._pageSize = value;
  }

  private _PageTotalCount: number = -1;
  public get PageTotalCount(): number {
    return this._PageTotalCount;
  }
  public set PageTotalCount(value: number) {
    this._PageTotalCount = value;
    this.CalculateLastFirst();
    this.calculateDisplayPage();
  }

  private _pageCurrent: number;
  public get pageCurrent(): number {
    return this._pageCurrent;
  }
  public set pageCurrent(value: number) {
    if(value <= 0 )
      value = 1;
    
    if(value > this.PageTotalCount && this.PageTotalCount != -1)
      value = this.PageTotalCount;

    this._pageCurrent = value;
    this.CalculateLastFirst();
    this.calculateDisplayPage();
  }
  private CalculateLastFirst(){
    if(this._pageCurrent == 1)
      this.IsFirst = true;
    else
      this.IsFirst = false;
    
    if(this._pageCurrent == this.PageTotalCount && this.PageTotalCount != -1)
      this.IsLast = true;
    else
      this.IsLast = false;

    if(this.PageTotalCount == -1)
    {
      this.PageTotalCount = 1;
      this.IsLast = true;
      this.IsFirst = true;
    }
  }
  private calculateDisplayPage()
  {
    
    this.PageDisplayList = [];
    let indexStart = this.pageCurrent - this.PageDisplayCount;
    let indexEnd   = this.pageCurrent + this.PageDisplayCount;
    
    // if(this.pageSize == 1 && this.pageCurrent == 1)
    // {
    //   this.PageDisplayList.push(1);
    //   return;
    // }
    if(indexStart <= 0)
      indexStart = 1;
    if(indexEnd > this.PageTotalCount)
      indexEnd = this.PageTotalCount;

      for(let i=indexStart;i<=indexEnd;i++)
      {
        this.PageDisplayList.push(i);
      }
  }

  public constructor() {
    this.pageSize = Constants.PAGE_SIZE;
    this.pageCurrent = Constants.PAGE_FIRST_NUMBER;
    this.PageDisplayCount = Constants.PAGE_Display_Count;
    //this.calculateDisplayPage();
  }
}
