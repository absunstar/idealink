import { Component, OnInit, ViewChild } from '@angular/core';
import { ShowMessage } from 'src/app/interface/Model/ModelShowMessage.class';
import { ModelRequestRegister } from 'src/app/interface/Model/ModelRequestRegister.class';
import { NgForm } from '@angular/forms';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { cEntityPartnerItem } from 'src/app/interface/Response/EntityPartner.class';
import { ServiceEntityManagement } from 'src/app/services/entitymanagement.service';
import { ServiceMisc } from 'src/app/services/misc.service';
import { Router } from '@angular/router';
import { DynamicFormComponent } from 'src/app/common/ConfigForms/components/dynamic-form/dynamic-form.component';
import { FieldConfig, Validator } from 'src/app/common/ConfigForms/field.interface';
import { ServiceConfigForm } from 'src/app/services/config-form.service';
import { ConfigForm } from 'src/app/Enum/ConfigForm.enum';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent extends baseComponent implements OnInit {

  pageTitle: string = 'Request Register';
  message: ShowMessage = new ShowMessage();
  modelObj: ModelRequestRegister = new ModelRequestRegister();
  lstPartners: cEntityPartnerItem[];
  isClicked: boolean = false;

  @ViewChild(DynamicFormComponent) form: DynamicFormComponent;
  regConfig: FieldConfig[] = [];

  constructor(private BLServiceEntityManagement: ServiceEntityManagement,
    private BLServiceMisc: ServiceMisc,
    private router: Router,
    private BLServiceConfigForm: ServiceConfigForm,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser,BLTranslate: TranslateService,) {
    super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
  }

  ngOnInit(): void {
    this.BLServiceMisc.getEntityPartnerListAllActiveAnonymous().subscribe({
      next: lst => {
        this.lstPartners = lst;
      },
      error: err => this.message.Error(err)
    });
    this.InitDynamicForm();
  }
  InitDynamicForm() {
    this.BLServiceMisc.GetByType(ConfigForm.RequestRegister).subscribe({
      next: lst => {
        this.regConfig = <FieldConfig[]>[];
        lst.forEach(item => {

          var obj = new FieldConfig();

          obj.inputType = item.inputType;
          obj.label = item.label;
          obj.name = item.label;
          obj.options = item.options;
          obj.order = item.order;
          obj.type = item.type;
          obj.IsRequired = item.IsRequired;
          if (obj.IsRequired)
            obj.validations.push(new Validator(item.label));
          if (this.modelObj.data)
            obj.value = this.modelObj.data[item.label];

          this.regConfig.push(obj);
        });
        this.form.fields = this.regConfig;
        this.form.createControl();
      },
      error: err => this.message.Error(err)
    });

  }
  modelSaveBtn(modelForm: NgForm): void {
    if (!modelForm.valid)
      return;
    this.isClicked = true;
    this.modelObj.data = this.form.value;

    this.BLServiceMisc.RequestRegister(this.modelObj).subscribe({
      next: response => {
        this.message.Success(this.msgSavedSuccessfully);
        this.BLServiceShowMessage.sendMessage(this.message);
        this.router.navigate(['/']);
      },
      error: err => {
        this.message.Error(err)
        this.isClicked = false;
      }
    });

  }
}
