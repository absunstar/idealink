import { Component, OnInit, ViewChild } from '@angular/core';
import { cGenericIdNameItem, cGenericSubItem } from 'src/app/interface/Response/GenericIdName.class';
import { ServiceCountry } from 'src/app/services/country.service';
import { ServiceIndustry } from 'src/app/services/industry.service';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { ModelCompany } from 'src/app/interface/Model/ModelCompany.class';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ServiceCompany } from 'src/app/services/service-company.service';
import { cCompanyItem } from 'src/app/interface/Response/Company.class';
import { TranslateService } from '@ngx-translate/core';
import { DynamicFormComponent } from 'src/app/common/ConfigForms/components/dynamic-form/dynamic-form.component';
import { FieldConfig, Validator } from 'src/app/common/ConfigForms/field.interface';
import { ConfigForm } from 'src/app/Enum/ConfigForm.enum';
import { ServiceConfigForm } from 'src/app/services/config-form.service';

@Component({
  selector: 'app-empoyer-profile',
  templateUrl: './empoyer-profile.component.html',
  styleUrls: ['./empoyer-profile.component.css']
})
export class EmpoyerProfileComponent extends baseComponent implements OnInit {

  LkupCountry: cGenericIdNameItem[];
  LkupCity: cGenericSubItem[];
  LkupIndustry: cGenericSubItem[];
  modelObj: ModelCompany = new ModelCompany();
  modelEstablishDate: NgbDateStruct;
  todayDate: Date = new Date();
  modelIsCreate: boolean;
  objProfile: cCompanyItem;
  profileId: string;
  IsUploading: boolean = false;
  FileName: string;
  dateOld: NgbDateStruct;
  @ViewChild(DynamicFormComponent) form: DynamicFormComponent;
  regConfig: FieldConfig[] = [];

  constructor(private BLCountry: ServiceCountry,
    private BLIndustry: ServiceIndustry,
    private BLCompany: ServiceCompany,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser, BLTranslate: TranslateService,
    private router: Router,
    private BLServiceConfigForm: ServiceConfigForm,
    private route: ActivatedRoute) {
    super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
  }
  InitDynamicForm() {
    this.BLServiceConfigForm.GetByType(ConfigForm.Company).subscribe({
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
  MapToModel(obj: cCompanyItem) {
    this.modelObj.About = obj.About;
    this.modelObj.Address = obj.Address;
    this.modelObj.CityId = obj.City._id;
    this.modelObj.CountryId = obj.Country._id;
    this.modelObj.Email = obj.Email;
    this.modelObj.Establish = obj.Establish;
    this.modelObj.IndustryId = obj.Industry._id;
    this.modelObj.Name = obj.Name;
    this.modelObj.Phone = obj.Phone;
    this.modelObj.SocialFacebook = obj.SocialFacebook;
    this.modelObj.SocialGooglePlus = obj.SocialGooglePlus;
    this.modelObj.SocialLinkedin = obj.SocialLinkedin;
    this.modelObj.SocialGooglePlus = obj.SocialGooglePlus;
    this.modelObj.SocialTwitter = obj.SocialTwitter;
    this.modelObj.Website = obj.Website;
    this.modelObj._id = obj._id;
    this.modelObj.data = obj.data;
    
    var d = new Date(this.modelObj.Establish);
    this.modelEstablishDate = {
      year: d.getUTCFullYear(), month: d.getUTCMonth() + 1
      , day: d.getUTCDate() + 1
    };
  }
  ngOnInit(): void {
    this.modelIsCreate = true;
    const param = this.route.snapshot.paramMap.get('Id');
    if (param) {
      this.modelIsCreate = false;
      this.profileId = param;
      this.BLCompany.getGetByid(param).subscribe({
        next: obj => {
          this.objProfile = obj;
          this.MapToModel(obj);
          this.onCountrySelect();
        },
        error: err => {
          this.message.Error(err);
          this.BLServiceShowMessage.sendMessage(this.message);
        }
      });
    }
    this.BLCountry.getListActive().subscribe({
      next: lst => {
        this.LkupCountry = lst;
        this.onCountrySelect();
      },
      error: err => {
        this.message.Error(err);
        this.BLServiceShowMessage.sendMessage(this.message);
      }
    });
    this.BLIndustry.getListActive().subscribe({
      next: lst => {
        this.LkupIndustry = lst;
      },
      error: err => {
        this.message.Error(err);
        this.BLServiceShowMessage.sendMessage(this.message);
      }
    });
    var d = new Date();
    this.dateOld = {
      year: d.getUTCFullYear() - 100, month: d.getUTCMonth() + 1
      , day: d.getUTCDate()
    };
    this.InitDynamicForm();
  }
  modelSaveBtn(modelForm: NgForm): void {
    if (!modelForm.valid)
      return;

    this.modelObj.data = this.form.value;

    if (this.modelIsCreate) {
      this.BLCompany.create(this.modelObj).subscribe({
        next: response => {
          this.message.Success("Saved successfully and waiting admin approval.");
          this.BLServiceShowMessage.sendMessage(this.message);
          this.router.navigate(['Employer/ManageMyCompanies']);
        },
        error: err => this.message.Error(err)
      });
    }
    else {
      this.BLCompany.update(this.modelObj).subscribe({
        next: response => {
          this.message.Success("Saved successfully and waiting admin approval.");
          this.BLServiceShowMessage.sendMessage(this.message);
          this.router.navigate(['Employer/ManageMyCompanies']);
        },
        error: err => this.message.Error(err)
      });
    }
  }
  onCountrySelect() {
    this.LkupCity = [];

    var sub = this.LkupCountry?.find(x => x._id == this.modelObj.CountryId).subItems;
    if (sub.length == 0)
      return;

    this.LkupCity = sub.filter(y => y.IsActive == true).sort((a, b) => a.Name.localeCompare(b.Name));
  }
  onEstDateSelect(param) {
    this.modelObj.Establish = new Date(param.year, param.month - 1, param.day);
  }
  onCancel() {
    this.router.navigate(['Employer/ManageMyCompanies']);
  }
  UploadStatus(status) {
    this.IsUploading = status;
  }
  OnFileUpload(filename) {
    this.FileName = filename;
    this.modelObj.CompanyLogo = filename;
  }
}
