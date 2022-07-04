import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/common/sharedmodule.module';
import { RouterModule } from '@angular/router';
import { ResumeComponent } from './resume/resume.component';
import { JobSeekerDashboardComponent } from './job-seeker-dashboard/job-seeker-dashboard.component';
import { ShortListJobsComponent } from './short-list-jobs/short-list-jobs.component';
import { AppliedJobsComponent } from './applied-jobs/applied-jobs.component';
import { JobSeekerMenuComponent } from './job-seeker-menu/job-seeker-menu.component';
import { SearchComponent } from './search/search.component';
// import { SearchComponent } from '../Employer/search/search.component';
import { ApproveContactComponent } from './approve-contact/approve-contact.component';


@NgModule({
  declarations: [
    JobSeekerDashboardComponent,
    ResumeComponent,
    ShortListJobsComponent,
    AppliedJobsComponent,
    JobSeekerMenuComponent,
    SearchComponent,
    ApproveContactComponent
  ],
  imports: [
    SharedModule,
    // SearchComponent,
    RouterModule.forRoot([
      { path: 'JobSeeker/Dashboard', component: JobSeekerDashboardComponent },
      { path: 'JobSeeker/Resume/:Id', component: ResumeComponent },
      { path: 'JobSeeker/ShortList', component: ShortListJobsComponent },
      { path: 'JobSeeker/Search', component: SearchComponent, data:{PageName:"home"} },
      { path: 'JobSeeker/History', component: AppliedJobsComponent },
      { path: 'JobSeeker/ApproveContact', component: ApproveContactComponent }
    ]),
  ]
})
export class JobseekerModule { }
