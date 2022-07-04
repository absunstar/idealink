import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Constants } from '../constants';
import { tap, catchError } from 'rxjs/operators';
import { ModelId } from '../interface/Model/ModelId.interface';
import { Observable, throwError } from 'rxjs';
import { ModelPaging } from '../interface/Model/ModelPaging.interface';

export class ServiceGeneric<T, W, Z> {
    protected apiURL = Constants.apiRoot;
    
    constructor(ControllerName: string,
        protected httpClient: HttpClient) {
        this.apiURL += ControllerName;
    }
    getGetByid(Id: string): Observable<T> {
        var model = new ModelId();
        model.Id = Id;
        return this.httpClient.post<T>(this.apiURL + 'GetById', model).pipe(
            //tap(data => console.log('All: ' + JSON.stringify(data))),
            catchError(this.handleError)
        );
    }
    getListActive(): Observable<T[]> {
        return this.httpClient.get<T[]>(this.apiURL + 'ListActive').pipe(
            // tap(data => console.log('All: ' + JSON.stringify(data))),
            catchError(this.handleError)
        );
    }
    getAll(currentPage: number, filterText: string): Observable<W> {

        var model = new ModelPaging()
        model.CurrentPage = currentPage;
        model.filterText = filterText;
        return this.httpClient.post<W>(this.apiURL + 'ListAll', model).pipe(
            //tap(data => console.log('All: ' + JSON.stringify(data))),
            catchError(this.handleError)
        );
    }
    setActivate(Id: string): Observable<boolean> {
        var model = new ModelId();
        model.Id = Id;
        return this.httpClient.post<boolean>(this.apiURL + 'Activate', model).pipe(
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
    update(model: Z): Observable<boolean> {
        return this.httpClient.post<boolean>(this.apiURL + 'Update', model).pipe(
            //tap(data => console.log('All: ' + JSON.stringify(data))),
            catchError(this.handleError)
        );
    }
    create(model: Z): Observable<boolean> {
        return this.httpClient.post<boolean>(this.apiURL + 'Create', model).pipe(
            //tap(data => console.log('All: ' + JSON.stringify(data))),
            catchError(this.handleError)
        );
    }
    //#endregion
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