import { Injectable } from '@angular/core';
import { Constants } from '../constants';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { cExamTemplateItem, cExamTemplateList } from '../interface/Response/ExamTemplate.class';
import { ModelId } from '../interface/Model/ModelId.interface';
import { tap, catchError } from 'rxjs/operators';
import { ModelPaging } from '../interface/Model/ModelPaging.interface';

@Injectable({
  providedIn: 'root'
})
export class ServiceExamTemplate {

  private apiURL = Constants.apiRoot + 'ExamTemplate/'

  constructor(private httpClient: HttpClient) { }
  //#region ExamTemplate
  getExamTemplateGetByid(Id: string): Observable<cExamTemplateItem> {
    var model = new ModelId();
    model.Id = Id;
    return this.httpClient.post<cExamTemplateItem>(this.apiURL + 'ExamTemplateGetById', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  ExamTemplateListActive(): Observable<cExamTemplateItem[]> {
    
    return this.httpClient.get<cExamTemplateItem[]>(this.apiURL + 'ExamTemplateListActive').pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  getExamTemplateAll(currentPage: number, filterText: string): Observable<cExamTemplateList> {
    var model = new ModelPaging()
    model.CurrentPage = currentPage;
    model.filterText = filterText;
    
    return this.httpClient.post<cExamTemplateList>(this.apiURL + 'ExamTemplateListAll', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  setExamTemplateActivate(Id: string): Observable<boolean> {
    var model = new ModelId();
    model.Id = Id;
    return this.httpClient.post<boolean>(this.apiURL + 'ExamTemplateActivate', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  setExamTemplateDeactivate(Id: string): Observable<boolean> {
    var model = new ModelId();
    model.Id = Id;
    return this.httpClient.post<boolean>(this.apiURL + 'ExamTemplateDeactivate', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  updateExamTemplate(model): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'ExamTemplateUpdate', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  createExamTemplate(model): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'ExamTemplateCreate', model).pipe(
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
