import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../common/sharedmodule.module';
import { JobApprovalComponent } from './job-approval/job-approval.component';
import { RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { CompanyApprovalComponent } from './company-approval/company-approval.component';
import { AdminJobListComponent } from './admin-job-list/admin-job-list.component';
import { CompanyListComponent } from './company-list/company-list.component';



@NgModule({
  declarations: [
    JobApprovalComponent,
    LoginComponent,
    CompanyApprovalComponent,
    AdminJobListComponent,
    CompanyListComponent
  ],
  imports: [
    SharedModule,
    RouterModule.forChild([
      { path: 'Admin/JobApproval', component: JobApprovalComponent},
      { path: 'Admin/CompanyApproval', component: CompanyApprovalComponent},
      { path: 'Admin/Login', component: LoginComponent, data:{PageName:"home"}},
      { path: 'Admin/JobsList', component: AdminJobListComponent },
      { path: 'Admin/CompanyList', component: CompanyListComponent },
    ])
  ]
})
export class AdminModule { }
