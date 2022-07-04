import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../common/sharedmodule.module';
import { RouterModule } from '@angular/router';
import { FilterPipe } from '../core/pipe/filter.pipe';
import { EmployerDashboardComponent } from './employer-dashboard/employer-dashboard.component';
import { EmployerMenuComponent } from './employer-menu/employer-menu.component';
import { EmpoyerProfileComponent } from './empoyer-profile/empoyer-profile.component';
import { EmpoyerMyJobsComponent } from './empoyer-my-jobs/empoyer-my-jobs.component';
import { EmpoyerShortListResumesComponent } from './empoyer-short-list-resumes/empoyer-short-list-resumes.component';
import { EmployerPostJobComponent } from './employer-post-job/employer-post-job.component';
import { EmployerMyCompaniesComponent } from './employer-my-companies/employer-my-companies.component';
import { CompanyViewComponent } from './company-view/company-view.component';
import { JobViewComponent } from './job-view/job-view.component';
import { SearchComponent } from './search/search.component';
import { AppliedJobSeekersComponent } from './applied-job-seekers/applied-job-seekers.component';
import { CoreModule } from '../core/core.module';
import { FilterLPipe } from './filter.pipe';


@NgModule({
  declarations: [
    EmployerDashboardComponent,
    EmployerMenuComponent,
    EmpoyerProfileComponent,
    EmpoyerMyJobsComponent,
    EmpoyerShortListResumesComponent,
    EmployerPostJobComponent,
    EmployerMyCompaniesComponent,
    CompanyViewComponent,
    JobViewComponent,
    SearchComponent,
    AppliedJobSeekersComponent,
    FilterLPipe,
    
    
    // FilterPipe
  ],
  imports: [
    SharedModule,
    CoreModule,
    // /JwPaginationModule,
    // FilterPipe,
    RouterModule.forRoot([
      { path: 'Employer/Dashboard', component: EmployerDashboardComponent },
      { path: 'Employer/Company/:Id', component: CompanyViewComponent },
      { path: 'Employer/Profile', component: EmpoyerProfileComponent },
      { path: 'Employer/Profile/:Id', component: EmpoyerProfileComponent },
      { path: 'Employer/ManageMyCompanies', component: EmployerMyCompaniesComponent },
      { path: 'Employer/PostJob', component: EmployerPostJobComponent },
      { path: 'Employer/PostJob/:Id', component: EmployerPostJobComponent },
      { path: 'Employer/Job/:Id', component: JobViewComponent, data:{PageName:"home"} },
      { path: 'Employer/ManageMyJobs', component: EmpoyerMyJobsComponent,data:{PageName:"home"}  },
      { path: 'Employer/ShortListResumes', component: EmpoyerShortListResumesComponent },
      { path: 'Employer/Search', component: SearchComponent },
      { path: 'Job/Search', component: SearchComponent, data:{PageName:"home"} },
      { path: 'Employer/AppliedResume/:Id', component: AppliedJobSeekersComponent }
    ])
  ],
  exports: [ SearchComponent]
})
export class EmployerModule { }
