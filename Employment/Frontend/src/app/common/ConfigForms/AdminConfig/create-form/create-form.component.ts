import { Component, OnInit, ViewChild, AfterViewChecked, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray, MaxLengthValidator } from '@angular/forms';
import { DynamicFormComponent } from '../../components/dynamic-form/dynamic-form.component';
import { FieldConfig, Validator } from '../../field.interface';
import { ServiceConfigForm } from 'src/app/services/config-form.service';
import { ModelConfigForm, ModelFieldConfig } from 'src/app/interface/Model/ModelConfigForm.class';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { ConfigForm } from 'src/app/Enum/ConfigForm.enum';
import { ConfirmationDialogService } from 'src/app/common/confirmation-dialog/confirmation-dialog.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-create-form',
  templateUrl: './create-form.component.html',
  styleUrls: ['./create-form.component.css']
})
export class CreateFormComponent extends baseComponent implements OnInit {

  @ViewChild(DynamicFormComponent) form: DynamicFormComponent;
  regConfig: FieldConfig[] = [
    {
      type: "button",
      label: "Save",
      order: 100
    }
  ];

  @Input() pageTitle: string = 'Config Form Fields';
  @Input() formType: ConfigForm = 0;

  adminForm: FormGroup;
  get FormInputs(): FormArray {
    return <FormArray>this.adminForm.get('FormInputs');
  }
  get options(): FormArray {
    return <FormArray>this.adminForm.get('options');
  }

  constructor(private fb: FormBuilder,
    protected BLService: ServiceConfigForm,
    private confirmationDialogService: ConfirmationDialogService,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser,BLTranslate: TranslateService,) {
    super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);;
  }
  InitDynamicForm() {
    this.BLService.GetByType(this.formType).subscribe({
      next: lst => {
        this.regConfig = <FieldConfig[]>[];
        if (lst.length > 0) {
          this.FormInputs.removeAt(0);
        }

        lst.forEach(item => {

          var obj = new FieldConfig();

          obj.inputType = item.inputType;
          obj.label = item.label;
          obj.name = item.name;
          obj.options = item.options;
          obj.order = item.order;
          obj.type = item.type;
          obj.validations.push(new Validator(item.label));

          this.regConfig.push(obj);

          var ctl = this.fb.group({
            Order: [item.order, [Validators.required, Validators.max(100), Validators.min(0)]],
            Label: [item.label, [Validators.required, Validators.maxLength(50)]],
            Name: [item.name, [Validators.required, Validators.maxLength(50)]],
            Type: [item.type, [Validators.required]],
            InputType: [item.inputType],
            options: this.fb.array([]),
            IsRequired: true,
          });
          var opt = <FormArray>ctl.controls["options"];
          item.options.forEach(obj => {
            var yy = obj["option"];
            var xx = this.buildoptions(yy);
            opt.push(xx)
          });
          this.FormInputs.push(ctl);
        });
        this.form.fields = this.regConfig;
        this.form.createControl();

      },
      error: err => this.message.Error(err)
    });

  }
  ngOnInit() {
    this.adminForm = this.fb.group({
      FormInputs: this.fb.array([this.buildFormInputs()])
    });
    this.InitDynamicForm();
  }

  addFormInputs() {
    this.ConvertToForm();
    this.FormInputs.push(this.buildFormInputs());
  }
  removeFormInputs(idx) {
    this.confirmationDialogService.confirm("Are you sure you want to remove this?")
      .then((confirmed) => {
        if (!confirmed)
          return;
        if (this.FormInputs.controls.length > 1) {
          this.FormInputs.removeAt(idx);
        }
        this.ConvertToForm();
      });
  }

  buildFormInputs() {
    return this.fb.group({
      Order: ['', [Validators.required, Validators.max(100), Validators.min(0)]],
      Label: ['', [Validators.required, Validators.maxLength(50)]],
      Name: ['', [Validators.required, Validators.maxLength(50)]],
      Type: ['', [Validators.required]],
      InputType: ['text'],
      options: this.fb.array([]),
      IsRequired: true,
    });
  }
  onTypeChange(obj) {
    if (obj.get('Type').value == 'select' || obj.get('Type').value == 'selectmulti') {
      if (obj.get('options').controls.length == 0) {
        obj.get('options').push(this.buildoptions());
      }
    }
  }
  buildoptions(value: string = '') {
    return this.fb.group({
      option: [value, [Validators.required]]
    });
  }
  addOption(idx: number) {
    //this.options.push(this.buildoptions());
    const control = <FormArray>this.adminForm.get(['FormInputs', idx, 'options']);
    control.push(this.buildoptions());
  }
  getoptions(idx: number) {
    const opt = <FormArray>this.adminForm.get(['FormInputs', idx, 'options']);
    return opt?.controls;
  }
  ConvertToForm() {

    this.regConfig = <FieldConfig[]>[];
    // [{
    //   type: "button",
    //   label: "Save",
    //   order: 100
    // }];
    var frm = <FormArray>this.adminForm.get(['FormInputs']);

    frm.controls.forEach(element => {
      var ctl = new FieldConfig();

      ctl.order = element.get('Order').value;
      ctl.name = element.get('Name').value;
      ctl.label = element.get('Label').value;
      ctl.type = element.get('Type').value;
      ctl.inputType = element.get('InputType').value;

      if (ctl.type == 'select' || ctl.type == 'selectmulti')
        ctl.options = element.get('options').value;


      ctl.validations.push(new Validator(ctl.label));

      this.regConfig.push(ctl);
    });

    if (this.regConfig.length > 1) {
      this.regConfig = this.regConfig.sort((obj1, obj2) => {
        if (obj1.order > obj2.order) {
          return 1;
        }

        if (obj1.order < obj2.order) {
          return -1;
        }

        return 0;
      });
    }
    this.form.fields = this.regConfig;
    this.form.createControl();
  }
  Save(adminForm) {
    this.ConvertToForm();
    if (!adminForm.valid)
      return;

    var model = new ModelConfigForm();
    model.FormType = this.formType;
    model.Form = <ModelFieldConfig[]>[];
    this.regConfig.forEach(item => {

      var obj = new ModelFieldConfig();

      obj.inputType = item.inputType;
      obj.label = item.label;
      obj.name = item.name;
      obj.options = item.options;
      obj.order = item.order;
      obj.type = item.type;
      obj.validations = true;

      model.Form.push(obj);
    });
    this.BLService.Update(model).subscribe({
      next: response => {
        this.message.Success("Saved successfully.");
        this.BLServiceShowMessage.sendMessage(this.message);
      },
      error: err => this.message.Error(err)
    });
  }
}
