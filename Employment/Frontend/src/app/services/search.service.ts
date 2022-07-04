import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ServiceGeneric } from './GenericService.service';
import { Observable } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { ObservableInput } from 'rxjs';
import { ModelId } from '../interface/Model/ModelId.interface';

@Injectable({
  providedIn: 'root'
})
export class SearchService {
  apiURL: string = "https://employment.idealake.com/Job/";
  handleError: (err: any, caught: Observable<Object>) => ObservableInput<any>;

  constructor(protected httpClient: HttpClient) { }

  searchSer(Id: string) {
    var model = new ModelId();
    model.Id = Id
    return this.httpClient.post(this.apiURL + 'Search', model).pipe(
      //tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
  }
}
