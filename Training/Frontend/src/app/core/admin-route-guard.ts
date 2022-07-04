import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot } from '@angular/router';
import { AuthService } from './auth-service.component';

@Injectable()
export class AdminRouteGuard implements CanActivate {
    constructor(private _authService: AuthService) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {

        return !!this._authService.authContext &&
            this._authService.authContext.isAdmin;
    }
}
@Injectable()
export class AdminPartnerRouteGuard implements CanActivate {
    constructor(private _authService: AuthService) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        return (!!this._authService.authContext &&
            (this._authService.authContext.isAdmin
                || this._authService.authContext.isPartner));
    }
}
@Injectable()
export class AdminPartnerSubPartnerRouteGuard implements CanActivate {
    constructor(private _authService: AuthService) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        return (!!this._authService.authContext &&
            (this._authService.authContext.isAdmin
                || this._authService.authContext.isPartner
                || this._authService.authContext.isSubPartner));
    }
}
@Injectable()
export class TraineeRouteGuard implements CanActivate {
    constructor(private _authService: AuthService) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        return (!!this._authService.authContext &&
            this._authService.authContext.isTrainee);
    }
}