import { Injectable, OnInit } from '@angular/core';
import { AuthService } from '../core/auth-service.component';
import { Subject } from 'rxjs';
import { AuthContext } from '../model/auth-context';

@Injectable({
  providedIn: 'root'
})
export class ServiceLoginUser implements OnInit {
  AuthContext: AuthContext;
  isLoggedIn = false;
  userName: string;
  userId: string;
  userRole: string;
  IsAdmin: boolean;
  isEmployer: boolean;
  isJobSeeker: boolean;

  private _userId = new Subject<string>();
  UserIdChanged = this._userId.asObservable();

  private _userName = new Subject<string>();
  UserNameChanged = this._userName.asObservable();

  private _userRole = new Subject<string>();
  UserRoleChanged = this._userRole.asObservable();

  private _userIsLoggedIn = new Subject<boolean>();
  UserIsLoggedInChanged = this._userIsLoggedIn.asObservable();

  private _userIsAdmin = new Subject<boolean>();
  UserIsAdminChanged = this._userIsAdmin.asObservable();

  private _userIsEmployer = new Subject<boolean>();
  UserIsEmployerChanged = this._userIsEmployer.asObservable();

  private _userIsJobSeeker = new Subject<boolean>();
  UserIsJobSeekerChanged = this._userIsJobSeeker.asObservable();

  constructor(private _authService: AuthService) {
    this._authService.loginChanged.subscribe(loggedIn => {
      this.isLoggedIn = loggedIn;
      this._userIsLoggedIn.next(loggedIn);
    });
    this._authService.authContextChanged.subscribe(obj => {
      this.AuthContext = obj;
      if (this.isLoggedIn && this.AuthContext) {
        this.userName = obj.userProfile.FullName;
        this.userId = obj.userId;
        this.userRole = obj.userProfile.Role;
        this.IsAdmin = obj.isAdmin;
        this.isEmployer = obj.isEmployer;
        this.isJobSeeker = obj.isJobSeeker;

        this._userName.next(obj.userProfile.FullName);
        this._userId.next(obj.userId);
        this._userRole.next(obj.userProfile.Role);
        this._userIsAdmin.next(obj.isAdmin);
        this._userIsEmployer.next(obj.isEmployer);
        this._userIsJobSeeker.next(obj.isJobSeeker);
      }
    });
  }
  ngOnInit() {

  }

}
