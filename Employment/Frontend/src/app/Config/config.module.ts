import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { JobFairCreateComponent } from './job-fair-create/job-fair-create.component';
import { SharedModule } from '../common/sharedmodule.module';
import { RouterModule } from '@angular/router';
import { JobFairRegisterComponent } from './job-fair-register/job-fair-register.component';
import { MenuComponent } from './menu/menu.component';
import { CompanyConfigComponent } from './company-config/company-config.component';



@NgModule({
  declarations: [
    JobFairCreateComponent,
    JobFairRegisterComponent,
    MenuComponent,
    CompanyConfigComponent
  ],
  imports: [
    SharedModule,
    RouterModule.forChild([
        { path: 'Config/JobFairCreate', component: JobFairCreateComponent },
        { path: 'Config/JobFairRegister', component: JobFairRegisterComponent },
        { path: 'Config/Company', component: CompanyConfigComponent }
      ])
  ]
})
export class ConfigModule { }
