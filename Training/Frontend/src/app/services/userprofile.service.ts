import { Injectable } from '@angular/core';
import { ModelId } from '../interface/Model/ModelId.interface';
import { tap, catchError } from 'rxjs/operators';
import { cUserProfileItem, cUserProfileList, cUserProfileTrainerCertificateItem, cUserProfileTrainerCertificateItemWithProfile, cUserProfileTrainerCertificateList } from '../interface/Response/UserProfile.class';
import { Observable, throwError } from 'rxjs';
import { HttpErrorResponse, HttpClient } from '@angular/common/http';
import { Constants } from '../constants';
import { ModelPaging } from '../interface/Model/ModelPaging.interface';
import { ModelUserProfile } from '../interface/Model/ModelUserProfile.class';
import { ServiceGeneric } from './GenericService.service';
import { ModelAccountSearch } from '../interface/Model/ModelAccountSearch.class';
import { ModelMyAssignedAccount } from '../interface/Model/ModelMyAssignedAccount.class';
import { ModelTrainerCertificateApproval } from '../interface/Model/ModelTrainerCertificateApproval.class';
import { ModelChangeEmail } from '../interface/Model/ModelChangeEmail.class';

@Injectable({
  providedIn: 'root'
})
export class ServiceUserProfile extends ServiceGeneric<cUserProfileItem, cUserProfileList, ModelUserProfile> {
  constructor(protected httpClient: HttpClient) {
    super("AccountManagement/", httpClient);
  }
  getSearch(currentPage: number, filterText: string, filterType: string): Observable<cUserProfileList> {

    var model = new ModelAccountSearch()
    model.CurrentPage = currentPage;
    model.filterText = filterText;
    model.filterType = +filterType;

    return this.httpClient.post<cUserProfileList>(this.apiURL + 'ListAll', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  GetTrainerCertificate(currentPage: number, filterText: string): Observable<cUserProfileTrainerCertificateList> {

    var model = new ModelPaging()
    model.CurrentPage = currentPage;
    model.filterText = filterText;

    return this.httpClient.post<cUserProfileTrainerCertificateList>(this.apiURL + 'GetTrainerCertificate', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  UpdateUserEmail(model: ModelChangeEmail): Observable<boolean> {

    return this.httpClient.post<boolean>(this.apiURL + 'UpdateUserEmail', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  CertificateApprove(model: ModelTrainerCertificateApproval): Observable<boolean> {

    return this.httpClient.post<boolean>(this.apiURL + 'CertificateApprove', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  GetMyTrainersBySubPartnerId(subPartnerId: string): Observable<cUserProfileItem[]> {
    var model = new ModelId();
    model.Id = subPartnerId;
    return this.httpClient.post<cUserProfileItem[]>(this.apiURL + 'GetMyTrainersBySubPartnerId', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  GetMyTrainers(): Observable<cUserProfileItem[]> {
    return this.httpClient.get<cUserProfileItem[]>(this.apiURL + 'GetMyTrainers').pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  GetPartnerSearch(filterText: string, currentPage: number = 1): Observable<cUserProfileItem[]> {
    var model = new ModelPaging()
    model.CurrentPage = currentPage;
    model.filterText = filterText;
    return this.httpClient.post<cUserProfileItem[]>(this.apiURL + 'GetPartnerSearch', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  GetSubPartnerSearch(filterText: string, currentPage: number = 1): Observable<cUserProfileItem[]> {
    var model = new ModelPaging()
    model.CurrentPage = currentPage;
    model.filterText = filterText;
    return this.httpClient.post<cUserProfileItem[]>(this.apiURL + 'GetSubPartnerSearch', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  AddAccountToMyAssignedToAccount(UserId: string, AccountId, type: number): Observable<boolean> {
    var model = new ModelMyAssignedAccount()
    model.UserId = UserId;
    model.AccountId = AccountId;
    model.Type = type;
    return this.httpClient.post<boolean>(this.apiURL + 'AddAccountToMyAssignedToAccount', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  RemoveAccountToMyAssignedToAccount(UserId: string, AccountId: string, type: number): Observable<boolean> {
    var model = new ModelMyAssignedAccount()
    model.UserId = UserId;
    model.AccountId = AccountId;
    model.Type = type;
    return this.httpClient.post<boolean>(this.apiURL + 'RemoveAccountToMyAssignedToAccount', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  ResendActivationLink(Email: string): Observable<boolean> {
    var model = new ModelUserProfile();
    model.Email = Email;
    return this.httpClient.post<boolean>(this.apiURL + 'ResendActivationLink', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  ResendPasswordLink(Email: string): Observable<boolean> {
    var model = new ModelUserProfile();
    model.Email = Email;
    return this.httpClient.post<boolean>(this.apiURL + 'ResendPasswordLink', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  MyTrainerCertificate(Id: string): Observable<cUserProfileTrainerCertificateItemWithProfile> {
    var model = new ModelId();
    model.Id = Id;
    return this.httpClient.post<cUserProfileTrainerCertificateItemWithProfile>(this.apiURL + 'MyTrainerCertificate', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
}