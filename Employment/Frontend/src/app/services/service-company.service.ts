import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ModelCompany } from '../interface/Model/ModelCompany.class';
import { ServiceGeneric } from './GenericService.service';
import { cCompanyItem, cCompanyList } from '../interface/Response/Company.class';
import { Observable } from 'rxjs';
import { ModelId } from '../interface/Model/ModelId.interface';
import { tap, catchError } from 'rxjs/operators';
import { CompanyEmployers } from '../interface/Response/CompanyEmployer.class';
import { ModelCompanyAddEmployer } from '../interface/Model/ModelCompanyAddEmployer.class';
import { ModelPaging } from '../interface/Model/ModelPaging.interface';
import { ModelReportDates } from '../interface/Model/ModelReportDates.class';

@Injectable({
  providedIn: 'root'
})
export class ServiceCompany extends ServiceGeneric<cCompanyItem,cCompanyList,ModelCompany> {

  constructor(protected httpClient: HttpClient) {
    super("Company/", httpClient);
  }
  ListCompanyEmployers(Id: string): Observable<CompanyEmployers[]> {
    var model = new ModelId();
    model.Id = Id;
    return this.httpClient.post<CompanyEmployers[]>(this.apiURL + 'ListCompanyEmployers', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  } 
  
  AddEmployer(UserId: string, CompanyId: string): Observable<boolean> {
    var model = new ModelCompanyAddEmployer();
    model.UserId = UserId;
    model.CompanyId = CompanyId;
    return this.httpClient.post<boolean>(this.apiURL + 'AddEmployer', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  } 
  RemoveEmployer(UserId: string, CompanyId: string): Observable<boolean> {
    var model = new ModelCompanyAddEmployer();
    model.UserId = UserId;
    model.CompanyId = CompanyId;
    return this.httpClient.post<boolean>(this.apiURL + 'RemoveEmployer', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  } 
  GetCompanyWaitingApproval(currentPage: number, filterText: string): Observable<cCompanyList> {
    var model = new ModelPaging()
    model.CurrentPage = currentPage;
    model.filterText = filterText;
    return this.httpClient.post<cCompanyList>(this.apiURL + 'GetCompanyWaitingApproval', model).pipe(
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
  ListCompany(): Observable<CompanyEmployers> {
    return this.httpClient.get<CompanyEmployers>(this.apiURL + 'ListCompany').pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  ListAnyCompany(): Observable<CompanyEmployers> {
    return this.httpClient.get<CompanyEmployers>(this.apiURL + 'ListAnyCompany').pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  ListAnyCompanyPaged(currentPage: number, filterText: string): Observable<cCompanyList> {
    var model = new ModelPaging();
    model.CurrentPage = currentPage;
    model.filterText = filterText;
    return this.httpClient.post<cCompanyList>(this.apiURL + 'ListAnyCompanyPaged',model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  GetCompanyWaitingApprovalCount(): Observable<number> {
    return this.httpClient.get<number>(this.apiURL + 'GetCompanyWaitingApprovalCount').pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  ReportCompanyCount(model: ModelReportDates): Observable<number> {
   
    return this.httpClient.post<number>(this.apiURL + 'ReportCompanyCount', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
}
