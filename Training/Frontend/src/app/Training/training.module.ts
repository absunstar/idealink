import { NgModule } from '@angular/core';
import { SharedModule } from '../common/sharedmodule.module';
import { RouterModule } from '@angular/router';
import { TrainingListComponent } from './training-list/training-list.component';
import { TraineeListComponent } from './trainee-list/trainee-list.component';
import { AttendanceComponent } from './attendance/attendance.component';
import { MyTrainingsComponent } from './my-trainings/my-trainings.component';

@NgModule({
  declarations: [
    TrainingListComponent,
    TraineeListComponent,
    AttendanceComponent,
    MyTrainingsComponent
  ],
  imports: [
    SharedModule,
   
    RouterModule.forChild([
      { path: 'Training/List', component: TrainingListComponent},
      { path: 'Training/Trainees/:Id', component: TraineeListComponent},
      { path: 'Training/Attendance/:Id', component: AttendanceComponent}
    ])
  ]
})
export class TrainingModule { }
