import { Injectable } from '@angular/core';
import { ServiceGeneric } from './GenericService.service';
import { cJobItem, cJobList, ReportJobCount } from '../interface/Response/Job.class';
import { ModelJob } from '../interface/Model/ModelJob.class';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { ModelPaging } from '../interface/Model/ModelPaging.interface';
import { ModelJobSearch } from '../interface/Model/ModelJobSearch.class';
import { ModelId } from '../interface/Model/ModelId.interface';
import { JobStats } from '../interface/Response/JobStats.class';
import { ModelAdminJobSearch } from '../interface/Model/ModelJobFilter.class';
import { ModelReportJob } from '../interface/Model/ModelReportDates.class';

@Injectable({
  providedIn: 'root'
})
export class ServiceJob extends ServiceGeneric<cJobItem, cJobList, ModelJob> {
  searchSer(param: { filterText: "Market"; }) {
    throw new Error('Method not implemented.');
  }

  constructor(protected httpClient: HttpClient) {
    super("Job/", httpClient);
  }
  GetJobsByCompanyId(Id: string): Observable<cJobItem[]> {
    var model = new ModelId();
    model.Id = Id
    return this.httpClient.post<cJobItem[]>(this.apiURL + 'GetJobsByCompanyId', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  GetMyJobStats(): Observable<JobStats> {
    return this.httpClient.get<JobStats>(this.apiURL + 'GetMyJobStats').pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  updateDraft(model: ModelJob): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'updateDraft', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  updateDraftPublish(model: ModelJob): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'updateDraftPublish', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  updatePublish(model: ModelJob): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'updatePublish', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  ListAllByEmployerId(currentPage: number, filterText: string): Observable<cJobList> {
    var model = new ModelPaging()
    model.CurrentPage = currentPage;
    model.filterText = filterText;
    return this.httpClient.post<cJobList>(this.apiURL + 'ListAllByEmployerId', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  Search(model: ModelJobSearch): Observable<cJobList> {
    return this.httpClient.post<cJobList>(this.apiURL + 'Search', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  setApproved(Id: string): Observable<boolean> {
    var model = new ModelId();
    model.Id = Id;
    return this.httpClient.post<boolean>(this.apiURL + 'updateApproved', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  setRejected(Id: string): Observable<boolean> {
    var model = new ModelId();
    model.Id = Id;
    return this.httpClient.post<boolean>(this.apiURL + 'updateRejected', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  GetJobWaitingApproval(currentPage: number, filterText: string): Observable<cJobList> {
    var model = new ModelPaging()
    model.CurrentPage = currentPage;
    model.filterText = filterText;
    return this.httpClient.post<cJobList>(this.apiURL + 'GetJobWaitingApproval', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  AdminJobSearch(model: ModelAdminJobSearch): Observable<cJobList> {
    return this.httpClient.post<cJobList>(this.apiURL + 'AdminJobSearch', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  GetJobWaitingApprovalCount(): Observable<number> {
    return this.httpClient.get<number>(this.apiURL + 'GetJobWaitingApprovalCount').pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  ReportJobCount(model: ModelReportJob): Observable<ReportJobCount[]> {
   
    return this.httpClient.post<ReportJobCount[]>(this.apiURL + 'ReportJobCount', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
}
