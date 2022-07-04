import { NgModule } from '@angular/core';
import { AccountComponent } from './account/account.component';
import { SharedModule } from 'src/app/common/sharedmodule.module';
import { RouterModule } from '@angular/router';
import { AdminPartnerRouteGuard, AdminRouteGuard, AdminPartnerSubPartnerRouteGuard } from 'src/app/core/admin-route-guard';
import { TraineeComponent } from './trainee/trainee.component';
import { TrainerCertificateComponent } from './trainer-certificate/trainer-certificate.component';



@NgModule({
  declarations: [
    AccountComponent,
    TraineeComponent,
    TrainerCertificateComponent
  ],
  imports: [
    SharedModule,
    RouterModule.forChild([
      { path: 'Account/Account', component: AccountComponent, canActivate: [AdminPartnerSubPartnerRouteGuard] },
      { path: 'Account/Trainee', component: TraineeComponent }
    ])
  ],
  providers:[
    AdminRouteGuard,
    AdminPartnerRouteGuard,
    AdminPartnerSubPartnerRouteGuard
  ]
})
export class AccountModule { }
