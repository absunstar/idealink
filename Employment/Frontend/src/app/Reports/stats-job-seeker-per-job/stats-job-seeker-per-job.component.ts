import { Component, OnInit } from '@angular/core';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ModelReportDates } from 'src/app/interface/Model/ModelReportDates.class';
import { ReportApply } from 'src/app/interface/Response/Apply.class';
import { ServiceApply } from 'src/app/services/apply.service';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';

@Component({
  selector: 'app-stats-job-seeker-per-job',
  templateUrl: './stats-job-seeker-per-job.component.html',
  styleUrls: ['./stats-job-seeker-per-job.component.css']
})
export class StatsJobSeekerPerJobComponent extends baseComponent implements OnInit {

  modelStartDate: NgbDateStruct;
  modelEndDate: NgbDateStruct;
  modelObj: ModelReportDates = new ModelReportDates();
  IsDatesCorrect: boolean = true;
  lstResult : ReportApply[] = [];

  constructor(BLServiceShowMessage: ServiceShowMessage,
    BLJobSeekerLoginUser: ServiceLoginUser, BLTranslate: TranslateService,
    private BLServiceApply: ServiceApply) {
    super(BLServiceShowMessage, BLJobSeekerLoginUser, BLTranslate);
  }

  ngOnInit(): void {
    this.loadData();
  }
  private loadData(): void {

    this.BLServiceApply.ReportJobSeekerAppliedPerJobCount(this.modelObj).subscribe({
      next: obj => {
        this.lstResult = obj;
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
  Search() {
    this.loadData();
  }
}
