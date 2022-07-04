import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/common/sharedmodule.module';
import { TestComponent } from './test.component';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';



@NgModule({
  declarations: [
    TestComponent
  ],
  imports: [
    SharedModule,
    HttpClientModule,
    RouterModule.forChild([
      { path: 'Test', component: TestComponent },
    ])
  ]
})
export class TestModule { }
