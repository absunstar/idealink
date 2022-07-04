import { Injectable } from '@angular/core';
import { ServiceGeneric } from './GenericService.service';
import { cJobSeekerItem, cJobSeekerList, cResponseContactInformationRequestList, ResponseContactInformationRequest } from '../interface/Response/JobSeeker.class';
import { ModelJobSeeker, ModelResumeItem, ModelResumeCertification } from '../interface/Model/ModelJobSeeker.class';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { ModelId } from '../interface/Model/ModelId.interface';
import { ModelJobSeekerSearch } from '../interface/Model/ModelJobSeekerSearch.class';
import { ModelFileUpload } from '../interface/Model/ModelFileUpload.class';
import { ServiceFiles } from './files.service';
import { ModelPaging } from '../interface/Model/ModelPaging.interface';
import { ModelReportDates } from '../interface/Model/ModelReportDates.class';
import { ReportJobSeekerGender } from '../interface/Response/ReportJobSeekerGender.class';


@Injectable({
  providedIn: 'root'
})
export class ServiceJobSeeker extends ServiceGeneric<cJobSeekerItem, cJobSeekerList, ModelJobSeeker> {

  constructor(protected httpClient: HttpClient,
    protected BLServiceFiles: ServiceFiles) {
    super("JobSeeker/", httpClient);
  }
  UploadFile(fileName: string, type: number): Observable<boolean> {
    var model = new ModelFileUpload();
    model.FileName = fileName;
    model.type = type;
    return this.httpClient.post<boolean>(this.apiURL + 'UploadFile', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  UpdateInfo(model: ModelJobSeeker): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'UpdateInfo', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
 
  UpdateDescription(model: ModelJobSeeker): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'UpdateDescription', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  UpdateProfile(model: ModelJobSeeker): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'UpdateProfile', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  UpdateSocialMedia(model: ModelJobSeeker): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'UpdateSocialMedia', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  AddEducation(model: ModelResumeItem): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'AddEducation', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  UpdateEducation(model: ModelResumeItem): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'UpdateEducation', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  RemoveEducation(Id: string): Observable<boolean> {
    var model = new ModelId();
    model.Id = Id;
    return this.httpClient.post<boolean>(this.apiURL + 'RemoveEducation', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  AddWorkExperience(model: ModelResumeItem): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'AddWorkExperience', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  UpdateWorkExperience(model: ModelResumeItem): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'UpdateWorkExperience', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  RemoveWorkExperience(Id: string): Observable<boolean> {
    var model = new ModelId();
    model.Id = Id;
    return this.httpClient.post<boolean>(this.apiURL + 'RemoveWorkExperience', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  AddExtraCurricular(model: ModelResumeItem): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'AddExtraCurricular', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  UpdateExtraCurricular(model: ModelResumeItem): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'UpdateExtraCurricular', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  RemoveExtraCurricular(Id: string): Observable<boolean> {
    var model = new ModelId();
    model.Id = Id;
    return this.httpClient.post<boolean>(this.apiURL + 'RemoveExtraCurricular', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  AddCertification(model: ModelResumeCertification): Observable<boolean> {
    
      return this.httpClient.post<boolean>(this.apiURL + 'AddCertification', model).pipe(
        //tap(data => console.log('All: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  
  }
  UpdateCertification(model: ModelResumeCertification): Observable<boolean> {
   
    return this.httpClient.post<boolean>(this.apiURL + 'UpdateCertification', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  RemoveCertification(Id: string): Observable<boolean> {
    var model = new ModelId();
    model.Id = Id;
    return this.httpClient.post<boolean>(this.apiURL + 'RemoveCertification', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  Search(model: ModelJobSeekerSearch): Observable<cJobSeekerList> {
    // let headers = new HttpHeaders()
    // headers=headers.set('Authorization', `Bearer 'eyJhbGciOiJSUzI1NiIsImtpZCI6IjNENUZDQTJEOTg3MTI1QUU4NzZBQkI4QjdGQjhENTU4RjNBREE0NzciLCJ0eXAiOiJhdCtqd3QiLCJ4NXQiOiJQVl9LTFpoeEphNkhhcnVMZjdqVldQT3RwSGMifQ.eyJuYmYiOjE2Mjg1MTkwODAsImV4cCI6MTYyODUyOTA4MCwiaXNzIjoiaHR0cHM6Ly9lbXBsb3ltZW50c3RzLmlkZWFsYWtlLmNvbSIsImF1ZCI6InByb2plY3RzLWFwaSIsImNsaWVudF9pZCI6Ijg3YTEwYjg3LVhYWFgtOTQ1Ny0wODNlZDI1ZmFhYzUiLCJzdWIiOiI5ODk3ZjMwYi05NDE2LTRkMDAtYTNkOS01Yjk5MjIzMDM4OGMiLCJhdXRoX3RpbWUiOjE2MjgxNjIwNTcsImlkcCI6ImxvY2FsIiwiQXNwTmV0LklkZW50aXR5LlNlY3VyaXR5U3RhbXAiOiJWRlVWU1hGSEZPWTMySTVHNTRaWDIyWklFRUxBVEFRRCIsInJvbGUiOiJFbXBsb3llciIsInByZWZlcnJlZF91c2VybmFtZSI6InNvbmFsX2FnYXJ3YWxAaWRlYWxha2UuY29tIiwibmFtZSI6InNvbmFsX2FnYXJ3YWxAaWRlYWxha2UuY29tIiwiZW1haWwiOiJzb25hbF9hZ2Fyd2FsQGlkZWFsYWtlLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjp0cnVlLCJNRElEIjoiNjBkOTgyNDc3NTY3YmUxZmEwNDA2MjBlIiwic2NvcGUiOlsib3BlbmlkIiwicHJvZmlsZSIsInByb2plY3RzLWFwaSJdLCJhbXIiOlsicHdkIl19.YjJ7fqYrMpmsmrw7zCqPcCXrqdv38f0JivxbANDxXjkFbY-gKYqiPXzyIdD1-d7qm6WtWIGth-qALAZBK4l8lzMaiHFrrGRrynEt0GfRWFShZ9vvdxAw7-YKEt6oCq-OS_qRA2Wk2XaC2xlT7GAWAVTJ6gnMiM4Oap5rSIdNnCayTJ4f3v04PPXrUv-C4gxoJjezm0LbiUEHgP5PhXmDzIKpJpQqUtkpoQhPzcg7rjc8NYiKY07LoQ0zdAv4TFWge2ITVQQ2WPexfzhQeiHGUZB14u75gNas-z0DpSUBlpZqzuVvb_wjuHFa_WASkrZ6W0Eex3tlBc36rSvN05mZ7w'`);
    return this.httpClient.post<cJobSeekerList>(this.apiURL + 'Search', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }

  ContactPermissionRequest(model: ModelId): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'ContactPermissionRequest', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  ContactPermissionApprove(model: ModelId): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'ContactPermissionApprove', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  ContactPermissionReject(model: ModelId): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'ContactPermissionReject', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  ContactPermissioGetApprovalList(currentPage: number, filterText: string): Observable<cResponseContactInformationRequestList> {
    var model = new ModelPaging()
    model.CurrentPage = currentPage;
    model.filterText = filterText;
    return this.httpClient.post<cResponseContactInformationRequestList>(this.apiURL + 'ContactPermissioGetApprovalList', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  ReportJobSeekerCount(model: ModelReportDates): Observable<number> {
    return this.httpClient.post<number>(this.apiURL + 'ReportJobSeekerCount', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  ReportJobSeekerGenderCount(model: ModelReportDates): Observable<ReportJobSeekerGender> {
    return this.httpClient.post<ReportJobSeekerGender>(this.apiURL + 'ReportJobSeekerGenderCount', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  ImportCertification(): Observable<ReportJobSeekerGender> {
    return this.httpClient.get<ReportJobSeekerGender>(this.apiURL + 'ImportCertification').pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
}
