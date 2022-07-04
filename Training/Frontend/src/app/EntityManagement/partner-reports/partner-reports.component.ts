import { Component, OnInit } from '@angular/core';
import { cUserProfileItem } from 'src/app/interface/Response/UserProfile.class';
import { Page } from 'src/app/common/pagination/page';
import { ServiceEntityManagement } from 'src/app/services/entitymanagement.service';
import { CustomPaginationService } from 'src/app/common/pagination/services/custom-pagination.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { cEntityPartnerReportsList, cEntityPartnerReportsItem } from 'src/app/interface/Response/PartnerReports.class';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-partner-reports',
  templateUrl: './partner-reports.component.html',
  styleUrls: ['./partner-reports.component.css']
})
export class PartnerReportsComponent extends baseComponent implements OnInit {

  pageTitle: string = 'Partner Reports Account';
  lstEntityPartner: cEntityPartnerReportsList;
  PartnerMembers: cUserProfileItem[];
  filtertxt: string = '';
  modelNameEdited: string = '';
  modelObj: cEntityPartnerReportsItem;
  modelIsCreate:boolean = true;
  page: Page<cEntityPartnerReportsItem> = new Page();
  MemberId: string;

  constructor(private BLServiceEntityManagement: ServiceEntityManagement,
    private paginationService: CustomPaginationService,
    BLServiceShowMessage: ServiceShowMessage,BLTranslate: TranslateService,
    BLServiceLoginUser : ServiceLoginUser) { 
      super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
    }


  ngOnInit() {
    this.loadData();
  }
  private loadData(): void {
    this.BLServiceEntityManagement.EntityPartnerReportListAll(this.page.pageable.pageCurrent, this.filtertxt).subscribe({
      next: lst => {
        this.lstEntityPartner = lst;
        this.page.pageable.pageSize = lst.pageSize;
        this.page.totalElements = lst.totalCount;
        this.page.content = lst.lstResult;
      },
      error: err => this.message.Error(err)
    });
  }
  public getNextPage(): void {
    this.page.pageable = this.paginationService.getNextPage(this.page);
    this.loadData();
  }

  public getPreviousPage(): void {
    this.page.pageable = this.paginationService.getPreviousPage(this.page);
    this.loadData();
  }
  public getloadPageCurrent(): void {
    this.loadData();
  }
  
  filterbtn(): void {
    this.page.pageable.pageCurrent = 1;
    this.loadData();
  }
  

}
