import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { ModelApplyList } from '../interface/Model/ModelApplyList.class';
import { Constants } from '../constants';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { cApplyList, ReportApply } from '../interface/Response/Apply.class';
import { ModelId } from '../interface/Model/ModelId.interface';
import { ModelApplyCreate } from '../interface/Model/ModelApplyCreate.class';
import { ModelApplyHire } from '../interface/Model/ModelApplyHire.class';
import { ModelReportDates } from '../interface/Model/ModelReportDates.class';

@Injectable({
  providedIn: 'root'
})
export class ServiceApply{
  protected apiURL = Constants.apiRoot + 'Apply/';

  constructor(protected httpClient: HttpClient) { 
    
  }
  Hire(JobSeekerId: string, JobId:string): Observable<boolean> {
    var model = new ModelApplyHire();
    model.Id = JobId;
    model.JobSeekerId = JobSeekerId;
    return this.httpClient.post<boolean>(this.apiURL + 'Hire', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  UnHire(JobSeekerId: string, JobId:string): Observable<boolean> {
    var model = new ModelApplyHire();
    model.Id = JobId;
    model.JobSeekerId = JobSeekerId;
    return this.httpClient.post<boolean>(this.apiURL + 'UnHire', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  Create(JobId: string, msg:string): Observable<boolean> {
    var model = new ModelApplyCreate();
    model.Id = JobId
    model.Message = msg;
    return this.httpClient.post<boolean>(this.apiURL + 'Create', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  ListAll(pageCurrent:number, filter: string, JobId: string = ''): Observable<cApplyList> {
    var model = new ModelApplyList();
    model.JobId = JobId;
    model.CurrentPage = pageCurrent;
    model.filterText = filter;
    return this.httpClient.post<cApplyList>(this.apiURL + 'ListAll', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  CheckMyApply(JobId: string): Observable<boolean> {
    var model = new ModelId();
    model.Id = JobId
    return this.httpClient.post<boolean>(this.apiURL + 'CheckMyApply', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  ReportJobSeekerHiredCount(model: ModelReportDates): Observable<number> {
   
    return this.httpClient.post<number>(this.apiURL + 'ReportJobSeekerHiredCount', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  ReportJobSeekerAppliedPerJobCount(model: ModelReportDates): Observable<ReportApply[]> {
   
    return this.httpClient.post<ReportApply[]>(this.apiURL + 'ReportJobSeekerAppliedPerJobCount', model).pipe(
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
