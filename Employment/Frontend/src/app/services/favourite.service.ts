import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { ModelId } from '../interface/Model/ModelId.interface';
import { tap, catchError } from 'rxjs/operators';
import { Constants } from '../constants';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { cFavouriteItem } from '../interface/Response/Favourite.class';

@Injectable({
  providedIn: 'root'
})
export class ServiceFavourite {
  
  protected apiURL = Constants.apiRoot+'Favourite/';

  constructor(protected httpClient: HttpClient) { }
  
  Create(Id: string): Observable<boolean> {
    var model = new ModelId();
    model.Id = Id;
    return this.httpClient.post<boolean>(this.apiURL + 'Create', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  ListAll(): Observable<cFavouriteItem[]> {
    return this.httpClient.get<cFavouriteItem[]>(this.apiURL + 'ListAll').pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  setDeActivate(Id: string): Observable<boolean> {
    var model = new ModelId();
    model.Id = Id;
    return this.httpClient.post<boolean>(this.apiURL + 'DeActivate', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  
  // DeActivateByProfileId(Id: string): Observable<boolean> {
  //   var model = new ModelId();
  //   model.Id = Id;
  //   return this.httpClient.post<boolean>(this.apiURL + 'DeActivateByProfileId', model).pipe(
  //     //tap(data => console.log('All: ' + JSON.stringify(data))),
  //     catchError(this.handleError)
  //   );
  // }

  DeActivateByJobId(Id: string): Observable<boolean> {
    var model = new ModelId();
    model.Id = Id;
    return this.httpClient.post<boolean>(this.apiURL + 'DeActivateByJobId', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  CheckMyFavourite(JobId: string): Observable<boolean> {
    var model = new ModelId();
    model.Id = JobId
    return this.httpClient.post<boolean>(this.apiURL + 'CheckMyFavourite', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  protected handleError(err: HttpErrorResponse) {
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
    console.error(errorMessage);
    return throwError(errorMessage);
}
}
