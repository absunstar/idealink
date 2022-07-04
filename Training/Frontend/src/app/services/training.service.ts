import { Injectable } from '@angular/core';
import { cTrainingItem, cTrainingList, cAttendance } from '../interface/Response/Training.class';
import { ModelTraining } from '../interface/Model/ModelTraining.class';
import { ServiceGeneric } from './GenericService.service';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { tap, catchError } from 'rxjs/operators';
import { ModelFilterTraining } from '../interface/Model/ModelFilterTraining.class';
import { Observable } from 'rxjs';
import { ModelId } from '../interface/Model/ModelId.interface';
import { ModelTraineeTraining } from '../interface/Model/ModelTraineeTraining.class';
import { ModelSaveAttendnace } from '../interface/Model/ModelSaveAttendance.class';
import { ModelEntitySubEntityIds } from '../interface/Model/ModelEntitySubEntityIds.class';

@Injectable({
  providedIn: 'root'
})
export class ServiceTraining extends ServiceGeneric<cTrainingItem, cTrainingList, ModelTraining> {
  constructor(protected httpClient: HttpClient) {
    super("Training/", httpClient);
  }
  ExportTrainingTrainee(Id:string): Observable<Blob> {
    var model = new ModelId();
        model.Id = Id;
    return this.httpClient.post<Blob>(this.apiURL + 'ExportTrainingTrainee', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  SaveExamTemplate(model: ModelEntitySubEntityIds): Observable<boolean> {
    return this.httpClient.post<boolean>(this.apiURL + 'SaveExamTemplate', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  ExportTraining(model: ModelFilterTraining): Observable<Blob> {
    return this.httpClient.post<Blob>(this.apiURL + 'ExportTraining', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  searchTraining(model: ModelFilterTraining): Observable<cTrainingList> {
    return this.httpClient.post<cTrainingList>(this.apiURL + 'ListAll', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  SetAdminApproved(trainingId: string): Observable<cTrainingList> {
    var model = new ModelId();
    model.Id = trainingId;
    return this.httpClient.post<cTrainingList>(this.apiURL + 'SetAdminApproved', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  SetConfirmed1(trainingId: string): Observable<cTrainingList> {
    var model = new ModelId();
    model.Id = trainingId;
    return this.httpClient.post<cTrainingList>(this.apiURL + 'SetConfirmed1', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  SetConfirmed2(trainingId: string): Observable<cTrainingList> {
    var model = new ModelId();
    model.Id = trainingId;
    return this.httpClient.post<cTrainingList>(this.apiURL + 'SetConfirmed2', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  SaveAttendnace(trainingId: string, att: cAttendance) {
    var model = new ModelSaveAttendnace();
    model.trainingId = trainingId;
    model.Attendances = att;
    return this.httpClient.post<boolean>(this.apiURL + 'SaveAttendnace', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
  
}
