import { Injectable } from '@angular/core';
import { ServiceGeneric } from './GenericService.service';
import { cJobFairItem, cJobFairList } from '../interface/Response/JobFair.class';
import { ModelJobFair } from '../interface/Model/ModelJobFair.class';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ModelPaging } from '../interface/Model/ModelPaging.interface';
import { tap, catchError } from 'rxjs/operators';
import { ModelId } from '../interface/Model/ModelId.interface';
import { ModelJobFairRegisteration } from '../interface/Model/ModelJobFairRegisteration.class';
import { Constants } from '../constants';
import { ModelJobFairAttendance } from '../interface/Model/ModelJobFairAttendance.class';

@Injectable({
  providedIn: 'root'
})
export class ServiceJobFair extends ServiceGeneric<cJobFairItem, cJobFairList, ModelJobFair> {


  constructor(protected httpClient: HttpClient) {
    super("JobFair/", httpClient);
  }
  Search(currentPage: number, filterText: string): Observable<cJobFairList> {

    var model = new ModelPaging()
    model.CurrentPage = currentPage;
    model.filterText = filterText;
    return this.httpClient.post<cJobFairList>(this.apiURL + 'Search', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  CheckRegister(JobFairId: string): Observable<boolean> {

    var model = new ModelId()
    model.Id = JobFairId;
    return this.httpClient.post<boolean>(this.apiURL + 'CheckRegister', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  Register(model: ModelJobFairRegisteration): Observable<cJobFairList> {
    return this.httpClient.post<cJobFairList>(this.apiURL + 'Register', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  SetAttendance(model: ModelJobFairAttendance): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'SetAttendance', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  Export(JobFairId: string): Observable<string> {

    var model = new ModelId()
    model.Id = JobFairId;
    return this.httpClient.post<string>(this.apiURL + 'ExportRegisteredUser', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
}
