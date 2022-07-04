import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ModelJob } from 'src/app/interface/Model/ModelJob.class';
import { cGenericIdNameItem, cGenericSubItem } from 'src/app/interface/Response/GenericIdName.class';
import { cJobItem } from 'src/app/interface/Response/Job.class';
import { ActivatedRoute, Router } from '@angular/router';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { ServiceIndustry } from 'src/app/services/industry.service';
import { ServiceCountry } from 'src/app/services/country.service';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ServiceJob } from 'src/app/services/job.service';
import { ServiceQualification } from 'src/app/services/qualification.service';
import { ServiceYearsOfExperience } from 'src/app/services/years-of-experience.service';
import { ServiceJobFields } from 'src/app/services/job-fields.service';
import { ServiceUserProfile } from 'src/app/services/userprofile.service';
import { cUserProfileItem } from 'src/app/interface/Response/UserProfile.class';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { NgForm } from '@angular/forms';
import { JobStatus } from 'src/app/Enum/JobStatus.enum';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-employer-post-job',
  templateUrl: './employer-post-job.component.html',
  styleUrls: ['./employer-post-job.component.css']
})
export class EmployerPostJobComponent extends baseComponent implements OnInit, AfterViewInit {

  modelObj: ModelJob = new ModelJob();
  LkupCountry: cGenericIdNameItem[];
  LkupCity: cGenericSubItem[];
  LkupIndustry: cGenericSubItem[];
  LkupQualification: cGenericSubItem[];
  LkupExperience: cGenericSubItem[];
  LkupJobField: cGenericIdNameItem[];
  LkupJobSubField: cGenericSubItem[];
  modelIsCreate: boolean;
  objJob: cJobItem = new cJobItem();
  JobId: string;
  userProfile: cUserProfileItem;
  modelEndDate: NgbDateStruct;
  todayDate: Date = new Date();

