import { Injectable } from '@angular/core';
import { UserManager, User } from 'oidc-client';
import { Constants } from '../constants';
import { Subject } from 'rxjs';
import { CoreModule } from './core.module';
import { HttpClient } from '@angular/common/http';
import { AuthContext } from '../model/auth-context';
import * as Oidc from 'oidc-client';

@Injectable()
export class AuthService {
  private _userManager: UserManager;
  private _user: User;
  private _loginChangedSubject = new Subject<boolean>();
  private _authContextSubject = new Subject<AuthContext>();
  loginChanged = this._loginChangedSubject.asObservable();
  authContextChanged = this._authContextSubject.asObservable();

  authContext: AuthContext;

  constructor(private _httpClient: HttpClient) {
    const stsSettings = {
      authority: Constants.stsAuthority,
      client_id: Constants.clientId,
      redirect_uri: `${Constants.clientRoot}signin-callback`,
      scope: 'openid profile projects-api',
      response_type: 'code',
      post_logout_redirect_uri: `${Constants.clientRoot}signout-callback`,
      automaticSilentRenew: true,
      //silent_redirect_uri: `${Constants.clientRoot}core/silent-callback/silent-callback.component.html`
      silent_redirect_uri: `${Constants.clientRoot}assets/silent-callback.html`
    };
    this._userManager = new UserManager(stsSettings);
    this._userManager.startSilentRenew();
    this._userManager.events.addAccessTokenExpired(_ => {
      this._loginChangedSubject.next(false);
    });
    this._userManager.events.addUserLoaded(user => {
      if (this._user !== user) {
        this._user = user;
        this.loadSecurityContext();
        this._loginChangedSubject.next(!!user && !user.expired);
      }
    });

  }
  CheckLogin() {
    this._userManager.signinSilent();
  }
  login() {
    return this._userManager.signinRedirect();
  }

  isLoggedIn(): Promise<boolean> {
    return this._userManager.getUser().then(user => {
      const userCurrent = !!user && !user.expired;
      if (this._user !== user) {
        this._loginChangedSubject.next(userCurrent);
      }
      if (userCurrent && !this.authContext) {

        this.loadSecurityContext();
      }
      this._user = user;
      return userCurrent;
    });
  }

  completeLogin() {

    return this._userManager.signinRedirectCallback().then(user => {
      this._user = user;
      this._loginChangedSubject.next(!!user && !user.expired);
      return user;
    });
  }
  logout() {
    this._userManager.signoutRedirect();
  }

  completeLogout() {
    this._user = null;
    this._loginChangedSubject.next(false);
    return this._userManager.signoutRedirectCallback();
  }

  getAccessToken() {
    return this._userManager.getUser().then(user => {
      if (!!user && !user.expired) {
        return user.access_token;
      }
      else {
        return null;
      }
    });
  }

  loadSecurityContext() {
    this._httpClient
      .get<AuthContext>(`${Constants.apiRoot}Account/AuthContext`)
      .subscribe(
        context => {

          this.authContext = new AuthContext();
          this.authContext.claims = context.claims;
          this.authContext.userProfile = context.userProfile;
          this._authContextSubject.next(this.authContext);
        },
        error => {}//console.error(error)
      );
  }

}