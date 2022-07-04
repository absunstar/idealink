import { Injectable } from '@angular/core';
import { ServiceGeneric } from './GenericService.service';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { cTraineeItem, cTraineeList } from '../interface/Response/Trainee.class';
import { ModelTrainee } from '../interface/Model/ModelTrainee.class';
import { ModelTraineeTraining } from '../interface/Model/ModelTraineeTraining.class';
import { Observable } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { ModelPaging } from '../interface/Model/ModelPaging.interface';
import { MyTraining } from '../interface/Response/MyTraining.class';
import { ModelId } from '../interface/Model/ModelId.interface';
import { ModelUserProfile } from '../interface/Model/ModelUserProfile.class';
import { ImportTrainee } from '../interface/Response/ImportTrainee.class';
//import { saveAs } from 'file-saver/FileSaver';

@Injectable({
  providedIn: 'root'
})
export class ServiceTrainee extends ServiceGeneric<cTraineeItem, cTraineeList, ModelTrainee> {
  constructor(protected httpClient: HttpClient) {
    super("Trainee/", httpClient);
  }
  RemoveTraining(trainingId: string, traineeId: string): Observable<boolean> {
    var model = new ModelTraineeTraining();
    model.TraineeId = traineeId;
    model.TrainingId = trainingId;
    return this.httpClient.post<boolean>(this.apiURL + 'RemoveTraining', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  AddTraining(trainingId: string, traineeId: string): Observable<boolean> {
    var model = new ModelTraineeTraining();
    model.TraineeId = traineeId;
    model.TrainingId = trainingId;
    return this.httpClient.post<boolean>(this.apiURL + 'AddTraining', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  ListSearch(filterText: string): Observable<cTraineeItem[]> {
    var model = new ModelPaging()
    model.CurrentPage = 1;
    model.filterText = filterText;
    return this.httpClient.post<cTraineeItem[]>(this.apiURL + 'ListSearch', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  GetTraineeTraining(Id: string): Observable<MyTraining> {
    var model = new ModelId()
    model.Id = Id;
    return this.httpClient.post<MyTraining>(this.apiURL + 'GetTraineeTraining', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  GetMyTraining(): Observable<MyTraining> {
    return this.httpClient.get<MyTraining>(this.apiURL + 'GetMyTraining').pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  TraineeRegister(trainingId: string): Observable<boolean> {
    var model = new ModelId();
    model.Id = trainingId;
    return this.httpClient.post<boolean>(this.apiURL + 'TraineeRegister', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  ApproveTraineeRegister(model): Observable<boolean> {

    return this.httpClient.post<boolean>(this.apiURL + 'ApproveTraineeRegister', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  ImportTrainee(fileToUpload: File, TrainingId: string): Observable<ImportTrainee> {
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    return this.httpClient.post<ImportTrainee>(this.apiURL + 'ImportTrainee?TrainingId=' + TrainingId, formData).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  DownloadTrainingCertificate(trainingId: string): Observable<string> {
    var model = new ModelId();
    model.Id = trainingId;
    return this.httpClient.post<string>(this.apiURL + 'DownloadTrainingCertificate', model).pipe(
      //tap(data => saveAs(data, "CourseCertificates.zip")),
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
  getMyProfile(): Observable<cTraineeItem> {
    return this.httpClient.get<cTraineeItem>(this.apiURL + 'getMyProfile').pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  updateMyProfile(model: ModelTrainee): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'updateMyProfile', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
}
