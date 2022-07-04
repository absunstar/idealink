import { Injectable } from '@angular/core';
import { ShowMessage } from '../interface/Model/ModelShowMessage.class';
import { Observable, Subject, observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ServiceShowMessage {

  constructor() { }
  private _msgSubject = new Subject<ShowMessage>();

  msgChanged = this._msgSubject.asObservable();
  
  sendMessage(message: ShowMessage) {
      this._msgSubject.next(message);
    }
}
