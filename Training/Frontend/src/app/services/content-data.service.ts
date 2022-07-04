import { Injectable } from '@angular/core';
import { Constants } from '../constants';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { cContentDataItem, cContentDataList } from '../interface/Response/ContentData.class';
import { ModelId } from '../interface/Model/ModelId.interface';
import { tap, catchError } from 'rxjs/operators';
import { ModelPaging } from '../interface/Model/ModelPaging.interface';

@Injectable({
  providedIn: 'root'
})
export class ServiceContentData {

  private apiURL = Constants.apiRoot + 'ContentData/'

  constructor(private httpClient: HttpClient) { }
  ContentDataOneGetByTypeId(type): Observable<cContentDataItem> {
    var model = new ModelId();
    model.Id = <string>type;
    return this.httpClient.post<cContentDataItem>(this.apiURL + 'ContentDataOneGetByTypeId', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  getContentDataGetByid(Id: string): Observable<cContentDataItem> {
    var model = new ModelId();
    model.Id = Id;
    return this.httpClient.post<cContentDataItem>(this.apiURL + 'ContentDataGetById', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  getContentDataListActive(): Observable<cContentDataItem[]> {
    return this.httpClient.get<cContentDataItem[]>(this.apiURL + 'ContentDataListActive').pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  getContentDataAll(currentPage: number, filterText: string): Observable<cContentDataList> {

    var model = new ModelPaging()
    model.CurrentPage = currentPage;
    model.filterText = filterText;
    return this.httpClient.post<cContentDataList>(this.apiURL + 'ContentDataListAll', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  setContentDataActivate(Id: string): Observable<boolean> {
    var model = new ModelId();
    model.Id = Id;
    return this.httpClient.post<boolean>(this.apiURL + 'ContentDataActivate', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  setContentDataDeactivate(Id: string): Observable<boolean> {
    var model = new ModelId();
    model.Id = Id;
    return this.httpClient.post<boolean>(this.apiURL + 'ContentDataDeactivate', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  updateContentData(model): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'ContentDataUpdate', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  createContentData(model): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'ContentDataCreate', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  updateSiteLogo(fileToUpload: File): Observable<boolean> {
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    return this.httpClient.post<boolean>(this.apiURL + 'updateSiteLogo', formData).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  private handleError(err: HttpErrorResponse) {
    // in a real world app, we may send the server to some remote logging infrastructure
    // instead of just logging it to the console
    let errorMessage = '';
    if (err.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      errorMessage = `An error occurred: ${err.error.message}`;
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      errorMessage = `Server returned code: ${err.status}, error message is: ${err.message}`;
    }
    //console.error(errorMessage);
    return throwError(errorMessage);
  }
}
