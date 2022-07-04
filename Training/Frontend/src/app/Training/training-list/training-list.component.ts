import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { cTrainingList, cTrainingItem } from 'src/app/interface/Response/Training.class';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ServiceTraining } from 'src/app/services/training.service';
import { CustomPaginationService } from 'src/app/common/pagination/services/custom-pagination.service';
import { Page } from 'src/app/common/pagination/page';
import { ModelTraining } from 'src/app/interface/Model/ModelTraining.class';
import { cEntityPartnerItem } from 'src/app/interface/Response/EntityPartner.class';
import { cEntitySubPartnerItem } from 'src/app/interface/Response/EntitySubPartner.class';
import { cUserProfileItem } from 'src/app/interface/Response/UserProfile.class';
import { ServiceUserProfile } from 'src/app/services/userprofile.service';
import { ServiceEntityManagement } from 'src/app/services/entitymanagement.service';
import { cTrainingTypeItem } from 'src/app/interface/Response/TrainingType.class';
import { ITrainingCategoryItem } from 'src/app/interface/Response/TrainingCategory.interface';
import { ServiceDataManagement } from 'src/app/services/datamanagement.service';
import { cCityItem } from 'src/app/interface/Response/City.class';
import { cArea } from 'src/app/interface/Response/Area.class';
import { ModelFilterTraining } from 'src/app/interface/Model/ModelFilterTraining.class';
import { Router } from '@angular/router';
import { ServiceTrainee } from 'src/app/services/trainee.service';
import { cEntityTrainingCenterItem } from 'src/app/interface/Response/EntityTrainingCenter.class';
import { cExamTemplateItem } from 'src/app/interface/Response/ExamTemplate.class';
import { ServiceExamTemplate } from 'src/app/services/exam-template.service';
import { ModelEntitySubEntityIds } from 'src/app/interface/Model/ModelEntitySubEntityIds.class';
import { TranslateService } from '@ngx-translate/core';
import { Constants } from 'src/app/constants';
import { ICourse } from 'src/app/interface/Response/Course.interface';

@Component({
  selector: 'app-training-list',
  templateUrl: './training-list.component.html',
  styleUrls: ['./training-list.component.css']
})
export class TrainingListComponent extends baseComponent implements OnInit {

  filterObj: ModelFilterTraining = new ModelFilterTraining();
  pageTitle: string = 'Training List';
  lstTraining: cTrainingList;
  filtertxt: string = '';
  modelNameEdited: string = '';
  modelObj: ModelTraining;
  modelIsCreate: boolean = true;
  modelIsStarted: boolean = false;
  page: Page<cTrainingItem> = new Page();
  MemberId: string;
  lstPartners: cEntityPartnerItem[];
  lstSubPartners: cEntitySubPartnerItem[];
  lstTrainingCenters: cEntityTrainingCenterItem[];
  lstSubPartnersFilter: cEntitySubPartnerItem[];
  lstTrainers: cUserProfileItem[];
  lstTrainersFilter: cUserProfileItem[];
  lstTrainingTypes: cTrainingTypeItem[];
  lstTrainingCategory: ITrainingCategoryItem[];
  lstTrainingCategoryFilter: ITrainingCategoryItem[];
  lstCity: cCityItem[];
  lstAreas: cArea[];
  todayDate = new Date();
  IsDatesCorrect: boolean = true;
  lstExamTemplate: cExamTemplateItem[];
  modelExamTemplate: ModelEntitySubEntityIds;
  lstCourses: ICourse[];

  msgTrainingRegister: string;

  constructor(private BLServiceTraining: ServiceTraining,
    private BLServiceTrainee: ServiceTrainee,
    private BLServiceUserProfile: ServiceUserProfile,
    private BLServiceEntityManagement: ServiceEntityManagement,
    private BLServiceDataManagement: ServiceDataManagement,
    private BLServiceExamTemplate: ServiceExamTemplate,
    private paginationService: CustomPaginationService,
    private modalService: NgbModal,
    private router: Router,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser, BLTranslate: TranslateService,) {
    super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
  }


