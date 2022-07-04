import { Component, OnInit } from '@angular/core';
import { NgbDateStruct, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ModelReportDates } from 'src/app/interface/Model/ModelReportDates.class';
import { ReportJobSeekerGender } from 'src/app/interface/Response/ReportJobSeekerGender.class';
import { ServiceApply } from 'src/app/services/apply.service';
import { ServiceJobSeeker } from 'src/app/services/job-seeker.service';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { ServiceCompany } from 'src/app/services/service-company.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';

@Component({
  selector: 'app-stats',
  templateUrl: './stats.component.html',
  styleUrls: ['./stats.component.css']
})
export class StatsComponent extends baseComponent implements OnInit {

  modelStartDate: NgbDateStruct;
  modelEndDate: NgbDateStruct;
  modelObj: ModelReportDates = new ModelReportDates();
  IsDatesCorrect: boolean = true;

  reportCountJobSeeker: number = 0;
  reportCountCompany: number = 0;
  reportCountGender: ReportJobSeekerGender = new ReportJobSeekerGender();
  reportCountHired: number = 0;
  
  constructor( BLServiceShowMessage: ServiceShowMessage,
    BLJobSeekerLoginUser: ServiceLoginUser, BLTranslate: TranslateService,
    private BLJobSeeker: ServiceJobSeeker,
    private BLServiceCompany: ServiceCompany,
    private BLServiceApply: ServiceApply) {
    super(BLServiceShowMessage, BLJobSeekerLoginUser, BLTranslate)
   }

  ngOnInit(): void {
    this.loadData();
  }
  private loadData(): void {
    this.BLJobSeeker.ReportJobSeekerCount(this.modelObj).subscribe({
      next: obj => {
        this.reportCountJobSeeker = obj;
      },
      error: err => this.message.Error(err)
    });
    this.BLJobSeeker.ReportJobSeekerGenderCount(this.modelObj).subscribe({
      next: obj => {
        this.reportCountGender = obj;
      },
      error: err => this.message.Error(err)
    });
    this.BLServiceCompany.ReportCompanyCount(this.modelObj).subscribe({
      next: obj => {
        this.reportCountCompany = obj;
      },
      error: err => this.message.Error(err)
    });
    this.BLServiceApply.ReportJobSeekerHiredCount(this.modelObj).subscribe({
      next: obj => {
        this.reportCountHired = obj;
      },
      error: err => this.message.Error(err)
    });
  }
  onStartDateSelect(param) {
    this.modelObj.StartDate = new Date(param.year, param.month - 1, param.day + 1);
    if (this.modelObj.EndDate.toString() != "0001-01-01T00:00:00Z") {
      this.IsDatesCorrect = this.CompareDatesEqual(this.modelObj.StartDate, this.modelObj.EndDate);
    }
  }
  onEndDateSelect(param) {
    this.modelObj.EndDate = new Date(param.year, param.month - 1, param.day + 1);
    if (this.modelObj.EndDate.toString() != "0001-01-01T00:00:00Z") {
      this.IsDatesCorrect = this.CompareDatesEqual(this.modelObj.StartDate, this.modelObj.EndDate);
    }
  }
  Search(){
    this.loadData();
  }
}
