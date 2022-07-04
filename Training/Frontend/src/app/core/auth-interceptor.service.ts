import { HttpEvent, HttpHandler, HttpHeaders, HttpInterceptor, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { from, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Constants } from '../constants';
import { AuthService } from './auth-service.component';
import { Utils } from './utils';
import { ShowMessage } from '../interface/Model/ModelShowMessage.class';
import { ServiceShowMessage } from '../services/show-message.service';

@Injectable()
export class AuthInterceptorService implements HttpInterceptor {
  message: ShowMessage = new ShowMessage();

  constructor(private _authService: AuthService,
    private _router: Router,
    private BLServiceShowMessage: ServiceShowMessage) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (req.url.startsWith(Constants.apiRoot) && req.url.toLowerCase().indexOf("misc") == -1) {
      return from(this._authService.getAccessToken().then(token => {
        var lang = "en";
        if (localStorage.getItem('lang'))
          lang = localStorage.getItem('lang');

        const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`).set("currentLang", lang);
        const authReq = req.clone({ headers });
        return next.handle(authReq).pipe(tap(_ => { }, error => {
          var respError = error as HttpErrorResponse;
          if (respError && (respError.status === 401 || respError.status === 403)) {
            this._router.navigate(['/unauthorized']);
          }
          if (respError && (respError.status === 400)) {
            var msg = Utils.formatError(respError);
            this.message.Error(msg);
            this.BLServiceShowMessage.sendMessage(this.message);
          }
        })).toPromise();
      }));
    }
    else {
      return next.handle(req);
    }
  }
}