  ngOnInit() {

    this.BLTranslate.get("msgTrainingRegister").subscribe(res => { this.msgTrainingRegister = res; });


    this.loadData();
    if (!this.IsTrainee) {
      this.BLServiceEntityManagement.getEntityPartnerGetMy().subscribe({
        next: lst => {
          this.lstPartners = lst;
        },
        error: err => this.message.Error(err)
      });
      this.BLServiceExamTemplate.ExamTemplateListActive().subscribe({
        next: lst => {
          this.lstExamTemplate = lst;
        },
        error: err => this.message.Error(err)
      });
    }
    this.BLServiceDataManagement.getTrainingTypeListActive().subscribe({
      next: lst => {
        this.lstTrainingTypes = lst;
      },
      error: err => this.message.Error(err)
    });
    this.BLServiceDataManagement.getCityListActive().subscribe({
      next: lst => {
        this.lstCity = lst;
      },
      error: err => this.message.Error(err)
    });

  }

  onPartnerSelect() {
    if (this.modelObj.PartnerId == "")
      return;

    if (!this.IsTrainee) {
      this.BLServiceEntityManagement.getEntitySubPartnerGetMyByPartnerId(this.modelObj.PartnerId).subscribe({
        next: lst => {
          this.lstSubPartners = lst;
        },
        error: err => this.message.Error(err)
      });
      if (!this.IsSubPartner) {
        var tc = this.lstPartners.find(item => item.Id == this.modelObj.PartnerId).TrainingCenters;
        this.lstTrainingCenters = tc.filter(a => a.IsActive == true).sort((a, b) => a.Name.localeCompare(b.Name));
      }
    }
    this.lstTrainers = [];
  }
  onPartnerFilterSelect() {
    if (this.filterObj.PartnerId == "")
      return;

    if (!this.IsTrainee) {
      this.BLServiceEntityManagement.getEntitySubPartnerGetMyByPartnerId(this.filterObj.PartnerId).subscribe({
        next: lst => {
          this.lstSubPartnersFilter = lst;
        },
        error: err => this.message.Error(err)
      });
    }
    this.lstTrainers = [];
  }
  onSubPartnerSelect() {
    if (this.modelObj.SubPartnerId == "")
      return;
    if (!this.IsTrainee) {
      this.BLServiceUserProfile.GetMyTrainersBySubPartnerId(this.modelObj.SubPartnerId).subscribe({
        next: lst => {
          this.lstTrainers = lst;
        },
        error: err => this.message.Error(err)
      });
    }
    if (this.IsSubPartner) {

      var sub = this.lstSubPartners.find(item => item.Id == this.modelObj.SubPartnerId);
      this.lstTrainingCenters = sub.TrainingCenters;
    }
  }
  onSubPartnerFilterSelect() {
    if (this.filterObj.SubPartnerId == "")
      return;
    if (!this.IsTrainee) {
      this.BLServiceUserProfile.GetMyTrainersBySubPartnerId(this.filterObj.SubPartnerId).subscribe({
        next: lst => {
          this.lstTrainersFilter = lst;
        },
        error: err => this.message.Error(err)
      });
    }
  }
  onTrainingTypeSelect() {
    if (this.modelObj.TrainingTypeId == "")
      return;

    this.BLServiceDataManagement.getTrainingCategoryListByTrainingType(this.modelObj.TrainingTypeId).subscribe({
      next: lst => {
        this.lstTrainingCategory = lst.filter(a => a.IsActive == true).sort((a, b) => a.Name.localeCompare(b.Name));;
      },
      error: err => this.message.Error(err)
    });
  }
  onTrainingTypeFilterSelect() {
    if (this.filterObj.TrainingTypeId == "")
      return;

    this.BLServiceDataManagement.getTrainingCategoryListByTrainingType(this.filterObj.TrainingTypeId).subscribe({
      next: lst => {
        this.lstTrainingCategoryFilter = lst.filter(a => a.IsActive == true).sort((a, b) => a.Name.localeCompare(b.Name));;
      },
      error: err => this.message.Error(err)
    });
  }
  getCourses() {
    var lst = this.lstTrainingCategory?.find(x => x.Id == this.modelObj.TrainingCategoryId)?.Course;
    if (lst) {
      var x = lst?.filter(y => y.IsActive == true).sort((a, b) => a.Name.localeCompare(b.Name));
      return x;
    }
    return lst;
  }
  OnCitySelect() {

    this.lstAreas = [];

    var area = this.lstCity?.find(x => x.Id == this.modelObj.CityId).Areas;
    if (area.length == 0)
      return;
    this.lstAreas = area.filter(y => y.IsActive == true).sort((x, y) => x.Name.localeCompare(y.Name));

  }
  onTrainingAdminApproved(Id: string) {
    this.BLServiceTraining.SetAdminApproved(Id).subscribe({
      next: obj => {
        this.loadData();
      }
    });
  }
  onTrainingConfirm1(Id: string) {
    this.BLServiceTraining.SetConfirmed1(Id).subscribe({
      next: obj => {
        this.loadData();
      }
    });
  }
  onTrainingConfirm2(Id: string) {
    this.BLServiceTraining.SetConfirmed2(Id).subscribe({
      next: obj => {
        this.loadData();
      }
    });
  }
  private loadData(): void {

    this.filterObj.CurrentPage = this.page.pageable.pageCurrent;
    this.BLServiceTraining.searchTraining(this.filterObj).subscribe({
      next: lst => {
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
  setDeactivate(Id: string): void {
    if (!confirm(this.msgsetDeleted))
      return;

    this.BLServiceTraining.setDeactivate(Id).subscribe({
      next: response => {
        this.message.Success(this.msgDeletedSuccessfully);
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
    // this.modelId = "-1";
    // this.modelName = "";
    this.modelIsStarted = false;
    this.lstSubPartners = [];
    this.lstTrainers = [];
    this.lstAreas = [];
    this.modelObj = new ModelTraining();
    this.modelIsCreate = true;
    this.modelNameEdited = "";
    this.modalService.open(content, { size: 'lg', backdropClass: 'light-blue-backdrop', centered: true });
  }
  ExportBtn() {
    this.BLServiceTraining.ExportTraining(this.filterObj).subscribe({
      next: data => {
        
        this.downloadFile(data);
      },
      error: err => this.message.Error(err)
    });
  }
  modelSaveBtn(modelForm: NgForm): void {
    if (!modelForm.valid)
      return;

    if (this.modelIsCreate) {
      this.BLServiceTraining.create(this.modelObj).subscribe({
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
      this.BLServiceTraining.update(this.modelObj).subscribe({
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
  openExamTemplate(content, obj: cTrainingItem) {
    this.modelExamTemplate = new ModelEntitySubEntityIds();
    this.modelExamTemplate.MainEntityId = obj.Id;
    this.modelExamTemplate.SubEntityId = obj.ExamTemplateId;
    this.modalService.open(content, { size: 'lg', backdropClass: 'light-blue-backdrop', centered: true });
  }
  SaveExamTemplate(modelForm: NgForm) {
    if (!modelForm.valid)
      return;

    if (this.modelIsCreate) {
      this.BLServiceTraining.SaveExamTemplate(this.modelExamTemplate).subscribe({
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
  openBackDropCustomClass(content, obj: cTrainingItem) {

    var model = new ModelTraining;
    model.AreaId = obj.AreaId;
    model.CityId = obj.CityId;
    model.EndDate = obj.EndDate;
    model.Id = obj.Id;
    model.PartnerId = obj.PartnerId.Id;
    model.SubPartnerId = obj.SubPartnerId.Id;
    model.StartDate = obj.StartDate;
    model.TrainingCategoryId = obj.TrainingCategoryId.Id;
    model.TrainingTypeId = obj.TrainingTypeId.Id;
    model.days = obj.days;
    model.TrainerId = obj.TrainerId;
    model.TrainingCenterId = obj.TrainingCenterId.Id;
    model.Type = obj.Type.toString();
    model.IsOnline = obj.IsOnline;

    this.modelIsStarted = false;
    var start = new Date(model.StartDate );
    var end = new Date(model.EndDate);
    if (start <= this.todayDate && end>= this.todayDate)
      this.modelIsStarted = true;

    this.modelObj = model;
    this.onPartnerSelect();
    this.onSubPartnerSelect();
    this.onTrainingTypeSelect();
    this.OnCitySelect();

    this.modelIsCreate = false;
    //this.modelNameEdited = obj.Name;
    this.modalService.open(content, { size: 'lg', backdropClass: 'light-blue-backdrop', centered: true });
  }
  openViewOnly(content, obj: cTrainingItem) {

    var model = new ModelTraining;
    this.lstCourses = [];
    this.BLServiceUserProfile.getGetByid(obj.TrainerId).subscribe({
      next: user => {
        this.modelObj.TrainerId = user.Name;
      },
      error: err => this.message.Error(err)
    });

    this.BLServiceDataManagement.getTrainingCategoryGetByid(obj.TrainingCategoryId.Id).subscribe({
      next: cat => {

        if (cat) {
          var x = cat.Course?.filter(y => y.IsActive == true).sort((a, b) => {
            if (a.Name > b.Name)
              return 1;
            else
              return -1
          });
          this.lstCourses = x;
        }

      },
      error: err => this.message.Error(err)
    });


    model.EndDate = obj.EndDate;
    model.Id = obj.Id;
    model.PartnerId = obj.PartnerId.Name;
    model.SubPartnerId = obj.SubPartnerId.Name;
    model.StartDate = obj.StartDate;
    model.TrainingCategoryId = obj.TrainingCategoryId.Name;
    model.TrainingTypeId = obj.TrainingTypeId.Name;
    model.days = obj.days;
    model.TrainingCenterId = obj.TrainingCenterId.Name;
    model.Type = obj.Type.toString();
    model.IsOnline = obj.IsOnline;


    var city = this.lstCity?.find(x => x.Id == obj.CityId);
    model.CityId = city?.Name;

    var area = city.Areas.find(x => x.Id == obj.AreaId);
    model.AreaId = area?.Name;

    this.modelObj = model;

    this.modelIsCreate = false;
    this.modalService.open(content, { size: 'lg', backdropClass: 'light-blue-backdrop', centered: true });
  }
  isCourseStarted(startdate: Date) {
    var d = new Date(startdate);
    if (d < this.todayDate) //if start date less than today, course has started
      return true;

    return false;
  }
  isCourseEnded(endtdate: Date) {
    var d = new Date(endtdate);
    if (d < this.todayDate) //if start date less than today, course has started
      return true;

    return false;
  }
  ListTrainees(Id: string) {
    this.router.navigate(['/Training/Trainees/' + Id]);
  }
  Attendance(Id: string) {
    this.router.navigate(['/Training/Attendance/' + Id]);
  }
  Register(Id: string) {
    if (!confirm(this.msgTrainingRegister))
      return;

    this.BLServiceTrainee.TraineeRegister(Id).subscribe({
      next: response => {
        this.message.Success(this.msgSavedSuccessfully);
        this.BLServiceShowMessage.sendMessage(this.message);
        //this.loadData();
        //this.modalService.dismissAll();
      },
      error: err => this.message.Error(err)
    });
  }
  onStartDateSelect(param) {
    this.IsDatesCorrect = this.CompareTwoDates(this.modelObj.StartDate, this.modelObj.EndDate);
  }
  onEndDateSelect(param) {
    this.IsDatesCorrect = this.CompareTwoDates(this.modelObj.StartDate, param);
  }
  onLocationChange(e, fileType: string) {
    //if (e.target.value == "1") {
    if (fileType == "1") {
      this.modelObj.IsOnline = true;
    }
    else {
      this.modelObj.IsOnline = false;
    }
  }
  // }

}
