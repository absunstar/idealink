import { Component, OnInit } from '@angular/core';
import { ServiceDataManagement } from 'src/app/services/datamanagement.service';
import { ITrainingCategory, ITrainingCategoryItem } from 'src/app/interface/Response/TrainingCategory.interface';
import { CustomPaginationService } from 'src/app/common/pagination/services/custom-pagination.service';
import { Page } from 'src/app/common/pagination/page';
import { ShowMessage } from 'src/app/interface/Model/ModelShowMessage.class';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';
import { cfAutoComplete } from 'src/app/interface/forms/AutoComplete.class';
import { NgForm } from '@angular/forms';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { TranslateService } from '@ngx-translate/core';
import { TranslateType } from 'src/app/Enum/TranslateType.enum';

@Component({
  templateUrl: './TrainingCategory-list.component.html',
  styleUrls: ['./TrainingCategory-list.component.scss']
})
export class TrainingCategoryListComponent extends baseComponent implements OnInit {
  pageTitle: string = 'Training Category';
  lstTrainingCategory: ITrainingCategory;
  lstTrainingTypeoptions: cfAutoComplete[];
  message: ShowMessage = new ShowMessage();
  filtertxt: string = '';
  modelName: string = '';
  modelId: string = '';
  modelNameEdited: string = '';
  modelTrainingType: string;
  page: Page<ITrainingCategoryItem> = new Page();
  /////////////////

  /////////////
  constructor(private BLServiceDataManagement: ServiceDataManagement,
    private paginationService: CustomPaginationService,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser, BLTranslate: TranslateService,
    private router: Router,
    private modalService: NgbModal) { super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate); }

  ngOnInit() {
    this.loadData();
    this.BLServiceDataManagement.getTrainingTypeListActive().subscribe({
      next: lst => {

        this.lstTrainingTypeoptions = lst.map(opt => new cfAutoComplete(opt.Id, opt.Name));
      },
      error: err => this.message.Error(err)
    });
  }

  private loadData(): void {
    this.BLServiceDataManagement.getTrainingCategoryAll(this.page.pageable.pageCurrent, this.filtertxt).subscribe({
      next: lst => {
        this.lstTrainingCategory = lst;
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

    this.BLServiceDataManagement.setTrainingCategoryActivate(Id).subscribe({
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

    this.BLServiceDataManagement.setTrainingCategoryDeactivate(Id).subscribe({
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
    this.modelTrainingType = "";
    this.modelName = "";
    this.modelNameEdited = "";
    this.modelTrainingType = "";
    this.modalService.open(content, { backdropClass: 'light-blue-backdrop', centered: true });
  }
  modelSaveBtn(modelForm: NgForm): void {
    if (!modelForm.valid)
      return;

    const x = this.modelTrainingType;
    if (this.modelId == "-1") {
      this.BLServiceDataManagement.createTrainingCategory(this.modelName, this.modelTrainingType).subscribe({
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
      this.BLServiceDataManagement.updateTrainingCategory(this.modelId, this.modelName, this.modelTrainingType).subscribe({
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
  openBackDropCustomClass(content, obj: ITrainingCategoryItem) {
    this.modelId = obj.Id;
    this.modelName = obj.Name;
    this.modelNameEdited = obj.Name;
    this.modelTrainingType = obj.TrainingType.Id;
    this.modalService.open(content, { backdropClass: 'light-blue-backdrop', centered: true });
  }
  Courses(Id: string) {
    this.router.navigate(['/DataManagement/Course/' + Id]);
  }
  Translate() {
    this.router.navigate(['/Translate/' + TranslateType.TrainingCategory + '/0']);
  }
}
