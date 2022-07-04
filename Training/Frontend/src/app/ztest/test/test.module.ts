import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TestComponent } from './test.component';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'src/app/common/sharedmodule.module';


// RECOMMENDED

// NOT RECOMMENDED (Angular 9 doesn't support this kind of import)
//import { TypeaheadModule } from 'ngx-bootstrap';

@NgModule({
  declarations: [TestComponent],
  imports: [
    SharedModule,    
    RouterModule.forChild([
      { path: 'Test/Test', component: TestComponent}
    ])
  ],providers: [
  ]
})
export class TestModule { }
