import { Validators } from "@angular/forms";

export class Validator {
  constructor(strName:string)
  {
    this.name = strName;
    this.validator = Validators.required;
    this.message = "Please enter " + strName + ".";
  }
  name: string;
  validator: Validators;
  message: string;
}
export class FieldConfig {
  constructor(){
    this.validations = <Validator[]>[];
  }
  order?: number;
  label?: string;
  name?: string;
  inputType?: string;
  options?: options[];
  collections?: any;
  type: string;
  value?: any;
  IsRequired:boolean;
  validations?: Validator[];
  
}
export class options{
  option: string;
}
