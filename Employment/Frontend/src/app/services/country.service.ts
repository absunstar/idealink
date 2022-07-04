import { Injectable } from '@angular/core';
import { ServiceGeneric } from './GenericService.service';
import { cGenericIdNameItem, cGenericIdNameList } from '../interface/Response/GenericIdName.class';
import { ModelIdName } from '../interface/Model/ModelIdName.class';
import { HttpClient } from '@angular/common/http';
import { ModelSubEntity } from '../interface/Model/ModelSubEntity.class';
import { Observable } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ServiceCountry extends ServiceGeneric<cGenericIdNameItem,cGenericIdNameList,ModelIdName> {

  constructor(protected httpClient: HttpClient) {
    super("Country/", httpClient);
  }
  SubCreate(model: ModelSubEntity): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'SubCreate', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  SubUpdate(model: ModelSubEntity): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'SubUpdate', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  SubActivate(model: ModelSubEntity): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'SubActivate', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  SubDeActivate(model: ModelSubEntity): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'SubDeActivate', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
}
