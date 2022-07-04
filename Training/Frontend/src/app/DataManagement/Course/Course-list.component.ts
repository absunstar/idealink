import { Component, OnInit } from '@angular/core';
import { ServiceDataManagement } from 'src/app/services/datamanagement.service';
import { ICourse } from 'src/app/interface/Response/Course.interface';
import { ActivatedRoute, Router } from '@angular/router';
import { ITrainingCategoryItem } from 'src/app/interface/Response/TrainingCategory.interface';
import { ShowMessage } from 'src/app/interface/Model/ModelShowMessage.class';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Constants } from 'src/app/constants';
import { Page } from 'src/app/common/pagination/page';
import { CustomPaginationService } from 'src/app/common/pagination/services/custom-pagination.service';
import { NgForm } from '@angular/forms';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { TranslateService } from '@ngx-translate/core';
import { TranslateType } from 'src/app/Enum/TranslateType.enum';

@Component({
  selector: 'app-Course-list',
  templateUrl: './Course-list.component.html',
  styleUrls: ['./Course-list.component.scss']
})
export class CourseListComponent extends baseComponent implements OnInit {
  objTrainingCategory: ITrainingCategoryItem = new ITrainingCategoryItem();
  pageTitle: string = "Courses";
  message: ShowMessage = new ShowMessage();
  modelName: string = '';
  modelId: string = '';
  modelNameEdited: string = '';
  ModelIsCreate: boolean = true;
  page: Page<ICourse> = new Page();

  filtertxt: string = '';
 
  /////////////

  
  CourseId: string = '';
  filterCourse: ICourse[] = [];
  constructor(private BLServiceDataManagement: ServiceDataManagement,
    private route: ActivatedRoute,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser,BLTranslate: TranslateService,
    private paginationService: CustomPaginationService,
    private router: Router,
    private modalService: NgbModal) {
      super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
  }
  ngOnInit() {
    const param = this.route.snapshot.paramMap.get('id');
    if (param) {
      this.CourseId = param;
      this.page.pageable.pageSize = Constants.PAGE_SIZE;
      this.loadData();
    }
  }
  loadData() {
    this.BLServiceDataManagement.getTrainingCategoryGetByid(this.CourseId).subscribe({
      next: obj => {
        this.objTrainingCategory = obj;
        this.objTrainingCategory.Course = obj.Course.sort((a,b)=> b.Id.localeCompare(a.Id));
        this.filterCourse = this.loadDataFilter();
      },
      error: err => this.message.Error(err)
    });
  }
  public getNextPage(): void {
    this.page.pageable = this.paginationService.getNextPage(this.page);
  }

  public getPreviousPage(): void {
    this.page.pageable = this.paginationService.getPreviousPage(this.page);
  }
  public getloadPageCurrent(): void {
  }
  loadDataFilter(): ICourse[] {
    var lst = this.filtertxt == "" ? this.objTrainingCategory.Course : this.objTrainingCategory.Course.filter((item: ICourse) =>
      item.Name.toLocaleLowerCase().indexOf(this.filtertxt.toLocaleLowerCase()) !== -1);

    this.page.totalElements = lst.length;
    this.page.content = lst;
    this.page.pageable.pageCurrent = 1
    return lst;
  }
  setActivate(Id: string): void {
    if (!confirm(this.msgsetActivate))
      return;

    this.BLServiceDataManagement.setCourseActivate(this.objTrainingCategory.Id, Id).subscribe({
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

    this.BLServiceDataManagement.setCourseDeactivate(this.objTrainingCategory.Id, Id).subscribe({
      next: response => {
        this.message.Success(this.msgDeactivatedSuccessfully);
        this.BLServiceShowMessage.sendMessage(this.message);
        this.loadData();
      },
      error: err => this.message.Error(err)
    });
  }
  filterbtn(): void {
    this.loadDataFilter();
  }
  createBtn(content): void {
    this.modelId = this.objTrainingCategory.Id;
    this.modelName = "";
    this.modelNameEdited = "";
    this.ModelIsCreate = true;
    this.modalService.open(content, { backdropClass: 'light-blue-backdrop', centered: true });
  }
  modelSaveBtn(modelForm: NgForm): void {
    if (!modelForm.valid)
      return;


    if (this.ModelIsCreate) {
      this.BLServiceDataManagement.createCourse(this.objTrainingCategory.Id, this.modelName).subscribe({
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
      this.BLServiceDataManagement.updateCourse(this.objTrainingCategory.Id, this.modelId, this.modelName).subscribe({
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
    this.ModelIsCreate = false;
    this.modalService.open(content, { backdropClass: 'light-blue-backdrop', centered: true });
  }
  onBack(): void {
    this.router.navigate(['/DataManagement/TrainingCategory']);
  }
  Translate(){
    this.router.navigate(['/Translate/' + TranslateType.Courses + '/' + this.CourseId]);
  }
}
