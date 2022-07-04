import { Injectable } from '@angular/core';
import { ServiceGeneric } from './GenericService.service';
import { cGenericIdNameItem, cGenericIdNameList } from '../interface/Response/GenericIdName.class';
import { ModelIdName } from '../interface/Model/ModelIdName.class';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ServiceYearsOfExperience extends ServiceGeneric<cGenericIdNameItem,cGenericIdNameList,ModelIdName> {

  constructor(protected httpClient: HttpClient) {
    super("YearsOfExperience/", httpClient);
  }
}
