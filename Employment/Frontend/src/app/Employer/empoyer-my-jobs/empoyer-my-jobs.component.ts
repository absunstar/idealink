import { Component, OnInit } from '@angular/core';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { cJobList, cJobItem } from 'src/app/interface/Response/Job.class';
import { ShowMessage } from 'src/app/interface/Model/ModelShowMessage.class';
import { Page } from 'src/app/common/pagination/page';
import { Router } from '@angular/router';
import { ServiceJob } from 'src/app/services/job.service';
import { CustomPaginationService } from 'src/app/common/pagination/services/custom-pagination.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { JobStatus } from 'src/app/Enum/JobStatus.enum';
import { ConfirmationDialogService } from 'src/app/common/confirmation-dialog/confirmation-dialog.service';
import { JobStats } from 'src/app/interface/Response/JobStats.class';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-empoyer-my-jobs',
  templateUrl: './empoyer-my-jobs.component.html',
  styleUrls: ['./empoyer-my-jobs.component.css']
})
export class EmpoyerMyJobsComponent extends baseComponent implements OnInit {

  pageTitle: string = 'My Companies';
  lstResult: cJobList;
  message: ShowMessage = new ShowMessage();
  filtertxt: string = '';
  page: Page<cJobItem> = new Page();
  jobStats: JobStats;
  searchData:any=[];
  searchText: string;

  constructor(private router: Router,
    private BLService: ServiceJob,
    private paginationService: CustomPaginationService,
    private confirmationDialogService: ConfirmationDialogService,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser,BLTranslate: TranslateService,) {
    super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
  }

  ngOnInit(): void {
    this.loadData();
    let req=Object({filterText:this.filtertxt})
    this.BLService.Search(req).subscribe({
      next: response => {
        this.searchData= response.lstResult
      }  
    })
  }

  private loadData(): void {
    this.BLService.ListAllByEmployerId(this.page.pageable.pageCurrent, this.filtertxt).subscribe({
      next: lst => {
        this.lstResult = lst;
        this.page.pageable.pageSize = lst.pageSize;
        this.page.totalElements = lst.totalCount;
        this.page.content = lst.lstResult;
      },
      error: err => this.message.Error(err)
    });
    this.BLService.GetMyJobStats().subscribe({
      next: lst => {
        this.jobStats = lst;
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
  setActivate(Id: string): void {
    this.confirmationDialogService.confirmActivation()
      .then((confirmed) => {
        if (!confirmed)
          return;
        this.BLService.setActivate(Id).subscribe({
          next: response => {
            this.message.Success("Activated successfully.");
            this.BLServiceShowMessage.sendMessage(this.message);
            this.loadData();
          },
          error: err => this.message.Error(err)
        });
      });
  }
  setDeActivate(Id: string): void {
    this.confirmationDialogService.confirmDeactivation()
      .then((confirmed) => {
        if (!confirmed)
          return;
        this.BLService.setDeActivate(Id).subscribe({
          next: response => {
            this.message.Success("Deactivated successfully.");
            this.BLServiceShowMessage.sendMessage(this.message);
            this.loadData();
          },
          error: err => this.message.Error(err)
        });
      });
  }
  filterbtn() {
    // this.loadData();
    // let req=Object({filterText:this.filtertxt})
    // this.BLService.Search(req).subscribe({
    //   next: response => {
    //     this.searchData= response.lstResult
    //   }  
    // })
  }
  createBtn() {
    this.router.navigate(['Employer/PostJob']);
  }
  onEdit(Id: string) {
    this.router.navigate(['Employer/PostJob/' + Id]);
  }
  onView(Id: string) {
    this.router.navigate(['Employer/Job/' + Id]);
  }
  GetStatus(status: number) {
    return JobStatus[status].replace("_"," ");
  }

  tableSearch(event) {
    this.searchText = event.target.value
  }
}
