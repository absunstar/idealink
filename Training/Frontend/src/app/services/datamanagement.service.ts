import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Observable,throwError } from 'rxjs';
import { tap, catchError, map } from 'rxjs/operators';
import { ITrainingCategory,ITrainingCategoryItem } from '../interface/Response/TrainingCategory.interface';
import { Constants } from '../constants';
import { ModelPaging } from '../interface/Model/ModelPaging.interface';
import { ModelId } from '../interface/Model/ModelId.interface';
import { ModelCourse } from '../interface/Model/ModelCourse.class';
import { cNGOTypeItem, cNGOTypeList } from '../interface/Response/NGOType.class';
import { cCityItem, cCityList } from '../interface/Response/City.class';
import { ModelArea } from '../interface/Model/ModelArea.class';
import { cTrainingTypeItem, cTrainingTypeList } from '../interface/Response/TrainingType.class';
import { ModelTrainingCategory } from '../interface/Model/ModelTrainingCategory.class';



@Injectable({providedIn: 'root'})
export class ServiceDataManagement {
    private apiURL = Constants.apiRoot + 'DataManagement/'
    
    constructor(private httpClient: HttpClient) { }
    //#region TrainingType
    getTrainingTypeGetByid(Id: string):Observable<cTrainingTypeItem>{
      var model = new ModelId();
      model.Id = Id;
      return this.httpClient.post<cTrainingTypeItem>(this.apiURL + 'TrainingTypeGetById',model).pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
    }
    getTrainingTypeListActive():Observable<cTrainingTypeItem[]>{
      return this.httpClient.get<cTrainingTypeItem[]>(this.apiURL + 'TrainingTypeListActive').pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
    }
    getTrainingTypeAll(currentPage: number, filterText: string):Observable<cTrainingTypeList>{
      
        var model = new ModelPaging()
        model.CurrentPage = currentPage;
        model.filterText = filterText;
        return this.httpClient.post<cTrainingTypeList>(this.apiURL + 'TrainingTypeListAll',model).pipe(
            //tap(data => console.log('All: ' + JSON.stringify(data))),
            catchError(this.handleError)
          );
    }
    setTrainingTypeActivate(Id: string):Observable<boolean>{
      var model = new ModelId();
      model.Id = Id;
      return this.httpClient.post<boolean>(this.apiURL + 'TrainingTypeActivate',model).pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
    }
    setTrainingTypeDeactivate(Id: string):Observable<boolean>{
      var model = new ModelId();
      model.Id = Id;
      return this.httpClient.post<boolean>(this.apiURL + 'TrainingTypeDeactivate',model).pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
    }
    updateTrainingType(Id: string, Name:string):Observable<boolean>{
      var model = new cTrainingTypeItem();
      model.Id = Id;
      model.Name = Name;
      return this.httpClient.post<boolean>(this.apiURL + 'TrainingTypeUpdate',model).pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
    }
    createTrainingType(Name:string):Observable<boolean>{
      var model = new cTrainingTypeItem();
      model.Name = Name;
      return this.httpClient.post<boolean>(this.apiURL + 'TrainingTypeCreate',model).pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
    }
    //#endregion
    //#region TrainingCategory
    getTrainingCategoryGetByid(Id: string):Observable<ITrainingCategoryItem>{
      var model = new ModelId();
      model.Id = Id;
      return this.httpClient.post<ITrainingCategoryItem>(this.apiURL + 'TrainingCategoryGetById',model).pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
    }
    getTrainingCategoryAll(currentPage: number, filterText: string):Observable<ITrainingCategory>{
      
        var model = new ModelPaging()
        model.CurrentPage = currentPage;
        model.filterText = filterText;
        return this.httpClient.post<ITrainingCategory>(this.apiURL + 'TrainingCategoryListAll',model).pipe(
            //tap(data => console.log('All: ' + JSON.stringify(data))),
            catchError(this.handleError)
          );
    }
    getTrainingCategoryListByTrainingType(TrainingTypeId: string):Observable<ITrainingCategoryItem[]>{
      var model = new ModelId()
      model.Id = TrainingTypeId;
      return this.httpClient.post<ITrainingCategoryItem[]>(this.apiURL + 'TrainingCategoryListByTrainingType',model).pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
  }
    setTrainingCategoryActivate(Id: string):Observable<boolean>{
      var model = new ModelId();
      model.Id = Id;
      return this.httpClient.post<boolean>(this.apiURL + 'TrainingCategoryActivate',model).pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
    }
    setTrainingCategoryDeactivate(Id: string):Observable<boolean>{
      var model = new ModelId();
      model.Id = Id;
      return this.httpClient.post<boolean>(this.apiURL + 'TrainingCategoryDeactivate',model).pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
    }
    updateTrainingCategory(Id: string, Name:string, TrainingTypeId:string):Observable<boolean>{
      var model = new ModelTrainingCategory();
      model.Id = Id;
      model.Name = Name;
      model.TrainingTypeId = TrainingTypeId;
      return this.httpClient.post<boolean>(this.apiURL + 'TrainingCategoryUpdate',model).pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
    }
    createTrainingCategory(Name:string, TrainingTypeId:string):Observable<boolean>{
      var model = new ModelTrainingCategory();
      model.Name = Name;
      model.TrainingTypeId = TrainingTypeId;
      return this.httpClient.post<boolean>(this.apiURL + 'TrainingCategoryCreate',model).pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
    }
    //#endregion
    //#region Course
    setCourseActivate(TrainingCategoryId: string,Id: string):Observable<boolean>{
      var model = new ModelCourse();
      model.Id = Id;
      model.TrainingCategoryId = TrainingCategoryId;
      return this.httpClient.post<boolean>(this.apiURL + 'CourseActivate',model).pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
    }
    setCourseDeactivate(TrainingCategoryId: string,Id: string):Observable<boolean>{
      var model = new ModelCourse();
      model.Id = Id;
      model.TrainingCategoryId = TrainingCategoryId;
      return this.httpClient.post<boolean>(this.apiURL + 'CourseDeactivate',model).pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
    }
    createCourse(Id: string, Name:string):Observable<boolean>{
      var model = new ModelCourse();
      model.TrainingCategoryId = Id;
      model.Name = Name;
      return this.httpClient.post<boolean>(this.apiURL + 'CourseCreate',model).pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
    }
    updateCourse(TrainingCategoryId: string, Id: string, Name:string):Observable<boolean>{
      var model = new ModelCourse();
      model.Id = Id;
      model.Name = Name;
      model.TrainingCategoryId = TrainingCategoryId;
      return this.httpClient.post<boolean>(this.apiURL + 'CourseUpdate',model).pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
    }
    //#endregion
    //#region NGOType
    getNGOTypeGetByid(Id: string):Observable<cNGOTypeItem>{
      var model = new ModelId();
      model.Id = Id;
      return this.httpClient.post<cNGOTypeItem>(this.apiURL + 'NGOTypeGetById',model).pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
    }
    getNGOTypeAll(currentPage: number, filterText: string):Observable<cNGOTypeList>{
      
        var model = new ModelPaging()
        model.CurrentPage = currentPage;
        model.filterText = filterText;
        return this.httpClient.post<cNGOTypeList>(this.apiURL + 'NGOTypeListAll',model).pipe(
            //tap(data => console.log('All: ' + JSON.stringify(data))),
            catchError(this.handleError)
          );
    }
    setNGOTypeActivate(Id: string):Observable<boolean>{
      var model = new ModelId();
      model.Id = Id;
      return this.httpClient.post<boolean>(this.apiURL + 'NGOTypeActivate',model).pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
    }
    setNGOTypeDeactivate(Id: string):Observable<boolean>{
      var model = new ModelId();
      model.Id = Id;
      return this.httpClient.post<boolean>(this.apiURL + 'NGOTypeDeactivate',model).pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
    }
    updateNGOType(Id: string, Name:string):Observable<boolean>{
      var model = new cNGOTypeItem();
      model.Id = Id;
      model.Name = Name;
      return this.httpClient.post<boolean>(this.apiURL + 'NGOTypeUpdate',model).pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
    }
    createNGOType(Name:string):Observable<boolean>{
      var model = new cNGOTypeItem();
      model.Name = Name;
      return this.httpClient.post<boolean>(this.apiURL + 'NGOTypeCreate',model).pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
    }
    //#endregion
    //#region City
    getCityGetByid(Id: string):Observable<cCityItem>{
      var model = new ModelId();
      model.Id = Id;
      return this.httpClient.post<cCityItem>(this.apiURL + 'CityGetById',model).pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
    }
    getCityListActive():Observable<cCityItem[]>{
      return this.httpClient.get<cCityItem[]>(this.apiURL + 'CityListActive').pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
    }
    getCityAll(currentPage: number, filterText: string):Observable<cCityList>{
      
        var model = new ModelPaging()
        model.CurrentPage = currentPage;
        model.filterText = filterText;
        return this.httpClient.post<cCityList>(this.apiURL + 'CityListAll',model).pipe(
            //tap(data => console.log('All: ' + JSON.stringify(data))),
            catchError(this.handleError)
          );
    }
    setCityActivate(Id: string):Observable<boolean>{
      var model = new ModelId();
      model.Id = Id;
      return this.httpClient.post<boolean>(this.apiURL + 'CityActivate',model).pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
    }
    setCityDeactivate(Id: string):Observable<boolean>{
      var model = new ModelId();
      model.Id = Id;
      return this.httpClient.post<boolean>(this.apiURL + 'CityDeactivate',model).pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
    }
    updateCity(Id: string, Name:string):Observable<boolean>{
      var model = new cCityItem();
      model.Id = Id;
      model.Name = Name;
      return this.httpClient.post<boolean>(this.apiURL + 'CityUpdate',model).pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
    }
    createCity(Name:string):Observable<boolean>{
      var model = new cCityItem();
      model.Name = Name;
      return this.httpClient.post<boolean>(this.apiURL + 'CityCreate',model).pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
    }
    //#endregion
    //#region Area
    setAreaActivate(TrainingCategoryId: string,Id: string):Observable<boolean>{
      var model = new ModelArea();
      model.Id = Id;
      model.CityId = TrainingCategoryId;
      return this.httpClient.post<boolean>(this.apiURL + 'AreaActivate',model).pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
    }
    setAreaDeactivate(CityId: string,Id: string):Observable<boolean>{
      var model = new ModelArea();
      model.Id = Id;
      model.CityId = CityId;
      return this.httpClient.post<boolean>(this.apiURL + 'AreaDeactivate',model).pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
    }
    createArea(Id: string, Name:string):Observable<boolean>{
      var model = new ModelArea();
      model.CityId = Id;
      model.Name = Name;
      return this.httpClient.post<boolean>(this.apiURL + 'AreaCreate',model).pipe(
          //tap(data => console.log('All: ' + JSON.stringify(data))),
          catchError(this.handleError)
        );
    }
    updateArea(CityId: string, Id: string, Name:string):Observable<boolean>{
      var model = new ModelArea();
      model.Id = Id;
      model.Name = Name;
      model.CityId = CityId;
      return this.httpClient.post<boolean>(this.apiURL + 'AreaUpdate',model).pipe(
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