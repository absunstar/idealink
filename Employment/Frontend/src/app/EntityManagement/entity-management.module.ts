import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { PartnerComponent } from '../EntityManagement/partner/partner.component';
import { SubPartnerComponent } from '../EntityManagement/sub-partner/sub-partner.component';
import { TrainingCenterComponent } from '../EntityManagement/training-center/training-center.component';

import { AdminRouteGuard, AdminPartnerRouteGuard } from '../core/admin-route-guard';
import { SharedModule } from '../common/sharedmodule.module';

@NgModule({
  declarations: [ 
    PartnerComponent,
    SubPartnerComponent,
    TrainingCenterComponent
  ],
  imports: [ 
    SharedModule,
    RouterModule.forChild([
      { path: 'EntityManagement/Partner', component: PartnerComponent, canActivate: [AdminPartnerRouteGuard]},
      { path: 'EntityManagement/SubPartner', component: SubPartnerComponent, canActivate: [AdminPartnerRouteGuard]},
      { path: 'EntityManagement/TrainingCenter/:id', component: TrainingCenterComponent, canActivate: [AdminPartnerRouteGuard]}
    ])
  ]
})
export class EntityManagementModule { }
