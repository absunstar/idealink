import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmationDialogService } from 'src/app/common/confirmation-dialog/confirmation-dialog.service';
import { Page } from 'src/app/common/pagination/page';
import { CustomPaginationService } from 'src/app/common/pagination/services/custom-pagination.service';
import { ShowMessage } from 'src/app/interface/Model/ModelShowMessage.class';
import { cCompanyItem, cCompanyList } from 'src/app/interface/Response/Company.class';
import { ServiceCompany } from 'src/app/services/service-company.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';

@Component({
  selector: 'app-company-approval',
  templateUrl: './company-approval.component.html',
  styleUrls: ['./company-approval.component.css']
})
export class CompanyApprovalComponent implements OnInit {
  pageTitle: string = 'Job Approval';
  lstResult: cCompanyList;
  message: ShowMessage = new ShowMessage();
  filtertxt: string = '';

  page: Page<cCompanyItem> = new Page();

  constructor(private BLService: ServiceCompany,
    private confirmationDialogService: ConfirmationDialogService,
    private paginationService: CustomPaginationService,
    private BLServiceShowMessage: ServiceShowMessage,
    private modalService: NgbModal,
    private router: Router) { }


  ngOnInit() {
    this.loadData();
  }
  private loadData(): void {
    this.BLService.GetCompanyWaitingApproval(this.page.pageable.pageCurrent, this.filtertxt).subscribe({
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
  Approve(Id: string): void {
    this.confirmationDialogService.confirm("Are you sure you want to Approve?")
      .then((confirmed) => {
        if (!confirmed)
          return;
        this.BLService.setApproved(Id).subscribe({
          next: response => {
            this.message.Success("Approved successfully.");
            this.BLServiceShowMessage.sendMessage(this.message);
            this.loadData();
          },
          error: err => this.message.Error(err)
        });
      });
  }
  Reject(Id: string): void {
    this.confirmationDialogService.confirm("Are you sure you want to Reject?")
      .then((confirmed) => {
        if (!confirmed)
          return;
        this.BLService.setRejected(Id).subscribe({
          next: response => {
            this.message.Success("Reject successfully.");
            this.BLServiceShowMessage.sendMessage(this.message);
            this.loadData();
          },
          error: err => this.message.Error(err)
        });
      });
  }
  filterbtn(): void {
    this.page.pageable.pageCurrent = 1;
    this.loadData();
  }
  View(Id: string) {
    this.router.navigate(['/Employer/Company/' + Id])
  }


}
