import { Injectable } from '@angular/core';
import { Constants } from '../constants';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { ModelId } from '../interface/Model/ModelId.interface';
import { cEntityPartnerItem, cEntityPartnerList } from '../interface/Response/EntityPartner.class';
import { ModelPaging } from '../interface/Model/ModelPaging.interface';
import { cEntityTrainingCenterItem, cEntityTrainingCenterList } from '../interface/Response/EntityTrainingCenter.class';
import { ModelEntityTrainingCenter } from '../interface/Model/ModelEntityTrainingCenter.class';
import { cEntitySubPartnerItem, cEntitySubPartnerList } from '../interface/Response/EntitySubPartner.class';
import { ModelEntitySubPartner } from '../interface/Model/ModelEntitySubPartner.class';
import { ModelEntityMember } from '../interface/Model/ModelEntityMember.class';
import { ModelEntitySubEntityIds } from '../interface/Model/ModelEntitySubEntityIds.class';
import { cEntityPartnerReportsItem, cEntityPartnerReportsList } from '../interface/Response/PartnerReports.class';

@Injectable({ providedIn: 'root' })
export class ServiceEntityManagement {
  private apiURL = Constants.apiRoot + 'EntityManagement/'

  constructor(private httpClient: HttpClient) { }
  //#region EntityPartner
  getEntityPartnerGetByid(Id: string): Observable<cEntityPartnerItem> {
    var model = new ModelId();
    model.Id = Id;
    return this.httpClient.post<cEntityPartnerItem>(this.apiURL + 'EntityPartnerGetById', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  
  getEntityPartnerListActive(query: string = ""): Observable<cEntityPartnerItem[]> {
    return this.httpClient.post<cEntityPartnerItem[]>(this.apiURL + 'EntityPartnerListActive', query).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  EntityPartnerReportListAll(currentPage: number, filterText: string): Observable<cEntityPartnerReportsList> {
    var model = new ModelPaging()
    model.CurrentPage = currentPage;
    model.filterText = filterText;
    return this.httpClient.post<cEntityPartnerReportsList>(this.apiURL + 'EntityPartnerReportListAll', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  getEntityPartnerAll(currentPage: number, filterText: string): Observable<cEntityPartnerList> {
    var model = new ModelPaging()
    model.CurrentPage = currentPage;
    model.filterText = filterText;
    return this.httpClient.post<cEntityPartnerList>(this.apiURL + 'EntityPartnerListAll', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  getEntityPartnerGetMy(): Observable<cEntityPartnerItem[]> {
    return this.httpClient.get<cEntityPartnerItem[]>(this.apiURL + 'EntityPartnerGetMy').pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  setEntityPartnerActivate(Id: string): Observable<boolean> {
    var model = new ModelId();
    model.Id = Id;
    return this.httpClient.post<boolean>(this.apiURL + 'EntityPartnerActivate', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  setEntityPartnerDeactivate(Id: string): Observable<boolean> {
    var model = new ModelId();
    model.Id = Id;
    return this.httpClient.post<boolean>(this.apiURL + 'EntityPartnerDeactivate', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  updateEntityPartner(model: cEntityPartnerItem): Observable<boolean> {
    // var model = new cEntityPartnerItem();
    // model.Id = Id;
    // model.Name = Name;
    return this.httpClient.post<boolean>(this.apiURL + 'EntityPartnerUpdate', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  createEntityPartner(model: cEntityPartnerItem): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'EntityPartnerCreate', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  memberAddEntityPartner(PartnerId: string, UserId: string): Observable<boolean> {
    var model = new ModelEntityMember();
    model.EntityId = PartnerId;
    model.UserId = UserId;
    return this.httpClient.post<boolean>(this.apiURL + 'EntityPartnerAddMember', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  memberRemoveEntityPartner(PartnerId: string, UserId: string): Observable<boolean> {
    var model = new ModelEntityMember();
    model.EntityId = PartnerId;
    model.UserId = UserId;
    return this.httpClient.post<boolean>(this.apiURL + 'EntityPartnerRemoveMember', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  //#endregion
  //#region EntitySubPartner
  getEntitySubPartnerGetByid(Id: string): Observable<cEntitySubPartnerItem> {
    var model = new ModelId();
    model.Id = Id;
    return this.httpClient.post<cEntitySubPartnerItem>(this.apiURL + 'EntitySubPartnerGetById', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  getEntitySubPartnerListActive(query: string = "", PartnerIds: string[] = null): Observable<cEntitySubPartnerItem[]> {
    return this.httpClient.post<cEntitySubPartnerItem[]>(this.apiURL + 'EntitySubPartnerListActive', { "query": query, "PartnerIds": PartnerIds }).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  getEntitySubPartnerAll(currentPage: number, filterText: string): Observable<cEntitySubPartnerList> {

    var model = new ModelPaging()
    model.CurrentPage = currentPage;
    model.filterText = filterText;
    return this.httpClient.post<cEntitySubPartnerList>(this.apiURL + 'EntitySubPartnerListAll', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  getEntitySubPartnerGetMyByPartnerId(partnerId: string): Observable<cEntitySubPartnerItem[]> {
    var model = new ModelId();
    model.Id = partnerId;
    return this.httpClient.post<cEntitySubPartnerItem[]>(this.apiURL + 'EntitySubPartnerGetMyByPartnerId', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  getEntitySubPartnerGetMy(): Observable<cEntitySubPartnerItem[]> {
    return this.httpClient.get<cEntitySubPartnerItem[]>(this.apiURL + 'EntitySubPartnerGetMy').pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  setEntitySubPartnerActivate(Id: string): Observable<boolean> {
    var model = new ModelId();
    model.Id = Id;
    return this.httpClient.post<boolean>(this.apiURL + 'EntitySubPartnerActivate', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  setEntitySubPartnerDeactivate(Id: string): Observable<boolean> {
    var model = new ModelId();
    model.Id = Id;
    return this.httpClient.post<boolean>(this.apiURL + 'EntitySubPartnerDeactivate', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  updateEntitySubPartner(model: ModelEntitySubPartner): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'EntitySubPartnerUpdate', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  createEntitySubPartner(model: ModelEntitySubPartner): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'EntitySubPartnerCreate', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  memberAddEntitySubPartner(PartnerId: string, UserId: string): Observable<boolean> {
    var model = new ModelEntityMember();
    model.EntityId = PartnerId;
    model.UserId = UserId;
    return this.httpClient.post<boolean>(this.apiURL + 'EntitySubPartnerAddMember', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  memberRemoveEntitySubPartner(PartnerId: string, UserId: string): Observable<boolean> {
    var model = new ModelEntityMember();
    model.EntityId = PartnerId;
    model.UserId = UserId;
    return this.httpClient.post<boolean>(this.apiURL + 'EntitySubPartnerRemoveMember', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  AddPartnerEntityToEntitySubPartner(PartnerId: string, SubPartnerId: string): Observable<boolean> {
    var model = new ModelEntityMember();
    //UserId is Partner Id, EntityId Is subparnterId
    model.EntityId = SubPartnerId;
    model.UserId = PartnerId;
    return this.httpClient.post<boolean>(this.apiURL + 'EntitySubPartnerAddEntityPartner', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  RemovePartnerEntityToEntitySubPartner(PartnerId: string, SubPartnerId: string): Observable<boolean> {
    var model = new ModelEntityMember();
    //UserId is Partner Id, EntityId Is subparnterId
    model.EntityId = SubPartnerId;
    model.UserId = PartnerId;
    return this.httpClient.post<boolean>(this.apiURL + 'EntitySubPartnerRemoveEntityPartner', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  //#endregion
  //#region EntityTrainingCenter
  // getEntityTrainingCenterGetByid(Id: string):Observable<cEntityTrainingCenterItem>{
  //   var model = new ModelId();
  //   model.Id = Id;
  //   return this.httpClient.post<cEntityTrainingCenterItem>(this.apiURL + 'EntityTrainingCenterGetById',model).pipe(
  //       //tap(data => console.log('All: ' + JSON.stringify(data))),
  //       catchError(this.handleError)
  //     );
  // }
  // getEntityTrainingCenterListActive():Observable<cEntityTrainingCenterItem[]>{
  //     return this.httpClient.get<cEntityTrainingCenterItem[]>(this.apiURL + 'EntityTrainingCenterListActive').pipe(
  //         //tap(data => console.log('All: ' + JSON.stringify(data))),
  //         catchError(this.handleError)
  //       );
  //   }
  //   getEntityTrainingCenterAll(currentPage: number, filterText: string):Observable<cEntityTrainingCenterList>{

  //       var model = new ModelPaging()
  //       model.CurrentPage = currentPage;
  //       model.filterText = filterText;
  //       return this.httpClient.post<cEntityTrainingCenterList>(this.apiURL + 'EntityTrainingCenterListAll',model).pipe(
  //           //tap(data => console.log('All: ' + JSON.stringify(data))),
  //           catchError(this.handleError)
  //         );
  //   }
  setEntityTrainingCenterActivate(PartnerId: string, Id: string): Observable<boolean> {
    var model = new ModelEntitySubEntityIds();
    model.MainEntityId = PartnerId;
    model.SubEntityId = Id;
    return this.httpClient.post<boolean>(this.apiURL + 'EntityTrainingCenterActivate', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  setEntityTrainingCenterDeactivate(PartnerId: string, Id: string): Observable<boolean> {
    var model = new ModelEntitySubEntityIds();
    model.MainEntityId = PartnerId;
    model.SubEntityId = Id;
    return this.httpClient.post<boolean>(this.apiURL + 'EntityTrainingCenterDeactivate', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  updateEntityTrainingCenter(model: ModelEntityTrainingCenter): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'EntityTrainingCenterUpdate', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  createEntityTrainingCenter(model: ModelEntityTrainingCenter): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'EntityTrainingCenterCreate', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  AddEntityTrainingCenterToSubPartner(TrainginCenterId: string, SubPartnerId: string): Observable<boolean> {
    var model = new ModelEntitySubEntityIds();
    model.MainEntityId = TrainginCenterId;
    model.SubEntityId = SubPartnerId;
    return this.httpClient.post<boolean>(this.apiURL + 'AddEntityTrainingCenterToSubPartner', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  RemoveEntityTrainingCenterToSubPartner(TrainginCenterId: string, SubPartnerId: string): Observable<boolean> {
    var model = new ModelEntitySubEntityIds();
    model.MainEntityId = TrainginCenterId;
    model.SubEntityId = SubPartnerId;
    return this.httpClient.post<boolean>(this.apiURL + 'RemoveEntityTrainingCenterToSubPartner', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  AddEntityTrainingCenterToSubPartnerByPartnerID(PartnerId: string, SubPartnerId: string): Observable<boolean> {
    var model = new ModelEntitySubEntityIds();
    model.MainEntityId = PartnerId;
    model.SubEntityId = SubPartnerId;
    return this.httpClient.post<boolean>(this.apiURL + 'AddEntityTrainingCenterToSubPartnerByPartnerID', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  RemoveEntityTrainingCenterToSubPartnerByPartnerID(PartnerId: string, SubPartnerId: string): Observable<boolean> {
    var model = new ModelEntitySubEntityIds();
    model.MainEntityId = PartnerId;
    model.SubEntityId = SubPartnerId;
    return this.httpClient.post<boolean>(this.apiURL + 'RemoveEntityTrainingCenterToSubPartnerByPartnerID', model).pipe(
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
    //console.error(errorMessage);
    return throwError(errorMessage);
  }
}