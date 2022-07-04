import { Injectable } from '@angular/core';
import { Page } from '../../pagination/page';
import { Pageable } from '../../pagination/pageable';

@Injectable({
  providedIn: 'root'
})
export class CustomPaginationService {

  constructor() { }

  public getNextPage(page: Page<any>): Pageable {
    if(!page.pageable.IsLast) {
      page.pageable.pageCurrent = page.pageable.pageCurrent+1;
    }
    return page.pageable;
  }

  public getPreviousPage(page: Page<any>): Pageable {
    if(!page.pageable.IsFirst) {
      page.pageable.pageCurrent = page.pageable.pageCurrent-1;
    }
    return page.pageable;
  }
}
