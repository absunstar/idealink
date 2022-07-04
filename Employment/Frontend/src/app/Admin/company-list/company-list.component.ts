import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Page } from 'src/app/common/pagination/page';
import { CustomPaginationService } from 'src/app/common/pagination/services/custom-pagination.service';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ShowMessage } from 'src/app/interface/Model/ModelShowMessage.class';
import { cCompanyList, cCompanyItem } from 'src/app/interface/Response/Company.class';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { ServiceCompany } from 'src/app/services/service-company.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { ServiceUserProfile } from 'src/app/services/userprofile.service';

@Component({
  selector: 'app-company-list',
  templateUrl: './company-list.component.html',
  styleUrls: ['./company-list.component.css']
})
export class CompanyListComponent extends baseComponent implements OnInit {

  pageTitle: string = 'My Companies';
  lstResult: cCompanyList;
  message: ShowMessage = new ShowMessage();
  filtertxt: string = '';
  page: Page<cCompanyItem> = new Page();

  constructor(private router: Router,
    private BLService: ServiceCompany,
    private paginationService: CustomPaginationService,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser,BLTranslate: TranslateService,) {
    super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
  }

  ngOnInit(): void {
    this.loadData();
  }
  
  private loadData(): void {
    this.BLService.ListAnyCompanyPaged(this.page.pageable.pageCurrent, this.filtertxt).subscribe({
      next: lst => {debugger;
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

  filterbtn() {
    this.loadData();
  }
  onView(Id:string){
    this.router.navigate(['Employer/Company/' + Id]);
  }
}
