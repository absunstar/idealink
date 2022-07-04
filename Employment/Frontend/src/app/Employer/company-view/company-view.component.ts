import { Component, OnInit } from '@angular/core';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ServiceCompany } from 'src/app/services/service-company.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { ActivatedRoute } from '@angular/router';
import { cCompanyItem } from 'src/app/interface/Response/Company.class';
import { ServiceJob } from 'src/app/services/job.service';
import { cJobItem } from 'src/app/interface/Response/Job.class';
import { JobType } from 'src/app/Enum/JobType.enum';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-company-view',
  templateUrl: './company-view.component.html',
  styleUrls: ['./company-view.component.css']
})
export class CompanyViewComponent extends baseComponent implements OnInit {

  objProfile: cCompanyItem;
  lstJobs: cJobItem[];
  profileId: string;

  constructor(
    private BLCompany: ServiceCompany,
    private BLJob :ServiceJob,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser,BLTranslate: TranslateService,
    private route: ActivatedRoute) {
    super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
  }
  ngOnInit(): void {
    const param = this.route.snapshot.paramMap.get('Id');
    if (param) {
      this.profileId = param;
      this.BLCompany.getGetByid(param).subscribe({
        next: obj => {debugger;
          this.objProfile = obj;
        },
        error: err => {
          this.message.Error(err);
          this.BLServiceShowMessage.sendMessage(this.message);
        }
      });
      this.BLJob.GetJobsByCompanyId(param).subscribe({
        next: lst => {
          this.lstJobs = lst;
          
        },
        error: err => {
          this.message.Error(err);
          this.BLServiceShowMessage.sendMessage(this.message);
        }
      });
    }
  }
  GetJobType(type) {
    return JobType[type];
  }
}