  constructor(private BLCountry: ServiceCountry,
    private BLIndustry: ServiceIndustry,
    private BLQualification: ServiceQualification,
    private BLExperience: ServiceYearsOfExperience,
    private BLJobFields: ServiceJobFields,
    private BLUserProfile: ServiceUserProfile,
    private BLJob: ServiceJob,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser, BLTranslate: TranslateService,
    private router: Router,
    private route: ActivatedRoute) {
    super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
  }
  MapToModel(obj: cJobItem) {
    this.modelObj.Address = obj.Address;
    this.modelObj.CityId = obj.City._id;
    this.modelObj.CountryId = obj.Country._id;
    this.modelObj.IndustryId = obj.Industry._id;
    this.modelObj.JobFieldId = obj.JobField._id;
    this.modelObj.JobSubFieldId = obj.JobSubField._id;
    this.modelObj.QualificationId = obj.Qualification._id;
    this.modelObj.ExperienceId = obj.Experience._id;
    this.modelObj.Name = obj.Name;
    this.modelObj._id = obj._id;
    this.modelObj.CompanyId = obj.Company._id;
    this.modelObj.Deadline = obj.Deadline;
    this.modelObj.Description = obj.Description;
    this.modelObj.Skills = obj.Skills;
    this.modelObj.Benefits = obj.Benefits;
    this.modelObj.Gender = obj.Gender;
    this.modelObj.Type = obj.Type;
    this.modelObj.Remuneration = obj.Remuneration;

    var d = new Date(this.modelObj.Deadline);
    this.modelEndDate = {
      year: d.getUTCFullYear(), month: d.getUTCMonth() + 1
      , day: d.getUTCDate()
    };
  }
  ngAfterViewInit() {

  }
  LoadJob() {
    this.BLJob.getGetByid(this.JobId).subscribe({
      next: obj => {
        this.objJob = obj;
        this.MapToModel(obj);
      },
      error: err => {
        this.message.Error(err);
        this.BLServiceShowMessage.sendMessage(this.message);
      }
    });
  }
  ngOnInit(): void {
    this.modelIsCreate = true;
    const param = this.route.snapshot.paramMap.get('Id');
    if (param) {
      this.modelIsCreate = false;
      this.JobId = param;
      this.LoadJob();
    }
    this.BLUserProfile.GetMyUser().subscribe({
      next: obj => {
        this.userProfile = obj;
        this.userProfile.MyCompanies = this.userProfile.MyCompanies.filter(x=>x.IsApproved == true);
      },
      error: err => {
        this.message.Error(err);
        this.BLServiceShowMessage.sendMessage(this.message);
      }
    });
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
    this.BLQualification.getListActive().subscribe({
      next: lst => {
        this.LkupQualification = lst;
      },
      error: err => {
        this.message.Error(err);
        this.BLServiceShowMessage.sendMessage(this.message);
      }
    });
    this.BLExperience.getListActive().subscribe({
      next: lst => {
        this.LkupExperience = lst;
      },
      error: err => {
        this.message.Error(err);
        this.BLServiceShowMessage.sendMessage(this.message);
      }
    });
    this.BLJobFields.getListActive().subscribe({
      next: lst => {
        this.LkupJobField = lst;
        this.onJobFieldSelect();
      },
      error: err => {
        this.message.Error(err);
        this.BLServiceShowMessage.sendMessage(this.message);
      }
    });
  }
  onCountrySelect() {
    this.LkupCity = [];

    var sub = this.LkupCountry?.find(x => x._id == this.modelObj.CountryId)?.subItems;
    if (!sub || sub.length == 0)
      return;

    this.LkupCity = sub.filter(y => y.IsActive == true).sort((a, b) => a.Name.localeCompare(b.Name));
  }
  onJobFieldSelect() {
    this.LkupJobSubField = [];

    var sub = this.LkupJobField?.find(x => x._id == this.modelObj.JobFieldId)?.subItems;
    if (!sub || sub.length == 0)
      return;

    this.LkupJobSubField = sub.filter(y => y.IsActive == true).sort((a, b) => a.Name.localeCompare(b.Name));
  }
  onEndDateSelect(param) {
    this.modelObj.Deadline = new Date(param.year, param.month - 1, param.day);
  }
  onCancel() {
    this.router.navigate(['Employer/ManageMyJobs']);
  }
  SaveDraft(modelForm: NgForm) {
    // if (!modelForm.valid)
    //   return;

    if (this.modelIsCreate) {
      this.BLJob.create(this.modelObj).subscribe({
        next: response => {
          
          this.message.Success("Saved successfully.");
          this.BLServiceShowMessage.sendMessage(this.message);
          this.router.navigate(['Employer/PostJob/' + response]);
          //this.router.navigate(['Employer/ManageMyJobs']);
        },
        error: err => this.message.Error(err)
      });
    }
    else {
      this.BLJob.updateDraft(this.modelObj).subscribe({
        next: response => {
          this.message.Success("Saved successfully.");
          this.BLServiceShowMessage.sendMessage(this.message);
          this.router.navigate(['Employer/ManageMyJobs']);
        },
        error: err => this.message.Error(err)
      });
    }
  }
  SaveDraftPublish(modelForm: NgForm) {
    if (!modelForm.valid || !this.modelIsCreate)
      return;
    this.BLJob.updateDraftPublish(this.modelObj).subscribe({
      next: response => {
        this.message.Success("Saved successfully and sent to admin for approval.");
        this.BLServiceShowMessage.sendMessage(this.message);
        this.router.navigate(['Employer/ManageMyJobs']);
      },
      error: err => this.message.Error(err)
    });
  }
  SavePublish(modelForm: NgForm) {
    if (!modelForm.valid || this.modelIsCreate)
      return;
    this.BLJob.updatePublish(this.modelObj).subscribe({
      next: response => {
        this.message.Success("Saved successfully and sent to admin for approval.");
        this.BLServiceShowMessage.sendMessage(this.message);
        this.router.navigate(['Employer/ManageMyJobs']);
      },
      error: err => this.message.Error(err)
    });
  }
  CanSaveDraft() {
    return true;
    //return this.objJob.Status != JobStatus.Closed
  }
  CanSavePublish() {
    return !this.modelIsCreate //&& this.objJob.Status != JobStatus.Closed
  }
  CanSaveDraftPublish(){
    return this.modelIsCreate //&& this.objJob.Status != JobStatus.Closed
  }
}
