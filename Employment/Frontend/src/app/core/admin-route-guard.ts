import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot } from '@angular/router';
import { AuthService } from './auth-service.component';

@Injectable()
export class AdminRouteGuard implements CanActivate {
    constructor(private _authService: AuthService) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {

        return !!this._authService.authContext &&
        this._authService.isLoggedIn &&
            this._authService.authContext.isAdmin;
    }
}
@Injectable()
export class EmployerRouteGuard implements CanActivate {
    constructor(private _authService: AuthService) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        return !!this._authService.authContext &&
        this._authService.isLoggedIn &&
            this._authService.authContext.isEmployer;
    }
}
@Injectable()
export class JobSeekerRouteGuard implements CanActivate {
    constructor(private _authService: AuthService) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        return !!this._authService.authContext &&
            this._authService.isLoggedIn &&
            this._authService.authContext.isJobSeeker; 
    }
}