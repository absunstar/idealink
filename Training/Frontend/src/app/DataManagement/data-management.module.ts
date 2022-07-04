import { NgModule, CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from '@angular/core';
import { RouterModule } from '@angular/router';

import { TrainingCategoryListComponent } from './TrainingCategory/TrainingCategory-list.component';
import { CourseListComponent } from './Course/Course-list.component';
import { AdminRouteGuard } from '../core/admin-route-guard';
import { AreaComponent } from './area/area.component';
import { CityComponent } from './city/city.component';
import { NGOTypeComponent } from './ngotype/ngotype.component';
import { TrainingTypeComponent } from './training-type/training-type.component';
import { SharedModule } from '../common/sharedmodule.module';

@NgModule({
  declarations: [
    TrainingCategoryListComponent,
    CourseListComponent,
    AreaComponent,
    CityComponent,
    NGOTypeComponent,
    TrainingTypeComponent
  ],
  imports: [
    SharedModule,
    RouterModule.forChild([
      { path: 'DataManagement/Course/:id', component: CourseListComponent, canActivate: [AdminRouteGuard] },
      { path: 'DataManagement/TrainingType', component: TrainingTypeComponent, canActivate: [AdminRouteGuard]},
      { path: 'DataManagement/TrainingCategory', component: TrainingCategoryListComponent, canActivate: [AdminRouteGuard]},
      { path: 'DataManagement/NGOType', component: NGOTypeComponent, canActivate: [AdminRouteGuard]},
      { path: 'DataManagement/City', component: CityComponent, canActivate: [AdminRouteGuard]},
      { path: 'DataManagement/Area/:id', component: AreaComponent, canActivate: [AdminRouteGuard] },
    ])
  ],
  schemas: [ CUSTOM_ELEMENTS_SCHEMA,NO_ERRORS_SCHEMA  ]
})
export class DataManagementModule { }
