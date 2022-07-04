import { NgModule } from '@angular/core';
import { AccountComponent } from './account/account.component';
import { SharedModule } from 'src/app/common/sharedmodule.module';
import { RouterModule } from '@angular/router';
import {  AdminRouteGuard } from 'src/app/core/admin-route-guard';
import { ChangePasswordComponent } from './change-password/change-password.component';



@NgModule({
  declarations: [
    AccountComponent,
    ChangePasswordComponent
  ],
  imports: [
    SharedModule,
    RouterModule.forChild([
      { path: 'account/account', component: AccountComponent },
      { path: 'account/ChangePassword', component: ChangePasswordComponent },
    ])
  ],
  providers:[
    AdminRouteGuard
  ]
})
export class AccountModule { }
