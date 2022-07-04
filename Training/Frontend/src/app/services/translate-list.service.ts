import { Injectable } from '@angular/core';
import { Constants } from '../constants';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { ModelTranslateList } from '../interface/Model/ModelTranslateList.class';
import { Observable, throwError } from 'rxjs';
import { TranslateList } from '../interface/Response/TranslateList.classl';
import { tap, catchError } from 'rxjs/operators';
import { TranslateType } from '../Enum/TranslateType.enum';

@Injectable({
  providedIn: 'root'
})
export class ServiceTranslateList {

  private apiURL = Constants.apiRoot + 'Translate/'

  constructor(private httpClient: HttpClient) { }
  
  ListTranslationByType(model: ModelTranslateList): Observable<TranslateList> {
    return this.httpClient.post<TranslateList>(this.apiURL + 'ListTranslationByType', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  SaveListTranslation(model: TranslateList): Observable<TranslateList> {
    return this.httpClient.post<TranslateList>(this.apiURL + 'SaveListTranslation', model).pipe(
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
