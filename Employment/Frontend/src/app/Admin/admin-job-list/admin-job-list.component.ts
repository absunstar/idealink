import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ConfirmationDialogService } from 'src/app/common/confirmation-dialog/confirmation-dialog.service';
import { Page } from 'src/app/common/pagination/page';
import { CustomPaginationService } from 'src/app/common/pagination/services/custom-pagination.service';
import { JobStatus } from 'src/app/Enum/JobStatus.enum';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ModelAdminJobSearch } from 'src/app/interface/Model/ModelJobFilter.class';
import { ShowMessage } from 'src/app/interface/Model/ModelShowMessage.class';
import { CompanyEmployers } from 'src/app/interface/Response/CompanyEmployer.class';
import { cJobList, cJobItem } from 'src/app/interface/Response/Job.class';
import { JobStats } from 'src/app/interface/Response/JobStats.class';
import { ServiceJob } from 'src/app/services/job.service';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { ServiceCompany } from 'src/app/services/service-company.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';

@Component({
  selector: 'app-admin-job-list',
  templateUrl: './admin-job-list.component.html',
  styleUrls: ['./admin-job-list.component.css']
})
export class AdminJobListComponent extends baseComponent implements OnInit {

  pageTitle: string = 'My Companies';
  lstResult: cJobList;
  message: ShowMessage = new ShowMessage();
  //filtertxt: string = '';
  page: Page<cJobItem> = new Page();
  jobStats: JobStats;
  modelFilter: ModelAdminJobSearch = new ModelAdminJobSearch();
  lstCompanies: CompanyEmployers;

  constructor(private router: Router,
    private BLService: ServiceJob,
    private BLServiceCompany: ServiceCompany,
    private paginationService: CustomPaginationService,
    private confirmationDialogService: ConfirmationDialogService,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser,BLTranslate: TranslateService,) {
    super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
  }

  ngOnInit(): void {
    this.loadData();
    
    this.BLServiceCompany.ListCompany().subscribe({
      next: lst => {
        this.lstCompanies = lst;
       
      },
      error: err => this.message.Error(err)
    });
  }

  private loadData(): void {
    this.modelFilter.CurrentPage = this.page.pageable.pageCurrent;
    this.BLService.AdminJobSearch(this.modelFilter).subscribe({
      next: lst => {
        this.lstResult = lst;
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
  // setActivate(Id: string): void {
  //   this.confirmationDialogService.confirmActivation()
  //     .then((confirmed) => {
  //       if (!confirmed)
  //         return;
  //       this.BLService.setActivate(Id).subscribe({
  //         next: response => {
  //           this.message.Success("Activated successfully.");
  //           this.BLServiceShowMessage.sendMessage(this.message);
  //           this.loadData();
  //         },
  //         error: err => this.message.Error(err)
  //       });
  //     });
  // }
  // setDeActivate(Id: string): void {
  //   this.confirmationDialogService.confirmDeactivation()
  //     .then((confirmed) => {
  //       if (!confirmed)
  //         return;
  //       this.BLService.setDeActivate(Id).subscribe({
  //         next: response => {
  //           this.message.Success("Deactivated successfully.");
  //           this.BLServiceShowMessage.sendMessage(this.message);
  //           this.loadData();
  //         },
  //         error: err => this.message.Error(err)
  //       });
  //     });
  // }
  filterbtn() {
    this.loadData();
  }
  // createBtn() {
  //   this.router.navigate(['Employer/PostJob']);
  // }
  // onEdit(Id: string) {
  //   this.router.navigate(['Employer/PostJob/' + Id]);
  // }
  onView(Id: string) {
    this.router.navigate(['Employer/Job/' + Id]);
  }
  GetStatus(status: number) {
    return JobStatus[status];
  }
}
