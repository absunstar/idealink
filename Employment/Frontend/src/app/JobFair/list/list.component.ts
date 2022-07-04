import { Component, OnInit } from '@angular/core';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { UserType } from 'src/app/Enum/UserType.enum';
import { cJobFairList, cJobFairItem } from 'src/app/interface/Response/JobFair.class';
import { Page } from 'src/app/common/pagination/page';
import { ServiceJobFair } from 'src/app/services/job-fair.service';
import { CustomPaginationService } from 'src/app/common/pagination/services/custom-pagination.service';
import { ConfirmationDialogService } from 'src/app/common/confirmation-dialog/confirmation-dialog.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { ModelJobFairAttendance } from 'src/app/interface/Model/ModelJobFairAttendance.class';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent extends baseComponent implements OnInit {

  get EnumUserTypes() { return UserType };
  pageTitle: string = 'Job Fair ';
  lstJobFair: cJobFairList;
  filtertxt: string = '';
  modelNameEdited: string = '';
  modelObj: cJobFairItem;
  modelIsCreate: boolean = true;
  page: Page<cJobFairItem> = new Page();
  isClicked = false;
  modelAttendance: ModelJobFairAttendance = new ModelJobFairAttendance();
  fileurl:string;

  constructor(private BLServiceJobFair: ServiceJobFair,
    private paginationService: CustomPaginationService,
    private confirmationDialogService: ConfirmationDialogService,
    private modalService: NgbModal,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser,BLTranslate: TranslateService,) {
    super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
  }

  openBackDropCustomClass(content, idx: string, Name:string) {
    this.modelAttendance = new ModelJobFairAttendance();
    this.modelAttendance.JobFairId = idx;
    this.modelNameEdited = Name;
    this.modalService.open(content, { backdropClass: 'light-blue-backdrop', centered: true });
  }
  modelSaveBtn(obj){
    this.BLServiceJobFair.SetAttendance(this.modelAttendance).subscribe({
      next: flag => {
        if(flag)
        {
          this.message.Success("Save successfully.");
        }
        else
          {
          this.message.Error("Invalid Attendance Code.");
        }
        
        this.BLServiceShowMessage.sendMessage(this.message);
      },
      error: err => this.message.Error(err)
    });
  }
  ngOnInit() {
    this.loadData();
  }
  private loadData(): void {
    if (this.IsAdmin) {
      this.BLServiceJobFair.getAll(this.page.pageable.pageCurrent, this.filtertxt).subscribe({
        next: lst => {

          this.lstJobFair = lst;
          this.page.pageable.pageSize = lst.pageSize;
          this.page.totalElements = lst.totalCount;
          this.page.content = lst.lstResult;
        },
        error: err => this.message.Error(err)
      });
    }
    else if (this.isJobSeeker) {
      this.BLServiceJobFair.Search(this.page.pageable.pageCurrent, this.filtertxt).subscribe({
        next: lst => {

          this.lstJobFair = lst;
          this.page.pageable.pageSize = lst.pageSize;
          this.page.totalElements = lst.totalCount;
          this.page.content = lst.lstResult;
        },
        error: err => this.message.Error(err)
      });
    }
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
        this.BLServiceJobFair.setActivate(Id).subscribe({
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
        this.BLServiceJobFair.setDeActivate(Id).subscribe({
          next: response => {
            this.message.Success("Deactivated successfully.");
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
  Export(idx:string){
    this.BLServiceJobFair.Export(idx).subscribe({
      next: response => {
        window.open(response, "_blank");
        this.message.Success("Export Successfully.");
        this.BLServiceShowMessage.sendMessage(this.message);
      },
      error: err => this.message.Error(err)
    });
  }
}
