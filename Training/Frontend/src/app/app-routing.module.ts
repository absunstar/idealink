 import { NgModule, MissingTranslationStrategy } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ContactUsComponent } from './home/contact-us.component';
import { HomeComponent } from './home/home.component';
import { SigninRedirectCallbackComponent } from './home/signin-redirect-callback.component';
import { SignoutRedirectCallbackComponent } from './home/signout-redirect-callback.component';
import { UnauthorizedComponent } from './home/unauthorized.component';
import { ListComponent } from './Questions/list/list.component';
import { CreateComponent } from './Questions/create/create.component';
import { TakeExamComponent } from './Exam/take-exam/take-exam.component';
import { TermsComponent } from './home/terms/terms.component';
import { PolicyComponent } from './Home/policy/policy.component';
import { AboutComponent } from './Home/about/about.component';
import { ConfigTraineeComponent } from './config/config-trainee/config-trainee.component';
import { GenericsComponent } from './Certificates/generics/generics.component';
import { CategoryComponent } from './Certificates/category/category.component';
import { CertificateTrainingCenterComponent } from './Certificates/trainingcenter/trainingcenter.component';
import { MyTrainingsComponent } from './Training/my-trainings/my-trainings.component';
import { RegisterComponent } from './Request/register/register.component';
import { ConfigRequestRegisterComponent } from './config/config-request-register/config-request-register.component';
import { AboutEditComponent } from './Content/about-edit/about-edit.component';
import { AddTraineeComponent } from './common/add-trainee/add-trainee.component';
import { TrainerApprovalComponent } from './Certificates/trainer-approval/trainer-approval.component';
import { InfoEditComponent } from './Content/info-edit/info-edit.component';
import { PolicyEditComponent } from './Content/policy-edit/policy-edit.component';
import { TermEditComponent } from './Content/term-edit/term-edit.component';
import { TemplatesComponent } from './Exam/templates/templates.component';
import { TranslateListComponent } from './Translate/translate-list/translate-list.component';
import { TrainerCertificateComponent } from './Account/trainer-certificate/trainer-certificate.component';
import { SiteLogoComponent } from './Content/site-logo/site-logo.component';
import { PartnerLogoComponent } from './Content/partner-logo/partner-logo.component';
import { TraineeRouteGuard } from './core/admin-route-guard';

const routes: Routes = [
    { path: '', component: HomeComponent, data:{PageName:"home"} },
    { path: 'home/:msg/:isSuccess', component: HomeComponent, data:{PageName:"home"} },
    { path: 'RequestRegister', component: RegisterComponent , data:{PageName:"home"}},
    { path: 'Translate/:Type/:Id', component: TranslateListComponent },
    { path: 'MyTraining', component: MyTrainingsComponent },
    { path: 'MyProfile', component: AddTraineeComponent },
    { path: 'TrainerCertificate/:Id', component: TrainerCertificateComponent },
    { path: 'MyTraining/:Id', component: MyTrainingsComponent },
    { path: 'Trainee/Register', component: AddTraineeComponent, data:{PageName:"home", PageType:"register"} },
    { path: 'Trainee/Create/:IsCreate/:TraineeId/:TrainingId', component: AddTraineeComponent },
    { path: 'Content/PortalLogo', component: SiteLogoComponent },
    { path: 'Content/PartnerLogo', component: PartnerLogoComponent },
    { path: 'About/Edit', component: AboutEditComponent },
    { path: 'Term/Edit', component: TermEditComponent },
    { path: 'Policy/Edit', component: PolicyEditComponent },
    { path: 'Info/Edit', component: InfoEditComponent },
    { path: 'About', component: AboutComponent , data:{PageName:"home"}},
    { path: 'Questions/List', component: ListComponent },
    { path: 'Questions/Create/:Id', component: CreateComponent },
    { path: 'Exam/Templates', component: TemplatesComponent },
    { path: 'Exam/List', component: TakeExamComponent },
    { path: 'Exam/TakeExam/:Id', component: TakeExamComponent },
    { path: 'contact-us', component: ContactUsComponent, data:{PageName:"home"} },
    { path: 'Terms', component: TermsComponent, data:{PageName:"home"} },
    { path: 'Policy', component: PolicyComponent, data:{PageName:"home"} },
    { path: 'Certificate/TranierApproval', component: TrainerApprovalComponent },
    { path: 'Certificate/Generics', component: GenericsComponent },
    { path: 'Certificate/Category', component: CategoryComponent },
    { path: 'Certificate/TrainingCenter', component: CertificateTrainingCenterComponent },
    { path: 'ConfigTrainee', component:ConfigTraineeComponent},
    { path: 'ConfigRequestRegister', component:ConfigRequestRegisterComponent},
    { path: 'signin-callback', component: SigninRedirectCallbackComponent },
    { path: 'signout-callback', component: SignoutRedirectCallbackComponent },
    { path: 'unauthorized', component: UnauthorizedComponent },
    
  
    

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
