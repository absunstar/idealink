import { Injectable } from '@angular/core';
import { Constants } from '../constants';
import { HttpClient, HttpErrorResponse, HttpEventType } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { cExamQuestionList, cQuestionItemTemplate } from '../interface/Model/ModelQuestions.class';
import { ModelId } from '../interface/Model/ModelId.interface';
import { tap, catchError } from 'rxjs/operators';
import { ModelPaging } from '../interface/Model/ModelPaging.interface';
import { ResponseFileUpload } from '../interface/Model/ModelUploadFile.class';
import { cCertificateList } from '../interface/Response/Certificate.class';
import { cTrainingTypeList } from '../interface/Response/TrainingType.class';

@Injectable({
  providedIn: 'root'
})
export class ServiceCertificate {

  private apiURL = Constants.apiRoot + 'Certificate/'

  constructor(private httpClient: HttpClient) { }
  CertificateListAllByTrainingCenterId(currentPage: number, filterText: string): Observable<cCertificateList> {
    var model = new ModelPaging()
    model.CurrentPage = currentPage;
    model.filterText = filterText;
    return this.httpClient.post<cCertificateList>(this.apiURL + 'CertificateListAllByTrainingCenterId', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  CertificateActivate(Id: string):Observable<boolean>{
    var model = new ModelId();
    model.Id = Id;
    return this.httpClient.post<boolean>(this.apiURL + 'CertificateActivate',model).pipe(
        //tap(data => console.log('All: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }
  CertificateDeActivate(Id: string):Observable<boolean>{
    var model = new ModelId();
    model.Id = Id;
    return this.httpClient.post<boolean>(this.apiURL + 'CertificateDeActivate',model).pipe(
        //tap(data => console.log('All: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }
  CertificateListAllByPartnerId(currentPage: number, filterText: string): Observable<cCertificateList> {
    var model = new ModelPaging()
    model.CurrentPage = currentPage;
    model.filterText = filterText;
    return this.httpClient.post<cCertificateList>(this.apiURL + 'CertificateListAllByPartnerId', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  CertificateListAllGenericByPartnerId(currentPage: number, filterText: string): Observable<cCertificateList> {
    var model = new ModelPaging()
    model.CurrentPage = currentPage;
    model.filterText = filterText;
    return this.httpClient.post<cCertificateList>(this.apiURL + 'CertificateListAllGenericByPartnerId', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  CertificateListAllSystemGeneric(): Observable<cCertificateList> {
    return this.httpClient.get<cCertificateList>(this.apiURL + 'CertificateListAllSystemGeneric').pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  uploadFile(fileToUpload: File, fileType: number, partnerId?: string): Observable<ResponseFileUpload> {
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);

    if (partnerId) {
      return this.httpClient.post<ResponseFileUpload>(this.apiURL + 'UploadSystemGenericFile?fileType=' + fileType + '&partnerId=' + partnerId, formData).pipe(
        //tap(data => console.log('All: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
    }
    else {
      return this.httpClient.post<ResponseFileUpload>(this.apiURL + 'UploadSystemGenericFile?fileType=' + fileType, formData).pipe(
        //tap(data => console.log('All: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );

    }
  }

  uploadCategoryCertificateFile(fileToUpload: File, fileType: number, partnerId: string, trainingTypeId: string, trainingCategoryId: string): Observable<ResponseFileUpload> {
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);

    return this.httpClient.post<ResponseFileUpload>(this.apiURL + 'UploadCertificateCategoryFile?fileType=' + fileType + '&partnerId=' + partnerId + '&trainingTypeId=' + trainingTypeId + '&trainingCategoryId=' + trainingCategoryId, formData).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  uploadTrainingCenterCertificateFile(fileToUpload: File, fileType: number, partnerId: string, trainingCenterId: string, trainingTypeId: string, trainingCategoryId: string): Observable<ResponseFileUpload> {
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);

    return this.httpClient.post<ResponseFileUpload>(this.apiURL + 'UploadCertificateTrainingCenterFile?fileType=' + fileType + '&partnerId=' + partnerId + '&trainingCenterId=' + trainingCenterId + '&trainingTypeId=' + trainingTypeId + '&trainingCategoryId=' + trainingCategoryId, formData).pipe(
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
