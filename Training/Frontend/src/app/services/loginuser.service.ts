import { Injectable, OnInit } from '@angular/core';
import { AuthService } from '../core/auth-service.component';
import { UserProfile } from '../model/user-profile';
import { Observable, Subject } from 'rxjs';
import { AuthContext } from '../model/auth-context';

@Injectable({
  providedIn: 'root'
})
export class ServiceLoginUser implements OnInit {
  AuthContext: AuthContext;
  isLoggedIn = false;
  userName: string;
  userRole: string;
  IsAdmin: boolean;
  IsPartner: boolean;
  IsSubPartner: boolean;
  IsTrainer: boolean;
  IsTrainee: boolean;

  private _userName = new Subject<string>();
  UserNameChanged = this._userName.asObservable();

  private _userRole = new Subject<string>();
  UserRoleChanged = this._userRole.asObservable();

  private _userIsLoggedIn = new Subject<boolean>();
  UserIsLoggedInChanged = this._userIsLoggedIn.asObservable();

  private _userIsAdmin = new Subject<boolean>();
  UserIsAdminChanged = this._userIsAdmin.asObservable();

  private _userIsPartner = new Subject<boolean>();
  UserIsPartnerChanged = this._userIsPartner.asObservable();

  private _userIsSubPartner = new Subject<boolean>();
  UserIsSubPartnerChanged = this._userIsSubPartner.asObservable();

  private _userIsTrainer = new Subject<boolean>();
  UserIsTrainerChanged = this._userIsTrainer.asObservable();

  private _userIsTrainee = new Subject<boolean>();
  UserIsTraineeChanged = this._userIsTrainee.asObservable();

  constructor(private _authService: AuthService) {
    this._authService.loginChanged.subscribe(loggedIn => {
      this.isLoggedIn = loggedIn;
      this._userIsLoggedIn.next(loggedIn);
    });
    this._authService.authContextChanged.subscribe(obj => {
      this.AuthContext = obj;
      if (this.isLoggedIn && this.AuthContext) {
        this.userName = obj.userProfile.FullName;
        this.userRole = obj.userProfile.Role;
        this.IsAdmin = obj.isAdmin;
        this.IsPartner = obj.isPartner;
        this.IsSubPartner = obj.isSubPartner;
        this.IsTrainer = obj.isTrainer;
        this.IsTrainee = obj.isTrainee;

        this._userName.next(obj.userProfile.FullName);
        this._userRole.next(obj.userProfile.Role);
        this._userIsAdmin.next(obj.isAdmin);
        this._userIsPartner.next(obj.isPartner);
        this._userIsSubPartner.next(obj.isSubPartner);
        this._userIsTrainer.next(obj.isTrainer);
        this._userIsTrainee.next(obj.isTrainee);
      }
    });
  }
  ngOnInit() {

  }

}
