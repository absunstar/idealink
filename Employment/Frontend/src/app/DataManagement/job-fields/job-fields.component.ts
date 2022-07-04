import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgForm } from '@angular/forms';

import { ShowMessage } from 'src/app/interface/Model/ModelShowMessage.class';
import { CustomPaginationService } from 'src/app/common/pagination/services/custom-pagination.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { Page } from 'src/app/common/pagination/page';
import { cGenericIdNameList, cGenericIdNameItem } from 'src/app/interface/Response/GenericIdName.class';
import { ModelIdName } from 'src/app/interface/Model/ModelIdName.class';
import { ServiceJobFields } from 'src/app/services/job-fields.service';
import { Router } from '@angular/router';
import { ConfirmationDialogService } from 'src/app/common/confirmation-dialog/confirmation-dialog.service';
import { TranslateType } from 'src/app/Enum/TranslateType.enum';

@Component({
  selector: 'app-job-fields',
  templateUrl: './job-fields.component.html',
  styleUrls: ['./job-fields.component.css']
})
export class JobFieldsComponent implements OnInit {


  pageTitle: string = 'Job Field';
  lstResult: cGenericIdNameList;
  message: ShowMessage = new ShowMessage();
  filtertxt: string = '';
  modelObj:ModelIdName;
  modelIsCreate: boolean;
  modelNameEdited: string = '';

  page: Page<cGenericIdNameItem> = new Page();

  constructor(private BLService: ServiceJobFields,
    private confirmationDialogService: ConfirmationDialogService,
    private paginationService: CustomPaginationService,
    private BLServiceShowMessage: ServiceShowMessage,
    private modalService: NgbModal,
    private router: Router) { }


  ngOnInit() {
    this.loadData();
  }
  private loadData(): void {
    this.BLService.getAll(this.page.pageable.pageCurrent, this.filtertxt).subscribe({
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
  filterbtn(): void {
    this.page.pageable.pageCurrent = 1;
    this.loadData();
  }
  createBtn(content): void {
    this.modelIsCreate = true;
    this.modelObj = new ModelIdName();
    this.modelNameEdited = "";
    this.modalService.open(content, { backdropClass: 'light-blue-backdrop', centered: true });
  }
  modelSaveBtn(modelForm: NgForm): void {
    this.modelObj.Name = this.modelObj.Name.trim();

    if (!modelForm.valid || this.modelObj.Name == "")
      return;
      
    if (this.modelIsCreate) {
      this.BLService.create(this.modelObj).subscribe({
        next: response => {
          this.message.Success("Saved successfully.");
          this.BLServiceShowMessage.sendMessage(this.message);
          this.loadData();
          this.modalService.dismissAll();
        },
        error: err => this.message.Error(err)
      });
    }
    else {
      this.BLService.update(this.modelObj).subscribe({
        next: response => {
          this.message.Success("Saved successfully.");
          this.BLServiceShowMessage.sendMessage(this.message);
          this.loadData();
          this.modalService.dismissAll();
        },
        error: err => this.message.Error(err)
      });
    }
  }
  openBackDropCustomClass(content, obj:cGenericIdNameItem) {
    this.modelObj = new ModelIdName();
    this.modelObj.Id = obj._id;
    this.modelObj.Name = obj.Name;
    this.modelNameEdited = obj.Name;
    this.modelIsCreate = false;
    this.modalService.open(content, { backdropClass: 'light-blue-backdrop', centered: true });
  }
  GoToSubs(Id:string){
    this.router.navigate(['/DataManagement/JobSubFields/' + Id]);
  }
  Translate(){
    this.router.navigate(['/Translate/' + TranslateType.JobField + '/0']);
  }
}
