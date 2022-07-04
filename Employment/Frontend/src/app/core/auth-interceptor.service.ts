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
  message:ShowMessage = new ShowMessage();

  constructor(private _authService: AuthService,
    private _router: Router,
    private BLServiceShowMessage : ServiceShowMessage) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (req.url.startsWith(Constants.apiRoot)) {
      return from(this._authService.getAccessToken().then(token => {
        var lang = "en";
        if (localStorage.getItem('lang'))
          lang = localStorage.getItem('lang');

        const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`).set("currentLang", lang);
        // const headers = new HttpHeaders().set('Authorization', `Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6IjNENUZDQTJEOTg3MTI1QUU4NzZBQkI4QjdGQjhENTU4RjNBREE0NzciLCJ0eXAiOiJhdCtqd3QiLCJ4NXQiOiJQVl9LTFpoeEphNkhhcnVMZjdqVldQT3RwSGMifQ.eyJuYmYiOjE2Mjg1MTkwODAsImV4cCI6MTYyODUyOTA4MCwiaXNzIjoiaHR0cHM6Ly9lbXBsb3ltZW50c3RzLmlkZWFsYWtlLmNvbSIsImF1ZCI6InByb2plY3RzLWFwaSIsImNsaWVudF9pZCI6Ijg3YTEwYjg3LVhYWFgtOTQ1Ny0wODNlZDI1ZmFhYzUiLCJzdWIiOiI5ODk3ZjMwYi05NDE2LTRkMDAtYTNkOS01Yjk5MjIzMDM4OGMiLCJhdXRoX3RpbWUiOjE2MjgxNjIwNTcsImlkcCI6ImxvY2FsIiwiQXNwTmV0LklkZW50aXR5LlNlY3VyaXR5U3RhbXAiOiJWRlVWU1hGSEZPWTMySTVHNTRaWDIyWklFRUxBVEFRRCIsInJvbGUiOiJFbXBsb3llciIsInByZWZlcnJlZF91c2VybmFtZSI6InNvbmFsX2FnYXJ3YWxAaWRlYWxha2UuY29tIiwibmFtZSI6InNvbmFsX2FnYXJ3YWxAaWRlYWxha2UuY29tIiwiZW1haWwiOiJzb25hbF9hZ2Fyd2FsQGlkZWFsYWtlLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjp0cnVlLCJNRElEIjoiNjBkOTgyNDc3NTY3YmUxZmEwNDA2MjBlIiwic2NvcGUiOlsib3BlbmlkIiwicHJvZmlsZSIsInByb2plY3RzLWFwaSJdLCJhbXIiOlsicHdkIl19.YjJ7fqYrMpmsmrw7zCqPcCXrqdv38f0JivxbANDxXjkFbY-gKYqiPXzyIdD1-d7qm6WtWIGth-qALAZBK4l8lzMaiHFrrGRrynEt0GfRWFShZ9vvdxAw7-YKEt6oCq-OS_qRA2Wk2XaC2xlT7GAWAVTJ6gnMiM4Oap5rSIdNnCayTJ4f3v04PPXrUv-C4gxoJjezm0LbiUEHgP5PhXmDzIKpJpQqUtkpoQhPzcg7rjc8NYiKY07LoQ0zdAv4TFWge2ITVQQ2WPexfzhQeiHGUZB14u75gNas-z0DpSUBlpZqzuVvb_wjuHFa_WASkrZ6W0Eex3tlBc36rSvN05mZ7w`).set("currentLang", lang);
        const authReq = req.clone({ headers });
        return next.handle(authReq).pipe(tap(_ => { }, error => {
          var respError = error as HttpErrorResponse;
          if (respError && (respError.status === 401 || respError.status === 403)) {
            this._router.navigate(['/unauthorized']);
          }
          if (respError && (respError.status === 400 )) {
            var msg = Utils.formatError(respError);
            this.message.Error(respError.error);
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