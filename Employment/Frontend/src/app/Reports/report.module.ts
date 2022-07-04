import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../common/sharedmodule.module';
import { RouterModule } from '@angular/router';
import { StatsComponent } from './stats/stats.component';
import { StatsJobSeekerPerJobComponent } from './stats-job-seeker-per-job/stats-job-seeker-per-job.component';
import { StatsJobComponent } from './stats-job/stats-job.component';



@NgModule({
  declarations: [StatsJobSeekerPerJobComponent, StatsJobComponent],
  imports:  [
    SharedModule,
    RouterModule.forChild([
      { path: 'Admin/ReportStats', component: StatsComponent},
      { path: 'Admin/ReportStatsJobs', component: StatsJobComponent},
      { path: 'Admin/ReportStatsJobSeekerPerJob', component: StatsJobSeekerPerJobComponent},
    ])
  ]
})
export class ReportModule { }
