import { Injectable } from '@angular/core';
import { Constants } from '../constants';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { cQuestionItem, cQuestionList } from '../interface/Model/ModelQuestions.class';
import { ModelId } from '../interface/Model/ModelId.interface';
import { tap, catchError } from 'rxjs/operators';
import { ModelPaging } from '../interface/Model/ModelPaging.interface';
import { ModelFilterQuestions } from '../interface/Model/ModelFilterQuestions.class';

@Injectable({
  providedIn: 'root'
})
export class ServiceQuestion {

  private apiURL = Constants.apiRoot + 'Question/'

  constructor(private httpClient: HttpClient) { }
  //#region Question
  getQuestionGetByid(Id: string): Observable<cQuestionItem> {
    var model = new ModelId();
    model.Id = Id;
    return this.httpClient.post<cQuestionItem>(this.apiURL + 'QuestionGetById', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  getQuestionListActive(model: ModelFilterQuestions): Observable<cQuestionItem[]> {
    return this.httpClient.post<cQuestionItem[]>(this.apiURL + 'getQuestionListActive', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  getQuestionAll(model: ModelFilterQuestions): Observable<cQuestionList> {

    return this.httpClient.post<cQuestionList>(this.apiURL + 'QuestionListAll', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  setQuestionActivate(Id: string): Observable<boolean> {
    var model = new ModelId();
    model.Id = Id;
    return this.httpClient.post<boolean>(this.apiURL + 'QuestionActivate', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  setQuestionDeactivate(Id: string): Observable<boolean> {
    var model = new ModelId();
    model.Id = Id;
    return this.httpClient.post<boolean>(this.apiURL + 'QuestionDeactivate', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  updateQuestion(model): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'QuestionUpdate', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  createQuestion(model): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'QuestionCreate', model).pipe(
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
