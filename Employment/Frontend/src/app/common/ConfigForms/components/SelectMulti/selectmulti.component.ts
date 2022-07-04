import { Component, OnInit } from "@angular/core";
import { FormGroup } from "@angular/forms";
import { FieldConfig } from "../../field.interface";
@Component({
  selector: "app-select",
  template: `
<mat-form-field class="demo-full-width margin-top" [formGroup]="group">
<mat-select placeholder="{{field.label}} *" [formControlName]="field.name" multiple>
<mat-option *ngFor="let item of field.options" [value]="item.option">{{item.option}}</mat-option>
</mat-select>
<ng-container *ngFor="let validation of field.validations;" ngProjectAs="mat-error">
<mat-error *ngIf="group.get(field.name).errors?.required">Please select {{field.label}}.</mat-error>
</ng-container>
</mat-form-field>
`,
  styles: []
})
export class SelectMultiComponent implements OnInit {
  field: FieldConfig;
  group: FormGroup;
  constructor() {}
  ngOnInit() {}
}
