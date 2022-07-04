import { Injectable } from '@angular/core';
import { Constants } from '../constants';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import {  cExamQuestionList, cQuestionItemTemplate } from '../interface/Model/ModelQuestions.class';
import { ModelId } from '../interface/Model/ModelId.interface';
import { tap, catchError } from 'rxjs/operators';
import { ModelPaging } from '../interface/Model/ModelPaging.interface';

@Injectable({
  providedIn: 'root'
})
export class ServiceExam {

  private apiURL = Constants.apiRoot + 'Exam/'

  constructor(private httpClient: HttpClient) { }
  takeExam(Id: string): Observable<cExamQuestionList> {
    var model = new ModelId();
    model.Id = Id;
    return this.httpClient.post<cExamQuestionList>(this.apiURL + 'TakeExam', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }

  SubmitExam(examId:string, questions:cQuestionItemTemplate[]){
    var model = new cExamQuestionList();
    model.questions = questions;
    model.ExamId = examId;
    return this.httpClient.post<boolean>(this.apiURL + 'SubmitExam', model).pipe(
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
