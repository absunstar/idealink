import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { SigninRedirectCallbackComponent } from './home/signin-redirect-callback.component';
import { SignoutRedirectCallbackComponent } from './home/signout-redirect-callback.component';
import { UnauthorizedComponent } from './home/unauthorized.component';
import { RegisterSuccessComponent } from './home/register-success/register-success.component';
import { TermComponent } from './home/term/term.component';
import { PolicyComponent } from './home/policy/policy.component';
import { TranslatelistComponent } from './Translate/translatelist/translatelist.component';

const routes: Routes = [
    { path: '', component: HomeComponent, data:{PageName:"home"} },
    { path: 'home/:msg/:isSuccess', component: HomeComponent, data:{PageName:"home"} },
    { path: 'home/RegisterSuccess', component: RegisterSuccessComponent, data:{PageName:"home"} },
    { path: 'Terms', component: TermComponent, data:{PageName:"home"} },
    { path: 'Policy', component: PolicyComponent, data:{PageName:"home"} },
    { path: 'Translate/:Type/:Id', component: TranslatelistComponent },
    { path: 'signin-callback', component: SigninRedirectCallbackComponent },
    { path: 'signout-callback', component: SignoutRedirectCallbackComponent },
    { path: 'unauthorized', component: UnauthorizedComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
