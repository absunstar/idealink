import { Component, OnInit } from '@angular/core';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import {  ModelReportJob } from 'src/app/interface/Model/ModelReportDates.class';
import { CompanyEmployers } from 'src/app/interface/Response/CompanyEmployer.class';
import { cGenericIdNameItem } from 'src/app/interface/Response/GenericIdName.class';
import { ReportJobCount } from 'src/app/interface/Response/Job.class';
import { ServiceJobFields } from 'src/app/services/job-fields.service';
import { ServiceJob } from 'src/app/services/job.service';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { ServiceCompany } from 'src/app/services/service-company.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';

@Component({
  selector: 'app-stats-job',
  templateUrl: './stats-job.component.html',
  styleUrls: ['./stats-job.component.css']
})
export class StatsJobComponent extends baseComponent implements OnInit {

  modelStartDate: NgbDateStruct;
  modelEndDate: NgbDateStruct;
  modelObj: ModelReportJob = new ModelReportJob();
  IsDatesCorrect: boolean = true;
  lstJobCount : ReportJobCount[] = [];
  lstCompanies: CompanyEmployers;
  LkupJobField: cGenericIdNameItem[];
  
  constructor(BLServiceShowMessage: ServiceShowMessage,
    BLJobSeekerLoginUser: ServiceLoginUser, BLTranslate: TranslateService,
    private BLServiceCompany: ServiceCompany,
    private BLJobFields: ServiceJobFields,
    private BLJob: ServiceJob,) {
    super(BLServiceShowMessage, BLJobSeekerLoginUser, BLTranslate)
  }

  ngOnInit(): void {
    this.loadData();
    this.BLServiceCompany.ListAnyCompany().subscribe({
      next: lst => {
        this.lstCompanies = lst;
      },
      error: err => this.message.Error(err)
    });
    this.BLJobFields.ListAnyJobFields().subscribe({
      next: lst => {
        this.LkupJobField = lst;
        
      },
      error: err => {
        this.message.Error(err);
        this.BLServiceShowMessage.sendMessage(this.message);
      }
    });
  }
  private loadData(): void {
    this.BLJob.ReportJobCount(this.modelObj).subscribe({
      next: obj => {
        this.lstJobCount = obj;
      },
      error: err => this.message.Error(err)
    });
  }
  onStartDateSelect(param) {
    this.modelObj.StartDate = new Date(param.year, param.month - 1, param.day + 1);
    if (this.modelObj.StartDate.toString() != "0001-01-01T00:00:00Z" &&
      this.modelObj.EndDate.toString() != "0001-01-01T00:00:00Z" ) {
      this.IsDatesCorrect = this.CompareDatesEqual(this.modelObj.StartDate, this.modelObj.EndDate);
    }
  }
  onEndDateSelect(param) {
    this.modelObj.EndDate = new Date(param.year, param.month - 1, param.day + 1);
    if (this.modelObj.StartDate.toString() != "0001-01-01T00:00:00Z" &&
      this.modelObj.EndDate.toString() != "0001-01-01T00:00:00Z" ) {
      this.IsDatesCorrect = this.CompareDatesEqual(this.modelObj.StartDate, this.modelObj.EndDate);
    }
  }
  Search() {
    this.loadData();
  }
}
