import { Component, OnInit, ViewChild } from '@angular/core';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ModelJobFairRegisteration } from 'src/app/interface/Model/ModelJobFairRegisteration.class';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { ServiceJobFair } from 'src/app/services/job-fair.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { Router, ActivatedRoute } from '@angular/router';
import { ServiceJobSeeker } from 'src/app/services/job-seeker.service';
import { cJobSeekerItem } from 'src/app/interface/Response/JobSeeker.class';
import { cJobFairItem } from 'src/app/interface/Response/JobFair.class';
import { ConfigForm } from 'src/app/Enum/ConfigForm.enum';
import { FieldConfig, Validator } from 'src/app/common/ConfigForms/field.interface';
import { DynamicFormComponent } from 'src/app/common/ConfigForms/components/dynamic-form/dynamic-form.component';
import { ServiceConfigForm } from 'src/app/services/config-form.service';
import { takeWhile, take } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent extends baseComponent implements OnInit {

  modelIsCreate: boolean = true;
  modelNameEdited: string = '';
  modelObj: ModelJobFairRegisteration = new ModelJobFairRegisteration();
  modelDOBDate: NgbDateStruct;
  modelId: string;
  isRegstered: boolean = false;
  isChecked: boolean = false;
  modelUser: cJobSeekerItem;
  jobfair: cJobFairItem;
  @ViewChild(DynamicFormComponent) form: DynamicFormComponent;
  regConfig: FieldConfig[] = [];

  constructor(
    private BLJobFair: ServiceJobFair,
    private BLJobSeeker: ServiceJobSeeker,
    private BLServiceConfigForm: ServiceConfigForm,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser,BLTranslate: TranslateService,
    private router: Router,
    private route: ActivatedRoute) {
    super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate)
  }
  ngOnInit(): void {
    this.InitDynamicForm();
    const param = this.route.snapshot.paramMap.get('Id');
    if (param) {
      //this.modelIsCreate = false;
      this.modelId = param;

    }
    this.modelObj.JobFairId = param;
    
    if (this.IsAdmin) {

    }
    else if (this.isJobSeeker) {
      this.checkUser();
      this.LoadUser(this.userId);
    }
    else {
      this.BLServiceLoginUser.UserIsJobSeekerChanged
        .pipe(take(1))
        .subscribe({
          next: flag => {
            if (flag) {
              this.checkUser();
              this.LoadUser(this.userId);
            }
          },
          error: err => this.message.Error(err)
        });
    }
    this.BLJobFair.getGetByid(this.modelObj.JobFairId).subscribe({
      next: obj => {
        this.jobfair = obj;
        this.modelNameEdited = obj.Name;
      },
      error: err => this.message.Error(err)
    });
  }
  InitDynamicForm() {
    this.BLServiceConfigForm.GetByType(ConfigForm.JobFairRegister).subscribe({
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

          this.regConfig.push(obj);
        });
        this.form.fields = this.regConfig;
        this.form.createControl();
      },
      error: err => this.message.Error(err)
    });

  }
  checkUser() {
    this.BLJobFair.CheckRegister(this.modelObj.JobFairId).subscribe({
      next: obj => {
        this.isRegstered = obj;
      },
      error: err => this.message.Error(err)
    });
  }

  LoadUser(Id: string) {
    this.modelObj.UserId = Id;

    this.BLJobSeeker.getGetByid(Id).subscribe({
      next: obj => {
        this.modelUser = obj;
        this.modelObj.Name = obj.Name;
        this.modelObj.JobTitle = obj.JobTitle;
        this.modelObj.Email = obj.Email.toLowerCase();
        this.modelObj.DOB = obj.DOB;

        var d = new Date(this.modelObj.DOB);
        this.modelDOBDate = {
          year: d.getUTCFullYear(), month: d.getUTCMonth() + 1
          , day: d.getUTCDate()
        };
      },
      error: err => this.message.Error(err)
    });
  }
  onDateSelect(param) {
    this.modelObj.DOB = new Date(param.year, param.month - 1, param.day + 1);
  }
  Save(modelForm) {
    if (!modelForm.valid)
      return;

    this.modelObj.data = this.form.value;

    this.BLJobFair.Register(this.modelObj).subscribe({
      next: response => {
        this.message.Success("Registered successfully.");
        this.BLServiceShowMessage.sendMessage(this.message);
        if (this.isJobSeeker)
          this.router.navigate(['JobFair/List']);
        else {
          modelForm.reset();
          this.InitDynamicForm();
        }
      },
      error: err => this.message.Error(err)
    });
  }
  OnSelectJobSeeker(idx: string) {
    this.LoadUser(idx);
  }
}
