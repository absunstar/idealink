import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Constants } from '../constants';
import { ModelId } from '../interface/Model/ModelId.interface';
import { ModelPaging } from '../interface/Model/ModelPaging.interface';
import { cLogoPartnerItem, cLogoPartnerList } from '../interface/Response/LogoPartner.class';

@Injectable({
  providedIn: 'root'
})
export class ServiceLogoPartner {

  private apiURL = Constants.apiRoot + 'LogoPartner/'

  constructor(private httpClient: HttpClient) { }
  getLogoPartnerGetByid(Id: string): Observable<cLogoPartnerItem> {
    var model = new ModelId();
    model.Id = Id;
    return this.httpClient.post<cLogoPartnerItem>(this.apiURL + 'LogoPartnerGetById', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  getLogoPartnerListActive(): Observable<cLogoPartnerItem[]> {
    return this.httpClient.get<cLogoPartnerItem[]>(this.apiURL + 'LogoPartnerListActive').pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  getLogoPartnerAll(currentPage: number, filterText: string): Observable<cLogoPartnerList> {

    var model = new ModelPaging()
    model.CurrentPage = currentPage;
    model.filterText = filterText;
    return this.httpClient.post<cLogoPartnerList>(this.apiURL + 'LogoPartnerListAll', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  setLogoPartnerActivate(Id: string): Observable<boolean> {
    var model = new ModelId();
    model.Id = Id;
    return this.httpClient.post<boolean>(this.apiURL + 'LogoPartnerActivate', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  setLogoPartnerDeactivate(Id: string): Observable<boolean> {
    var model = new ModelId();
    model.Id = Id;
    return this.httpClient.post<boolean>(this.apiURL + 'LogoPartnerDeactivate', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  createLogoPartner(WebsiteURL: string, fileToUpload: File): Observable<boolean> {
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);

    return this.httpClient.post<boolean>(this.apiURL + 'LogoPartnerCreate?WebsiteURL=' + WebsiteURL , formData).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  updateLogoPartner(Id: string, WebsiteURL: string, fileToUpload: File): Observable<boolean> {
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    return this.httpClient.post<boolean>(this.apiURL + 'LogoPartnerUpdate?WebsiteURL=' + WebsiteURL + "&Id=" + Id, formData).pipe(
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
