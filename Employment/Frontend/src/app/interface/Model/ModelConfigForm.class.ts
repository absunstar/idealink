import { ConfigForm } from 'src/app/Enum/ConfigForm.enum';

export class ModelConfigForm{

    FormType: ConfigForm;
    Form: ModelFieldConfig[];
}

export class ModelFieldConfig {

    order?: number;
    label?: string;
    name?: string;
    inputType?: string;
    options?: ModelOptions[];
    collections?: any;
    type: string;
    value?: any;
    validations?: boolean;
  }
  export class ModelOptions{
    option: string;
  }
  