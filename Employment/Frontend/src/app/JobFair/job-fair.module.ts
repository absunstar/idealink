import { NgModule } from '@angular/core';
import { SharedModule } from '../common/sharedmodule.module';
import { ListComponent } from './list/list.component';
import { CreateComponent } from './create/create.component';
import { RouterModule } from '@angular/router';
import { RegisterComponent } from './register/register.component';



@NgModule({
  declarations: [
    ListComponent,
    CreateComponent,
    RegisterComponent],
  imports: [
    SharedModule,
    RouterModule.forChild([
      { path: 'JobFair/List', component: ListComponent },
      { path: 'JobFair/Create', component: CreateComponent },
      { path: 'JobFair/Edit/:Id', component: CreateComponent },
      { path: 'JobFair/Register/:Id', component: RegisterComponent }
    ])
  ]
})
export class JobFairModule { }
