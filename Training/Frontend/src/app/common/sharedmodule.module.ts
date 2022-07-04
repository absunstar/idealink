import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatButtonModule, MatTableModule, MatInputModule, MatSelectModule, MatFormFieldModule, MatIconModule, MatRadioModule, MatCheckboxModule, MAT_LABEL_GLOBAL_OPTIONS, MatMenuModule, MatMenu, MatTabsModule, MatTooltipModule, MatAutocompleteModule, MatSlideToggleModule, MatExpansionModule } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { AtomSpinnerModule } from 'angular-epic-spinners';
import { OwlDateTimeModule, OwlNativeDateTimeModule } from 'ng-pick-datetime';
//import { OwlTooltipModule } from 'owl-ng';

import { CustomPaginationComponent } from './pagination/components/custom-pagination/custom-pagination.component';
import { SearchPartnerComponent } from './formcontrols/search-partner/search-partner.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SearchSubPartnerComponent } from './formcontrols/search-sub-partner/search-sub-partner.component';
import { SearchSubPartnerEntityComponent } from './formcontrols/search-sub-partner-entity/search-sub-partner-entity.component';
import { AutoCompeletListGenericComponent } from './FormControls/auto-compelet-list-generic/auto-compelet-list-generic.component';
import { SearchPartnerEntityComponent } from './FormControls/search-partner-entity/search-partner-entity.component';
import { AddTraineeComponent } from './add-trainee/add-trainee.component';
import { SearchTraineeComponent } from './FormControls/search-trainee/search-trainee.component';
import { CreateFormComponent } from './ConfigForms/AdminConfig/create-form/create-form.component';
import { BrowserModule } from '@angular/platform-browser';
import { ConfirmationDialogComponent } from './confirmation-dialog/confirmation-dialog.component';
import { InputComponent } from './ConfigForms/components/input/input.component';
import { ButtonComponent } from './ConfigForms/components/button/button.component';
import { SelectComponent } from './ConfigForms/components/select/select.component';
import { SelectMultiComponent } from './ConfigForms/components/SelectMulti/selectmulti.component';
import { DynamicFieldDirective } from './ConfigForms/components/dynamic-field/dynamic-field.directive';
import { DynamicFormComponent } from './ConfigForms/components/dynamic-form/dynamic-form.component';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { HttpClient } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { AngularEditorModule } from '@kolkov/angular-editor';

@NgModule({
  declarations: [
    CustomPaginationComponent,
    SearchPartnerComponent,
    SearchPartnerEntityComponent,
    SearchSubPartnerComponent,
    SearchSubPartnerEntityComponent,
    AutoCompeletListGenericComponent,
    AddTraineeComponent,
    SearchTraineeComponent,
    CreateFormComponent,
    ConfirmationDialogComponent,
    InputComponent,
    ButtonComponent,
    SelectComponent,
    SelectMultiComponent,
    DynamicFieldDirective,
    DynamicFormComponent,
  ],
  imports: [
    OwlDateTimeModule, 
    OwlNativeDateTimeModule,
    BrowserModule,
    CommonModule,
    FormsModule,
    AtomSpinnerModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
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
    MatExpansionModule,
    MatAutocompleteModule,
    AngularEditorModule,
    TypeaheadModule.forRoot(),
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: httpTranslateLoader,
        deps: [HttpClient]
      }
    })
  ],
  exports: [
    AutoCompeletListGenericComponent,
    CustomPaginationComponent,
    SearchPartnerComponent,
    SearchPartnerEntityComponent,
    SearchSubPartnerComponent,
    SearchSubPartnerEntityComponent,
    SearchTraineeComponent,
    AddTraineeComponent,
    CommonModule,
    FormsModule,
    BrowserAnimationsModule,
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
    ReactiveFormsModule,
    MatAutocompleteModule,
    MatMenuModule,
    MatSlideToggleModule,
    AtomSpinnerModule,
    TypeaheadModule,
    OwlDateTimeModule, 
    OwlNativeDateTimeModule,
    BrowserAnimationsModule,
    CreateFormComponent,
    DynamicFieldDirective,
    DynamicFormComponent,
    MatExpansionModule,
    TranslateModule,
    AngularEditorModule
  ],
  providers: [
    // { provide: MAT_LABEL_GLOBAL_OPTIONS, useValue: { float: 'auto' } }
  ]
})
export class SharedModule { }
// AOT compilation support
export function httpTranslateLoader(http: HttpClient) {
  return new TranslateHttpLoader(http);
}