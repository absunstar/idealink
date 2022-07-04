import { Component, OnInit } from '@angular/core';
import { cTrainingTypeList, cTrainingTypeItem } from 'src/app/interface/Response/TrainingType.class';
import { ShowMessage } from 'src/app/interface/Model/ModelShowMessage.class';
import { Page } from 'src/app/common/pagination/page';
import { ServiceDataManagement } from 'src/app/services/datamanagement.service';
import { CustomPaginationService } from 'src/app/common/pagination/services/custom-pagination.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgForm } from '@angular/forms';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { TranslateService } from '@ngx-translate/core';
import { TranslateType } from 'src/app/Enum/TranslateType.enum';
import { Router } from '@angular/router';

@Component({
  selector: 'app-training-type',
  templateUrl: './training-type.component.html',
  styleUrls: ['./training-type.component.scss']
})
export class TrainingTypeComponent extends baseComponent implements OnInit {


  pageTitle: string = 'Training Type';
  lstTrainingType: cTrainingTypeList;
  message: ShowMessage = new ShowMessage();
  filtertxt: string = '';
  modelName: string = '';
  modelId: string = '';
  modelNameEdited: string = '';

  page: Page<cTrainingTypeItem> = new Page();

  constructor(private BLServiceDataManagement: ServiceDataManagement,
    private paginationService: CustomPaginationService,
    private router: Router,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser, BLTranslate: TranslateService,
    private modalService: NgbModal) {
    super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
  }


  ngOnInit() {
    this.loadData();
  }
  private loadData(): void {
    this.BLServiceDataManagement.getTrainingTypeAll(this.page.pageable.pageCurrent, this.filtertxt).subscribe({
      next: lst => {
        this.lstTrainingType = lst;
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
    if (!confirm(this.msgsetActivate))
      return;

    this.BLServiceDataManagement.setTrainingTypeActivate(Id).subscribe({
      next: response => {

        this.message.Success(this.msgActivatedSuccessfully);
        this.BLServiceShowMessage.sendMessage(this.message);
        this.loadData();
      },
      error: err => this.message.Error(err)
    });
  }
  setDeactivate(Id: string): void {
    if (!confirm(this.msgsetDeactivate))
      return;
    this.BLServiceDataManagement.setTrainingTypeDeactivate(Id).subscribe({
      next: response => {
        this.message.Success(this.msgDeactivatedSuccessfully);
        this.BLServiceShowMessage.sendMessage(this.message);
        this.loadData();
      },
      error: err => this.message.Error(err)
    });
  }
  filterbtn(): void {
    this.page.pageable.pageCurrent = 1;
    this.loadData();
  }
  createBtn(content): void {
    this.modelId = "-1";
    this.modelName = "";
    this.modelNameEdited = "";
    this.modalService.open(content, { backdropClass: 'light-blue-backdrop', centered: true });
  }
  modelSaveBtn(modelForm: NgForm): void {
    if (!modelForm.valid)
      return;


    if (this.modelId == "-1") {
      this.BLServiceDataManagement.createTrainingType(this.modelName).subscribe({
        next: response => {
          this.message.Success(this.msgSavedSuccessfully);
          this.BLServiceShowMessage.sendMessage(this.message);
          this.loadData();
          this.modalService.dismissAll();
        },
        error: err => this.message.Error(err)
      });
    }
    else {
      this.BLServiceDataManagement.updateTrainingType(this.modelId, this.modelName).subscribe({
        next: response => {
          this.message.Success(this.msgSavedSuccessfully);
          this.BLServiceShowMessage.sendMessage(this.message);
          this.loadData();
          this.modalService.dismissAll();
        },
        error: err => this.message.Error(err)
      });
    }
  }
  openBackDropCustomClass(content, Id: string, name: string) {
    this.modelId = Id;
    this.modelName = name;
    this.modelNameEdited = name;
    this.modalService.open(content, { backdropClass: 'light-blue-backdrop', centered: true });
  }

  Translate() {
    this.router.navigate(['/Translate/' + TranslateType.TrainingType + '/0']);
  }
}
