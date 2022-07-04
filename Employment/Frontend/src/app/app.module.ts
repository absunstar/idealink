import { HttpClientModule } from '@angular/common/http';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {MatSnackBarModule, MatMenuModule, MatToolbarModule, MatButtonModule, MatTableModule, MatInputModule, MatSelectModule, MatFormFieldModule, MatIconModule, MatRadioModule, MatCheckboxModule } from '@angular/material';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { Ng2SearchPipeModule } from 'ng2-search-filter';
import { CookieService } from 'ngx-cookie-service';
// import { AgmCoreModule } from '@agm/core';
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';
import { HomeComponent } from './home/home.component';
import { SigninRedirectCallbackComponent } from './home/signin-redirect-callback.component';
import { SignoutRedirectCallbackComponent } from './home/signout-redirect-callback.component';
import { UnauthorizedComponent } from './home/unauthorized.component';
import { DataManagementModule } from './DataManagement/data-management.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AccountModule } from './Account/account.module';
import { EmployerModule } from './Employer/employer.module';
import { RegisterSuccessComponent } from './home/register-success/register-success.component';
import { JobseekerModule } from './JobSeeker/jobseeker.module';
import { TestModule } from './ztest/test/test.module';
import { CommonModule } from '@angular/common';
import { AdminModule } from './Admin/admin.module';
import { JobFairModule } from './JobFair/job-fair.module';
import { ConfigModule } from './Config/config.module';
import { FilterPipe } from '../app/core/pipe/filter.pipe';
import { TermComponent } from './home/term/term.component';
import { PolicyComponent } from './home/policy/policy.component';
import { TranslatelistComponent } from './Translate/translatelist/translatelist.component';
import { SharedModule } from './common/sharedmodule.module';
import { StatsComponent } from './Reports/stats/stats.component';
import { ReportModule } from './Reports/report.module';
// import { JwPaginationModule } from 'jw-angular-pagination';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    SigninRedirectCallbackComponent,
    SignoutRedirectCallbackComponent,
    UnauthorizedComponent,
    RegisterSuccessComponent,
    TermComponent,
    PolicyComponent,
    TranslatelistComponent,
    StatsComponent,
    // FilterPipe
  ],
  imports: [
    SharedModule,
    BrowserModule,
    CommonModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatSnackBarModule,
    MatMenuModule,
    MatToolbarModule,
    MatButtonModule,
    MatTableModule,
    MatInputModule,
    MatAutocompleteModule,
    MatSelectModule,
    MatFormFieldModule,
    MatIconModule,
    MatRadioModule,
    MatCheckboxModule,
    ReactiveFormsModule,
    FormsModule,
    CoreModule,
    AppRoutingModule,
    NgbModule,
    DataManagementModule,
    AccountModule,
    EmployerModule,
    JobseekerModule,
    TestModule,
    AdminModule,
    Ng2SearchPipeModule,
    JobFairModule,
    ConfigModule,
    ReportModule,
    // JwPaginationModule
    // FilterPipe
    // AgmCoreModule.forRoot({
    //   apiKey: 'YOUR-API-KEY-HERE',
    //   libraries: ['places']
    // })
  ],
  // exports: [
  //   FilterPipe
  // ],
  providers: [
    CookieService
  ],
  bootstrap: [AppComponent],
  entryComponents:[
    //AddEditMilestoneDialogComponent
    HomeComponent
  ],
  schemas: [ CUSTOM_ELEMENTS_SCHEMA  ]
})
export class AppModule { }
