import { Component, OnInit } from '@angular/core';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { cApplyList, cApplyItem } from 'src/app/interface/Response/Apply.class';
import { Page } from 'src/app/common/pagination/page';
import { ServiceApply } from 'src/app/services/apply.service';
import { Router, ActivatedRoute } from '@angular/router';
import { CustomPaginationService } from 'src/app/common/pagination/services/custom-pagination.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-applied-job-seekers',
  templateUrl: './applied-job-seekers.component.html',
  styleUrls: ['./applied-job-seekers.component.css']
})
export class AppliedJobSeekersComponent extends baseComponent implements OnInit {

  lstResult: cApplyList;
  show = false;
  filtertxt: string = '';
  page: Page<cApplyItem> = new Page();
  JobName: string = "";
  JobId: string;

  constructor(private BLService: ServiceApply,
    private router: Router,
    private route: ActivatedRoute,
    private paginationService: CustomPaginationService,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser,BLTranslate: TranslateService,) {
    super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
  }

  ngOnInit(): void {
    const param = this.route.snapshot.paramMap.get("Id");
    if (param)
    {
      this.JobId = param;
    }
    this.loadData();
  }
  onView(Id: string) {
    this.router.navigate(['JobSeeker/Resume/' + Id]);
  }
  onHired(Id:string){
    this.BLService.Hire(Id, this.JobId).subscribe({
      next: lst => {
        this.message.Success("Saved successfully.");
        this.BLServiceShowMessage.sendMessage(this.message);
        this.loadData();
      },
      error: err => this.message.Error(err)
    });
  }
  onUnHired(Id:string){
    this.BLService.UnHire(Id, this.JobId).subscribe({
      next: lst => {
        this.message.Success("Saved successfully.");
        this.BLServiceShowMessage.sendMessage(this.message);
        this.loadData();
      },
      error: err => this.message.Error(err)
    });
  }
  private loadData(): void {
    this.BLService.ListAll(this.page.pageable.pageCurrent, this.filtertxt, this.JobId).subscribe({
      next: lst => {
        this.lstResult = lst;
        this.page.pageable.pageSize = lst.pageSize;
        this.page.totalElements = lst.totalCount;
        this.page.content = lst.lstResult;

        if (lst.totalCount > 0)
          this.JobName = lst.lstResult[0].Job.Name;
      },
      error: err => this.message.Error(err)
    });
  }
  filterbtn(): void {
    this.page.pageable.pageCurrent = 1;
    this.loadData();
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
}

