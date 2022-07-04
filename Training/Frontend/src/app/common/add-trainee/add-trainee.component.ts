import { Component, OnInit, AfterViewInit, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { cTraineeItem } from 'src/app/interface/Response/Trainee.class';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgForm } from '@angular/forms';
import { ModelTrainee } from 'src/app/interface/Model/ModelTrainee.class';
import { ServiceTrainee } from 'src/app/services/trainee.service';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { DynamicFormComponent } from '../ConfigForms/components/dynamic-form/dynamic-form.component';
import { FieldConfig, Validator } from '../ConfigForms/field.interface';
import { ConfigForm } from 'src/app/Enum/ConfigForm.enum';
import { ActivatedRoute, Router, NavigationEnd } from '@angular/router';
import { ServiceMisc } from 'src/app/services/misc.service';
import { filter, map, mergeMap } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'cust-add-trainee',
  templateUrl: './add-trainee.component.html',
  styleUrls: ['./add-trainee.component.css']
})
export class AddTraineeComponent extends baseComponent implements AfterViewInit, OnInit {
  IsCreate: boolean = false;
  IsEdit: boolean = false;
  IsModelTemplate: boolean = false;
  obj: cTraineeItem;
  TrainingId: string = "";
  modelTraineeId: string = "";
  isPageRegister: boolean = false;
  //@Output() Reload: EventEmitter<boolean> = new EventEmitter<boolean>();

  emailPattern = "^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$";
  modelIsCreate: boolean = true;
  isClicked: boolean = false;
  modelNameEdited: string = '';
  modelObj: cTraineeItem = new cTraineeItem();
  IsLoggedInTrainee:boolean = false;

  @ViewChild(DynamicFormComponent) form: DynamicFormComponent;
  regConfig: FieldConfig[] = [];

  constructor(private modalService: NgbModal,
    private router: Router,
    private BLServiceMisc: ServiceMisc,
    private BLServiceTrainee: ServiceTrainee,
    private route: ActivatedRoute,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser, BLTranslate: TranslateService,) {
    super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
    console.log(this.router.url);
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd),
      map(() => this.route),
      map(route => {
        while (route.firstChild) route = route.firstChild
        return route
      }),
      filter(route => route.outlet === 'primary'),
      mergeMap(route => route.data)
    ).subscribe(data => {
      this.isPageRegister = data.PageType === 'register';
    })
  }

   async ngOnInit() {
    // var GetRoleFunction =  () => {
    //   var self = this;
    //   return new Promise((resolve) => {
    //     self.BLServiceLoginUser.UserIsTraineeChanged.subscribe(obj => {
    //       resolve(obj);
    //     });
    //   });
    // }
    // 
    var IsTrainee = this.router.url.toLocaleLowerCase() === "/myprofile";
    
    this.IsLoggedInTrainee = <any>IsTrainee;
    
    this.InitData();
    this.createBtn();
  }
  ngAfterViewInit(): void {
    this.InitDynamicForm();
  }

  InitData() {
    if (this.IsLoggedInTrainee) {
      this.IsCreate = false;
      this.IsEdit = true;

      this.BLServiceTrainee.getMyProfile().subscribe({
        next: response => {
          this.modelObj = response;
          this.openBackDropCustomClass(this.modelObj);
          this.InitDynamicForm();
        },
        error: err => {
          this.message.Error(err)
        }
      });
    }
    else {
      //{ path: 'Trainee/Create/:IsCreate/:TraineeId/:TrainingId', component: AddTraineeComponent },
      const paramCreate = this.route.snapshot.paramMap.get('IsCreate');
      if (paramCreate) {
        this.IsCreate = paramCreate == "1";
        this.IsEdit = paramCreate == "0";

        if (this.IsCreate) {
          this.createBtn();
        }
      }
      const paramTraineeId = this.route.snapshot.paramMap.get('TraineeId');
      if (paramTraineeId && paramTraineeId != "0" && !this.isPageRegister) {
        this.modelTraineeId = paramTraineeId;
        //Load trainee Data
        this.BLServiceTrainee.getGetByid(this.modelTraineeId).subscribe({
          next: response => {
            this.modelObj = response;
            this.openBackDropCustomClass(this.modelObj);
            this.InitDynamicForm();
          },
          error: err => {
            this.message.Error(err)
          }
        });
      }
      const paramTrainingId = this.route.snapshot.paramMap.get('TrainingId');
      if (paramTrainingId && paramTrainingId != "0" && !this.isPageRegister) {
        this.TrainingId = paramTrainingId;
      }
    }
  }
  InitDynamicForm() {
    this.BLServiceMisc.GetByType(ConfigForm.Trainee).subscribe({
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
          obj.IsRequired = item.IsRequired;
          if (obj.IsRequired)
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
  createBtn(): void {

    this.modelObj = new cTraineeItem;
    this.modelIsCreate = true;
    this.isClicked = false;
    this.modelNameEdited = "";
    //this.modalService.open(content, { backdropClass: 'light-blue-backdrop', centered: true });
  }
  modelSaveBtn(modelForm: NgForm): void {

    if (!modelForm.valid)
      return;

    this.isClicked = true;
    var model = new ModelTrainee();
    model.Id = this.modelObj.Id;
    model.Name = this.modelObj.Name;
    model.NationalId = this.modelObj.NationalId;
    model.Gender = this.modelObj.Gender.toString();
    model.IdType = this.modelObj.IdType.toString();
    model.Mobile = this.modelObj.Mobile;
    model.Email = this.modelObj.Email;
    model.DOB = this.modelObj.DOB;
    model.data = this.form.value;

    if (this.IsLoggedInTrainee) {
      this.BLServiceTrainee.updateMyProfile(model).subscribe({
        next: response => {
          this.message.Success(this.msgSavedSuccessfully);
          this.BLServiceShowMessage.sendMessage(this.message);
          //this.Reload.emit(true);
          this.isClicked = false;
        },
        error: err => {
          this.message.Error(err);
          this.isClicked = false;
        }
      });
    }
    else {
      if (this.modelIsCreate) {
        model.TrainingId = this.TrainingId;
        this.BLServiceTrainee.create(model).subscribe({
          next: response => {
            this.message.Success(this.msgSavedSuccessfully);
            this.BLServiceShowMessage.sendMessage(this.message);
            //this.Reload.emit(true);
            this.isClicked = false;
            this.OnRedrirectBack();
          },
          error: err => {
            this.message.Error(err)
            this.isClicked = false;
          }
        });
      }
      else {
        this.BLServiceTrainee.update(model).subscribe({
          next: response => {
            this.message.Success(this.msgSavedSuccessfully);
            this.BLServiceShowMessage.sendMessage(this.message);
            //this.Reload.emit(true);
            this.isClicked = false;
            this.OnRedrirectBack();
          },
          error: err => {
            this.message.Error(err);
            this.isClicked = false;
          }
        });
      }
    }
  }
  openBackDropCustomClass(obj: cTraineeItem) {

    this.modelObj = obj;
    this.modelIsCreate = false;
    this.isClicked = false;
    this.modelNameEdited = obj.Name;
    //this.modalService.open(content, { backdropClass: 'light-blue-backdrop', centered: true });
  }
  OnRedrirectBack() {
    if (this.isPageRegister) {
      this.router.navigate(['/']);
    }
    else if (this.TrainingId != "") {
      this.router.navigate(['/Training/Trainees/' + this.TrainingId]);
    }
    else {
      this.router.navigate(['/Account/Trainee']);
    }
  }
}
