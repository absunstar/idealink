import { Component, OnInit, ViewChild } from '@angular/core';
import { cEntitySubPartnerItem } from 'src/app/interface/Response/EntitySubPartner.class';
import { cUserProfileItem } from 'src/app/interface/Response/UserProfile.class';
import { ServiceUserProfile } from 'src/app/services/userprofile.service';
import { ServiceEntityManagement } from 'src/app/services/entitymanagement.service';
import { cEntityPartnerItem } from 'src/app/interface/Response/EntityPartner.class';
import { DynamicFormComponent } from 'src/app/common/ConfigForms/components/dynamic-form/dynamic-form.component';
import { FieldConfig, Validator } from 'src/app/common/ConfigForms/field.interface';
import { ServiceConfigForm } from 'src/app/services/config-form.service';
import { ConfigForm } from 'src/app/Enum/ConfigForm.enum';
import { cTraineeItem } from 'src/app/interface/Response/Trainee.class';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.scss']
})
export class TestComponent extends baseComponent implements OnInit {
  modelObj: cTraineeItem = new cTraineeItem();
@ViewChild(DynamicFormComponent) dyForm: DynamicFormComponent;
regConfig: FieldConfig[] = [];

  constructor(private BLServiceUserProfile:ServiceUserProfile,
    private BLServiceConfigForm: ServiceConfigForm,
        private BLserviceEntity:ServiceEntityManagement,
        BLServiceShowMessage: ServiceShowMessage,
        BLServiceLoginUser: ServiceLoginUser,BLTranslate: TranslateService,){
          super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
    }
    
  ngOnInit() {
    this.InitDynamicForm();
  }
  InitDynamicForm() {
    this.BLServiceConfigForm.GetByType(ConfigForm.Trainee).subscribe({
      next: lst => {
        this.regConfig = <FieldConfig[]>[];
        lst.forEach(item => {

          var obj = new FieldConfig();

          obj.inputType = item.inputType;
          obj.label = item.label;
          obj.name = item.name;
          obj.options = item.options;
          obj.order = item.order;
          obj.type = item.type;
          obj.validations.push(new Validator(item.label));
          if (this.modelObj.data)
            obj.value = this.modelObj.data[item.name];

          this.regConfig.push(obj);
        });
        this.dyForm.fields = this.regConfig;
        this.dyForm .createControl();
      },
      error: err => this.message.Error(err)
    });

  }
  loadData(){
    
  }
}
