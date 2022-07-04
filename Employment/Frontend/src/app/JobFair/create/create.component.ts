import { Component, OnInit, AfterViewInit, ViewChild, AfterViewChecked } from '@angular/core';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ModelJobFair } from 'src/app/interface/Model/ModelJobFair.class';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { Router, ActivatedRoute } from '@angular/router';
import { ServiceJobFair } from 'src/app/services/job-fair.service';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { DynamicFormComponent } from 'src/app/common/ConfigForms/components/dynamic-form/dynamic-form.component';
import { FieldConfig, Validator } from 'src/app/common/ConfigForms/field.interface';
import { ServiceConfigForm } from 'src/app/services/config-form.service';
import { ConfigForm } from 'src/app/Enum/ConfigForm.enum';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-create',
  templateUrl: './create.component.html',
  styleUrls: ['./create.component.css']
})
export class CreateComponent extends baseComponent implements OnInit {

  modelIsCreate: boolean = true;
  modelNameEdited: string = '';
  modelObj: ModelJobFair = new ModelJobFair();
  modelEventDate: NgbDateStruct;
  modelId: string;

  @ViewChild(DynamicFormComponent) form: DynamicFormComponent;
  regConfig: FieldConfig[] = []; 

  constructor(
    private BLJobFair: ServiceJobFair,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser,BLTranslate: TranslateService,
    private BLServiceConfigForm: ServiceConfigForm,
    private router: Router,
    private route: ActivatedRoute) {
    super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
  }

  InitDynamicForm() {
    this.BLServiceConfigForm.GetByType(ConfigForm.JobFairCreate).subscribe({
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
        this.form.fields = this.regConfig;
        this.form.createControl();
      },
      error: err => this.message.Error(err)
    });

  }

  ngOnInit(): void {
    const param = this.route.snapshot.paramMap.get('Id');
   
    if (param) {
      this.modelIsCreate = false;
      this.modelId = param;
      this.modelObj._id = param;

      this.BLJobFair.getGetByid(param).subscribe({
        next: obj => {
          this.modelObj = obj;
          this.modelNameEdited = obj.Name;
          
          var d = new Date(this.modelObj.EventDate);
          this.modelEventDate = {
            year: d.getUTCFullYear(), month: d.getUTCMonth() + 1
            , day: d.getUTCDate() +1
          };
        },
        error: err => this.message.Error(err)
      });
    }
    this.InitDynamicForm();
  }
  onDateSelect(param) {
    this.modelObj.EventDate = new Date(param.year, param.month - 1, param.day );
  }
  Save(modelForm) {

    if (!modelForm.valid)
      return;

    this.modelObj.data = this.form.value;

    if (this.modelIsCreate) {
      this.BLJobFair.create(this.modelObj).subscribe({
        next: response => {
          this.message.Success("Saved successfully.");
          this.BLServiceShowMessage.sendMessage(this.message);
          this.router.navigate(['JobFair/List']);
        },
        error: err => this.message.Error(err)
      });
    }
    else {
      this.BLJobFair.update(this.modelObj).subscribe({
        next: response => {
          this.message.Success("Saved successfully.");
          this.BLServiceShowMessage.sendMessage(this.message);
          this.router.navigate(['JobFair/List']);
        },
        error: err => this.message.Error(err)
      });
    }
  }
  onLocationChange(e, fileType: string) {
    if (fileType == "1") {
      this.modelObj.IsOnline = true;
    }
    else {
      this.modelObj.IsOnline = false;
    }
  }
}
