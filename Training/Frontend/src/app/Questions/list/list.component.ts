import { Component, OnInit } from '@angular/core';
import { ShowMessage } from 'src/app/interface/Model/ModelShowMessage.class';
import { cQuestionList, cQuestionItem } from 'src/app/interface/Model/ModelQuestions.class';
import { Page } from 'src/app/common/pagination/page';
import { ServiceQuestion } from 'src/app/services/Question.service';
import { CustomPaginationService } from 'src/app/common/pagination/services/custom-pagination.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { cTrainingTypeItem } from 'src/app/interface/Response/TrainingType.class';
import { ITrainingCategoryItem } from 'src/app/interface/Response/TrainingCategory.interface';
import { ServiceDataManagement } from 'src/app/services/datamanagement.service';
import { ModelFilterQuestions } from 'src/app/interface/Model/ModelFilterQuestions.class';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent extends baseComponent implements OnInit {

  pageTitle: string = 'Questions';
  lstQuestion: cQuestionList;
  message: ShowMessage = new ShowMessage();
  filtertxt: string = '';
  modelName: string = '';
  modelId: string = '';
  modelNameEdited: string = '';
  lstTrainingTypes: cTrainingTypeItem[];
  lstTrainingCategory: ITrainingCategoryItem[];
  lstTrainingCategoryFilter: ITrainingCategoryItem[];
  filterObj: ModelFilterQuestions = new ModelFilterQuestions();

  page: Page<cQuestionItem> = new Page();

  constructor(private BLServiceQuestion: ServiceQuestion,
    private paginationService: CustomPaginationService,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser,BLTranslate: TranslateService,
    private BLServiceDataManagement: ServiceDataManagement,
    private router: Router,
    private modalService: NgbModal) { super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);}


  ngOnInit() {
    this.loadData();
  }
  private loadData(): void {
    this.filterObj.CurrentPage = this.page.pageable.pageCurrent;
    this.BLServiceQuestion.getQuestionAll(this.filterObj).subscribe({
      next: lst => {
        this.lstQuestion = lst;
        this.page.pageable.pageSize = lst.pageSize;
        this.page.totalElements = lst.totalCount;
        this.page.content = lst.lstResult;
      },
      error: err => this.message.Error(err)
    });
    this.BLServiceDataManagement.getTrainingTypeListActive().subscribe({
      next: lst => {
        this.lstTrainingTypes = lst;
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

    this.BLServiceQuestion.setQuestionActivate(Id).subscribe({
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
    this.BLServiceQuestion.setQuestionDeactivate(Id).subscribe({
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
  Edit( Id: string) {
      this.router.navigate(['/Questions/Create/' + Id]);
  }
  onTrainingTypeFilterSelect() {
    if (this.filterObj.TrainingTypeId == "")
      return;

    this.BLServiceDataManagement.getTrainingCategoryListByTrainingType(this.filterObj.TrainingTypeId).subscribe({
      next: lst => {
        this.lstTrainingCategoryFilter = lst;
      },
      error: err => this.message.Error(err)
    });
  }
}
