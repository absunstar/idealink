import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatButtonModule, MatTableModule, MatInputModule, MatSelectModule, MatFormFieldModule, MatIconModule, MatRadioModule, MatCheckboxModule, MAT_LABEL_GLOBAL_OPTIONS, MatMenuModule, MatMenu, MatTabsModule, MatTooltipModule, MatAutocompleteModule, MatSlideToggleModule, MatDialog, MatDialogModule, MatDatepickerModule, MatCardModule, MatListModule, MatNativeDateModule, MatOptionModule, MatExpansionModule } from '@angular/material';

import { ReactiveFormsModule } from '@angular/forms';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { AtomSpinnerModule } from 'angular-epic-spinners';
import { OwlDateTimeModule, OwlNativeDateTimeModule } from 'ng-pick-datetime';
//import { OwlTooltipModule } from 'owl-ng';

import { CustomPaginationComponent } from './pagination/components/custom-pagination/custom-pagination.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgbModule, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { SearchEmployerComponent } from './search-employer/search-employer.component';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { ConfirmationDialogComponent } from './confirmation-dialog/confirmation-dialog.component';
import { InputComponent } from './ConfigForms/components/input/input.component';
import { ButtonComponent } from './ConfigForms/components/button/button.component';
import { SelectComponent } from './ConfigForms/components/select/select.component';
import { DateComponent } from './ConfigForms/components/date/date.component';
import { RadiobuttonComponent } from './ConfigForms/components/radiobutton/radiobutton.component';
import { CheckboxComponent } from './ConfigForms/components/checkbox/checkbox.component';
import { DynamicFieldDirective } from './ConfigForms/components/dynamic-field/dynamic-field.directive';
import { DynamicFormComponent } from './ConfigForms/components/dynamic-form/dynamic-form.component';
import { CreateFormComponent } from './ConfigForms/AdminConfig/create-form/create-form.component';
import { SelectMultiComponent } from './ConfigForms/components/SelectMulti/selectmulti.component';
import { SearchJobSeekerComponent } from './search-job-seeker/search-job-seeker.component';
import { NgxFileDropModule } from 'ngx-file-drop';
import { UploadFilesComponent } from './upload-files/upload-files.component';
import { NgbDateCustomParserFormatter } from '../core/NgbDateCustomParserFormatter.class';


import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { HttpClient, HttpClientModule } from '@angular/common/http';


@NgModule({
  declarations: [
    CustomPaginationComponent,
    SearchEmployerComponent,
    ConfirmationDialogComponent,
    InputComponent,
    ButtonComponent,
    SelectComponent,
    SelectMultiComponent,
    DateComponent,
    RadiobuttonComponent,
    CheckboxComponent,
    DynamicFieldDirective,
    DynamicFormComponent,
    CreateFormComponent,
    SearchJobSeekerComponent,
    UploadFilesComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    AtomSpinnerModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    MatCardModule,
    MatListModule,
    MatNativeDateModule,
    MatOptionModule,
    MatDatepickerModule,
    MatDatepickerModule,
    MatButtonModule,
    MatTableModule,
    MatInputModule,
    MatSelectModule,
    MatFormFieldModule,
    MatTooltipModule,
    MatTabsModule,
    MatIconModule,
    MatMenuModule,
    MatRadioModule,
    MatDialogModule,
    MatCheckboxModule,
    MatAutocompleteModule,
    MatExpansionModule,
    TypeaheadModule.forRoot(),
    AngularEditorModule ,
    NgxFileDropModule,
    HttpClientModule ,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: httpTranslateLoader,
        deps: [HttpClient]
      }
    })
  ],
  exports: [
    CustomPaginationComponent,
    CommonModule,
    FormsModule,
    BrowserAnimationsModule,
    MatCardModule,
    MatListModule,
    MatNativeDateModule,
    MatOptionModule,
    MatDatepickerModule,
    MatButtonModule,
    MatTableModule,
    MatInputModule,
    MatSelectModule,
    MatFormFieldModule,
    MatTooltipModule,
    MatTabsModule,
    MatIconModule,
    MatMenuModule,
    MatRadioModule,
    MatCheckboxModule,
    MatDialogModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    MatMenuModule,
    MatSlideToggleModule,
    AtomSpinnerModule,
    TypeaheadModule,
    OwlDateTimeModule, 
    OwlNativeDateTimeModule,
    NgbModule,
    SearchEmployerComponent,
    SearchJobSeekerComponent,
    AngularEditorModule,
    HttpClientModule ,
    InputComponent,
    ButtonComponent,
    SelectComponent,
    SelectMultiComponent,
    DateComponent,
    RadiobuttonComponent,
    CheckboxComponent,
    DynamicFieldDirective,
    DynamicFormComponent,
    MatExpansionModule,
    CreateFormComponent,
    NgxFileDropModule,
    UploadFilesComponent,
    TranslateModule,
  ],
  providers: [
    // { provide: MAT_LABEL_GLOBAL_OPTIONS, useValue: { float: 'auto' } }
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }
  ]
})
export class SharedModule { }
// AOT compilation support
export function httpTranslateLoader(http: HttpClient) {
  return new TranslateHttpLoader(http);
}