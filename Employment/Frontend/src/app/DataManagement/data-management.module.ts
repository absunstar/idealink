import { NgModule, CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from '@angular/core';
import { RouterModule } from '@angular/router';

import { SharedModule } from '../common/sharedmodule.module';
import { LanguagesComponent } from './languages/languages.component';
import { DataMenuComponent } from './data-menu/data-menu.component';
import { CountryComponent } from './country/country.component';
import { IndustryComponent } from './industry/industry.component';
import { JobFieldsComponent } from './job-fields/job-fields.component';
import { QualificationComponent } from './qualification/qualification.component';
import { YearsOfExperienceComponent } from './years-of-experience/years-of-experience.component';
import { CityComponent } from './city/city.component';
import { JobSubFieldComponent } from './job-sub-field/job-sub-field.component';
import { ConfirmationDialogService } from '../common/confirmation-dialog/confirmation-dialog.service';

@NgModule({
  declarations: [
    LanguagesComponent,
    DataMenuComponent,
    CountryComponent,
    IndustryComponent,
    JobFieldsComponent,
    QualificationComponent,
    YearsOfExperienceComponent,
    CityComponent,
    JobSubFieldComponent
  ],
  imports: [
    SharedModule,
    RouterModule.forChild([
      { path: 'DataManagement/Languages', component: LanguagesComponent},
      { path: 'DataManagement/Country', component: CountryComponent},
      { path: 'DataManagement/City/:Id', component: CityComponent},
      { path: 'DataManagement/Industry', component: IndustryComponent},
      { path: 'DataManagement/JobFields', component: JobFieldsComponent},
      { path: 'DataManagement/JobSubFields/:Id', component: JobSubFieldComponent},
      { path: 'DataManagement/Qualification', component: QualificationComponent},
      { path: 'DataManagement/YearsOfExperience', component: YearsOfExperienceComponent},
      
    ])
  ],
  providers: [ ConfirmationDialogService ],
  schemas: [ CUSTOM_ELEMENTS_SCHEMA,NO_ERRORS_SCHEMA  ]
})
export class DataManagementModule { }
