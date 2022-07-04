import { Component, OnInit } from '@angular/core';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { cApplyItem, cApplyList } from 'src/app/interface/Response/Apply.class';
import { ServiceApply } from 'src/app/services/apply.service';
import { Router } from '@angular/router';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { CustomPaginationService } from 'src/app/common/pagination/services/custom-pagination.service';
import { Page } from 'src/app/common/pagination/page';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-applied-jobs',
  templateUrl: './applied-jobs.component.html',
  styleUrls: ['./applied-jobs.component.css']
})
export class AppliedJobsComponent extends baseComponent implements OnInit {

  lstResult : cApplyList;
  show = false;
  filtertxt: string = '';
  page: Page<cApplyItem> = new Page();
  
  constructor(private BLService: ServiceApply,
    private router: Router,
    private paginationService: CustomPaginationService,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser,BLTranslate: TranslateService,) {
    super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
  }

  ngOnInit(): void {
    this.loadData();
  }
  onView(Id:string){
    this.router.navigate(['Employer/Job/' + Id]);
  }

  private loadData(): void {
    this.BLService.ListAll(this.page.pageable.pageCurrent, this.filtertxt).subscribe({
      next: lst => {
        this.lstResult = lst;
        this.page.pageable.pageSize = lst.pageSize;
        this.page.totalElements = lst.totalCount;
        this.page.content = lst.lstResult;
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

