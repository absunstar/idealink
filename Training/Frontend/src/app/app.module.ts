import { HttpClient, HttpClientModule } from '@angular/common/http';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatSnackBarModule, MatMenuModule, MatToolbarModule, MatButtonModule, MatTableModule, MatInputModule, MatSelectModule, MatFormFieldModule, MatIconModule, MatRadioModule, MatCheckboxModule, MatExpansionModule, MatDialogModule } from '@angular/material';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AdminModule } from './admin/admin.module';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';
import { ContactUsComponent } from './home/contact-us.component';
import { HomeComponent } from './home/home.component';
import { SigninRedirectCallbackComponent } from './home/signin-redirect-callback.component';
import { SignoutRedirectCallbackComponent } from './home/signout-redirect-callback.component';
import { UnauthorizedComponent } from './home/unauthorized.component';
import { DataManagementModule } from './DataManagement/data-management.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { EntityManagementModule } from './EntityManagement/entity-management.module';
import { AccountModule } from './Account/account.module';
import { TestModule } from './ztest/test/test.module';
import { HeaderComponent } from './common/header/header.component';
import { FooterComponent } from './common/footer/footer.component';
import { TrainingModule } from './Training/training.module';
import { ListComponent } from './Questions/list/list.component';
import { CreateComponent } from './Questions/create/create.component';
import { TakeExamComponent } from './Exam/take-exam/take-exam.component';
import { CountdownModule } from 'ngx-countdown';
import { TermsComponent } from './home/terms/terms.component';
import { PolicyComponent } from './Home/policy/policy.component';
import { AboutComponent } from './Home/about/about.component';
import { ConfigTraineeComponent } from './config/config-trainee/config-trainee.component';
import { SharedModule } from './common/sharedmodule.module';
import { CreateFormComponent } from './common/ConfigForms/AdminConfig/create-form/create-form.component';
import { DynamicFieldDirective } from './common/ConfigForms/components/dynamic-field/dynamic-field.directive';
import { DynamicFormComponent } from './common/ConfigForms/components/dynamic-form/dynamic-form.component';
import { GenericsComponent } from './Certificates/generics/generics.component';
import { CategoryComponent } from './Certificates/category/category.component';
import { CertificateTrainingCenterComponent } from './Certificates/trainingcenter/trainingcenter.component';
import { RegisterComponent } from './Request/register/register.component';
import { ConfigRequestRegisterComponent } from './config/config-request-register/config-request-register.component';
import { AboutEditComponent } from './Content/about-edit/about-edit.component';
import { TrainerApprovalComponent } from './Certificates/trainer-approval/trainer-approval.component';

import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { TermEditComponent } from './Content/term-edit/term-edit.component';
import { PolicyEditComponent } from './Content/policy-edit/policy-edit.component';
import { InfoEditComponent } from './Content/info-edit/info-edit.component';
import { TemplatesComponent } from './Exam/templates/templates.component';
import { TranslateListComponent } from './Translate/translate-list/translate-list.component';
import { SiteLogoComponent } from './Content/site-logo/site-logo.component';
import { PartnerLogoComponent } from './Content/partner-logo/partner-logo.component';
import { VerifyComponent } from './Certificates/verify/verify.component';


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    ContactUsComponent,
    SigninRedirectCallbackComponent,
    SignoutRedirectCallbackComponent,
    UnauthorizedComponent,
    HeaderComponent,
    FooterComponent,
    ListComponent,
    CreateComponent,
    TakeExamComponent,
    TermsComponent,
    PolicyComponent,
    AboutComponent,
    ConfigTraineeComponent,
    GenericsComponent,
    CategoryComponent,
    CertificateTrainingCenterComponent,
    RegisterComponent,
    ConfigRequestRegisterComponent,
    AboutEditComponent,
    TrainerApprovalComponent,
    TermEditComponent,
    PolicyEditComponent,
    InfoEditComponent,
    TemplatesComponent,
    TranslateListComponent,
    SiteLogoComponent,
    PartnerLogoComponent,
    VerifyComponent,
  ],
  imports: [
    SharedModule,
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatSnackBarModule,
    MatMenuModule,
    MatToolbarModule,
    MatButtonModule,
    MatTableModule,
    MatInputModule,
    MatSelectModule,
    MatFormFieldModule,
    MatIconModule,
    MatRadioModule,
    MatCheckboxModule,
    ReactiveFormsModule,
    FormsModule,
    AdminModule,
    CoreModule,
    AppRoutingModule,
    NgbModule,
    DataManagementModule,
    EntityManagementModule,
    AccountModule,
    TrainingModule,
    TestModule,
    CountdownModule,
    MatButtonModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatTableModule,
    MatToolbarModule
  ],
  providers: [
  ],
  bootstrap: [AppComponent],
  entryComponents: [
    //AddEditMilestoneDialogComponent
    HomeComponent
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AppModule { }

