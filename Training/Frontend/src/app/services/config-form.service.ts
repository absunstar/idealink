import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Constants } from '../constants';
import { ModelConfigForm } from '../interface/Model/ModelConfigForm.class';
import { Observable, throwError } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { ConfigForm } from '../Enum/ConfigForm.enum';
import { FieldConfig, Validator } from '../common/ConfigForms/field.interface';
import { ModelConfigFormGet } from '../interface/Model/ModelConfigFormGet.class';

@Injectable({
  providedIn: 'root'
})
export class ServiceConfigForm {
  
  protected apiURL = Constants.apiRoot;

  constructor(protected httpClient: HttpClient) {
    this.apiURL += "ConfigForm/";
  }
  GetByType(type: ConfigForm): Observable<FieldConfig[]> {
    var model = new ModelConfigFormGet();
    model.type = type;
    return this.httpClient.post<FieldConfig[]>(this.apiURL + 'GetByType', model).pipe(
      tap(data => {
        
        //console.log('All: ' + JSON.stringify(data));
      }),
      catchError(this.handleError)
    );
  }
  Update(model: ModelConfigForm): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'Update', model).pipe(
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
    //console.error(errorMessage);
    return throwError(errorMessage);
}
}
