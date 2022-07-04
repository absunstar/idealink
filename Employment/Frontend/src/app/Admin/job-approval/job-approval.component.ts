import { Component, OnInit } from '@angular/core';
import { cJobList, cJobItem } from 'src/app/interface/Response/Job.class';
import { ShowMessage } from 'src/app/interface/Model/ModelShowMessage.class';
import { Page } from 'src/app/common/pagination/page';
import { ServiceJob } from 'src/app/services/job.service';
import { CustomPaginationService } from 'src/app/common/pagination/services/custom-pagination.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';
import { ModelIdName } from 'src/app/interface/Model/ModelIdName.class';
import { NgForm } from '@angular/forms';
import { cGenericIdNameItem } from 'src/app/interface/Response/GenericIdName.class';
import { ConfirmationDialogService } from 'src/app/common/confirmation-dialog/confirmation-dialog.service';

@Component({
  selector: 'app-job-approval',
  templateUrl: './job-approval.component.html',
  styleUrls: ['./job-approval.component.css']
})
export class JobApprovalComponent implements OnInit {

  pageTitle: string = 'Job Approval';
  lstResult: cJobList;
  message: ShowMessage = new ShowMessage();
  filtertxt: string = '';

  page: Page<cJobItem> = new Page();

  constructor(private BLService: ServiceJob,
    private confirmationDialogService: ConfirmationDialogService,
    private paginationService: CustomPaginationService,
    private BLServiceShowMessage: ServiceShowMessage,
    private modalService: NgbModal,
    private router: Router) { }


  ngOnInit() {
    this.loadData();
  }
  private loadData(): void {
    this.BLService.GetJobWaitingApproval(this.page.pageable.pageCurrent, this.filtertxt).subscribe({
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
    this.router.navigate(['/Employer/Job/' + Id])
  }


}
