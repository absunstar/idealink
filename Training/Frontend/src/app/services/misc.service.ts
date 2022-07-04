import { Injectable } from '@angular/core';
import { Constants } from '../constants';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { cQuestionItem } from '../interface/Model/ModelQuestions.class';
import { ModelId } from '../interface/Model/ModelId.interface';
import { tap, catchError } from 'rxjs/operators';
import { cContentDataItem } from '../interface/Response/ContentData.class';
import { cEntityPartnerItem } from '../interface/Response/EntityPartner.class';
import { ConfigForm } from '../Enum/ConfigForm.enum';
import { FieldConfig } from '../common/ConfigForms/field.interface';
import { ModelConfigFormGet } from '../interface/Model/ModelConfigFormGet.class';
import { StatsCount } from '../interface/Response/StatsCount.class';
import { ModelLanguage } from '../interface/Model/ModelLanguage.class';
import { LanguageType } from '../Enum/LanguageType.enum';
import { cLogoPartnerItem } from '../interface/Response/LogoPartner.class';

@Injectable({
  providedIn: 'root'
})
export class ServiceMisc {

  private apiURL = Constants.apiRoot + 'Misc/'

  constructor(private httpClient: HttpClient) { }
  SetLanguage(strLang: string): Observable<cQuestionItem> {
    var model = new ModelLanguage()
    switch (strLang){
      case "en" : model.Lang = LanguageType.English;
      break;
      case "ar" : model.Lang = LanguageType.Arabic;
      break;
      case "fr" : model.Lang = LanguageType.French;
      break;
    }
    return this.httpClient.post<cQuestionItem>(this.apiURL + 'SetLanguage', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  RequestRegister(model): Observable<cQuestionItem> {
    return this.httpClient.post<cQuestionItem>(this.apiURL + 'RequestRegister', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  ContentDataOneGetByTypeId(type): Observable<cContentDataItem> {
    var model = new ModelId();
    model.Id = <string>type;
    return this.httpClient.post<cContentDataItem>(this.apiURL + 'ContentDataOneGetByTypeId', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  getEntityPartnerListAllActiveAnonymous(): Observable<cEntityPartnerItem[]> {
    return this.httpClient.get<cEntityPartnerItem[]>(this.apiURL + 'EntityPartnerListAllActiveAnonymous').pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
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
  GetStats(): Observable<StatsCount> {
    return this.httpClient.get<StatsCount>(this.apiURL + 'GetStats').pipe(
      tap(data => {

        //console.log('All: ' + JSON.stringify(data));
      }),
      catchError(this.handleError)
    );
  }
  getLogoPartnerListActive(): Observable<cLogoPartnerItem[]> {
    return this.httpClient.get<cLogoPartnerItem[]>(this.apiURL + 'LogoPartnerListActive').pipe(
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
