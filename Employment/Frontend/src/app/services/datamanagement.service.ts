import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Observable,throwError } from 'rxjs';
import { tap, catchError, map } from 'rxjs/operators';
import { Constants } from '../constants';
import { ModelPaging } from '../interface/Model/ModelPaging.interface';
import { ModelId } from '../interface/Model/ModelId.interface';
import { cNGOTypeItem, cNGOTypeList } from '../interface/Response/NGOType.class';



@Injectable({providedIn: 'root'})
export class ServiceDataManagement {
    private apiURL = Constants.apiRoot + 'DataManagement/'
    
    constructor(private httpClient: HttpClient) { }
   
    //#region NGOType
    getNGOTypeGetByid(Id: string):Observable<cNGOTypeItem>{
      var model = new ModelId();
      model.Id = Id;
      return this.httpClient.post<cNGOTypeItem>(this.apiURL + 'NGOTypeGetById',model).pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
    }
    getNGOTypeAll(currentPage: number, filterText: string):Observable<cNGOTypeList>{
      
        var model = new ModelPaging()
        model.CurrentPage = currentPage;
        model.filterText = filterText;
        return this.httpClient.post<cNGOTypeList>(this.apiURL + 'NGOTypeListAll',model).pipe(
            //tap(data => console.log('All: ' + JSON.stringify(data))),
            catchError(this.handleError)
          );
    }
    setNGOTypeActivate(Id: string):Observable<boolean>{
      var model = new ModelId();
      model.Id = Id;
      return this.httpClient.post<boolean>(this.apiURL + 'NGOTypeActivate',model).pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
    }
    setNGOTypeDeActivate(Id: string):Observable<boolean>{
      var model = new ModelId();
      model.Id = Id;
      return this.httpClient.post<boolean>(this.apiURL + 'NGOTypeDeActivate',model).pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
    }
    updateNGOType(Id: string, Name:string):Observable<boolean>{
      var model = new cNGOTypeItem();
      model.Id = Id;
      model.Name = Name;
      return this.httpClient.post<boolean>(this.apiURL + 'NGOTypeUpdate',model).pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
    }
    createNGOType(Name:string):Observable<boolean>{
      var model = new cNGOTypeItem();
      model.Name = Name;
      return this.httpClient.post<boolean>(this.apiURL + 'NGOTypeCreate',model).pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
    }
    //#endregion
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
        console.error(errorMessage);
        return throwError(errorMessage);
      }
}