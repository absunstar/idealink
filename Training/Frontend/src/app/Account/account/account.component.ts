import { Component, OnInit } from '@angular/core';
import { cUserProfileList, cUserProfileItem } from 'src/app/interface/Response/UserProfile.class';
import { Page } from 'src/app/common/pagination/page';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { CustomPaginationService } from 'src/app/common/pagination/services/custom-pagination.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ServiceUserProfile } from 'src/app/services/userprofile.service';
import { NgForm, FormControl, Validators } from '@angular/forms';
import { ModelUserProfile } from 'src/app/interface/Model/ModelUserProfile.class';
import { UserTypeItem, UserTypeList } from 'src/app/interface/Model/ModelUserType.class';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { UserType } from 'src/app/Enum/UserType.enum';
import { ServiceEntityManagement } from 'src/app/services/entitymanagement.service';
import { cEntityPartnerItem } from 'src/app/interface/Response/EntityPartner.class';
import { cEntitySubPartnerItem } from 'src/app/interface/Response/EntitySubPartner.class';
import { cCityList, cCityItem } from 'src/app/interface/Response/City.class';
import { ServiceDataManagement } from 'src/app/services/datamanagement.service';
import { TranslateService } from '@ngx-translate/core';
import { cArea } from 'src/app/interface/Response/Area.class';
import { noop } from 'rxjs';
import { Router } from '@angular/router';
import { ModelChangeEmail } from 'src/app/interface/Model/ModelChangeEmail.class';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss']
})
export class AccountComponent extends baseComponent implements OnInit {
  emailPattern = "^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$";
  get EnumUserTypes() { return UserType };
  pageTitle: string = 'User Accounts';
  lstUserProfile: cUserProfileList;
  lstUserType: UserTypeItem[];
  filterType: string = "0";
  filtertxt: string = '';
  modelNameEdited: string = '';
  modelObj: cUserProfileItem;
  modelIsCreate: boolean = true;
  page: Page<cUserProfileItem> = new Page();
  lstPartners: cEntityPartnerItem[];
  lstPartnersSelected: string[];
  lstSubPartners: cEntitySubPartnerItem[];
  lstSubPartnersSelected: string[];
  lstCity: cCityItem[];
  lstArea: cArea[];
  isClicked = false;
  IsDatesCorrect: boolean = true;
  modelChangeEmail: ModelChangeEmail = new ModelChangeEmail();

  AccountActivationLinkFailed: string;
  AccountActivationLinkSuccess: string;
  AccountPasswordSucess: string;
  AccountPasswordFailed: string;

  constructor(private BLServiceUserProfile: ServiceUserProfile,
    private BLServiceEntityManagement: ServiceEntityManagement,
    private BLServiceDataManagement: ServiceDataManagement,
    private paginationService: CustomPaginationService,
    private modalService: NgbModal,
    private router: Router,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser, BLTranslate: TranslateService,) {
    super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
    this.BLTranslate.get("AccountActivationLinkFailed").subscribe(res => { this.AccountActivationLinkFailed = res; });
    this.BLTranslate.get("AccountActivationLinkSuccess").subscribe(res => { this.AccountActivationLinkSuccess = res; });
    this.BLTranslate.get("AccountPasswordSucess").subscribe(res => { this.AccountPasswordSucess = res; });
    this.BLTranslate.get("AccountPasswordFailed").subscribe(res => { this.AccountPasswordFailed = res; });

  }


  ngOnInit() {
    this.loadData();
    this.lstUserType = new UserTypeList(this.BLTranslate).getUserListByType(this.userRole);
    this.BLServiceDataManagement.getCityListActive().subscribe({
      next: lst => {
        this.lstCity = lst;
      },
      error: err => this.message.Error(err)
    });
    this.BLServiceEntityManagement.getEntityPartnerListActive("").subscribe({
      next: lst => {
        this.lstPartners = lst;
      },
      error: err => this.message.Error(err)
    });
  }
  private loadData(): void {
    this.BLServiceUserProfile.getSearch(this.page.pageable.pageCurrent, this.filtertxt, this.filterType).subscribe({
      next: lst => {

        this.lstUserProfile = lst;
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
  ResendActivationLink(Email: string) {
    this.BLServiceUserProfile.ResendActivationLink(Email).subscribe({
      next: response => {
        this.message.Success(this.AccountActivationLinkSuccess);
        this.BLServiceShowMessage.sendMessage(this.message);
        //this.loadData();
      },
      error: err => {
        this.message.Error(this.AccountActivationLinkFailed);
        this.BLServiceShowMessage.sendMessage(this.message);
      }
    });
  }
  ResendPasswordLink(Email: string) {
    this.BLServiceUserProfile.ResendPasswordLink(Email).subscribe({
      next: response => {
        this.message.Success(this.AccountPasswordSucess);
        this.BLServiceShowMessage.sendMessage(this.message);
        //this.loadData();
      },
      error: err => {
        this.message.Error(this.AccountPasswordFailed);
        this.BLServiceShowMessage.sendMessage(this.message);
      }
    });
  }
  setActivate(Id: string): void {
    if (!confirm(this.msgsetActivate))
      return;
    this.BLServiceUserProfile.setActivate(Id).subscribe({
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

    this.BLServiceUserProfile.setDeactivate(Id).subscribe({
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
    this.modelObj = new cUserProfileItem;
    this.modelIsCreate = true;
    this.isClicked = false;
    this.modelNameEdited = "";
    this.lstPartnersSelected = [];
    this.lstSubPartnersSelected = [];
    this.modalService.open(content, { backdropClass: 'light-blue-backdrop', centered: true });
  }
  modelSaveBtn(modelForm: NgForm): void {
    if (!modelForm.valid)
      return;
    this.isClicked = true;
    var model = new ModelUserProfile();
    model.Id = this.modelObj.Id;
    model.Name = this.modelObj.Name;
    model.Type = UserType[this.modelObj.Type]
    model.Email = this.modelObj.Email;
    model.CityId = this.modelObj.CityId;
    model.AreaId = this.modelObj.AreaId;
    model.TrainerTrainingDetails = this.modelObj.TrainerTrainingDetails;
    model.TrainerStartDate = this.modelObj.TrainerStartDate;
    model.TrainerEndDate = this.modelObj.TrainerEndDate;
    model.SelectedPartnerEntityId = this.lstPartnersSelected;
    model.SelectedSubPartnerEntityId = this.lstSubPartnersSelected;


    if (this.modelIsCreate) {
      this.BLServiceUserProfile.create(model).subscribe({
        next: response => {
          this.message.Success(this.msgSavedSuccessfully);
          this.BLServiceShowMessage.sendMessage(this.message);
          this.loadData();
          this.isClicked = false;
          this.modalService.dismissAll();
        },
        error: err => {
          this.message.Error(err)
          this.isClicked = false;
        }
      });
    }
    else {
      this.BLServiceUserProfile.update(model).subscribe({
        next: response => {
          this.message.Success(this.msgSavedSuccessfully);
          this.BLServiceShowMessage.sendMessage(this.message);
          this.loadData();
          this.isClicked = false;
          this.modalService.dismissAll();
        },
        error: err => {
          this.message.Error(err);
          this.isClicked = false;
        }
      });
    }
  }
  openBackDropCustomClass(content, obj: cUserProfileItem) {
    this.modelObj = obj;

    this.modelObj.TrainerStartDate.toString() == "0001-01-01T00:00:00Z" ? this.modelObj.TrainerStartDate = null : noop;
    this.modelObj.TrainerEndDate.toString() == "0001-01-01T00:00:00Z" ? this.modelObj.TrainerEndDate = null : noop;

    this.modelObj.Type = obj.Type.toString();
    this.modelIsCreate = false;
    this.isClicked = false;
    this.modelNameEdited = obj.Name;
    this.lstPartnersSelected = this.modelObj?.MyPartnerListIds?.map(({ Id }) => Id);
    this.lstSubPartnersSelected = this.modelObj?.MySubPartnerListIds?.map(({ Id }) => Id);
    this.PartnerChange(this.lstPartnersSelected);
    this.CityChange(this.modelObj.CityId)
    this.modalService.open(content, { backdropClass: 'light-blue-backdrop', centered: true });
  }
  ConvertUserType(type: number) {
    return UserType[type];
  }
  RemovePartner(PartnerId: string) {
    if (!confirm(this.msgsetRemove))
      return;
    this.BLServiceEntityManagement.memberRemoveEntityPartner(PartnerId, this.modelObj.Id).subscribe({
      next: lst => {
        this.message.Success(this.msgSavedSuccessfully);
        this.BLServiceShowMessage.sendMessage(this.message);
        this.ReloadTheAccount(this.modelObj.Id);
      },
      error: err => this.message.Error(err)
    });
  }
  OnSelectedPartner(PartnerId: string): void {

    this.BLServiceEntityManagement.memberAddEntityPartner(PartnerId, this.modelObj.Id).subscribe({
      next: lst => {
        this.ReloadTheAccount(this.modelObj.Id);
        this.message.Success(this.msgSavedSuccessfully);
        this.BLServiceShowMessage.sendMessage(this.message);
      },
      error: err => this.message.Error(err)
    });
  }
  RemoveSubPartner(PartnerId: string) {
    if (!confirm(this.msgsetRemove))
      return;
    this.BLServiceEntityManagement.memberRemoveEntitySubPartner(PartnerId, this.modelObj.Id).subscribe({
      next: lst => {
        this.message.Success(this.msgSavedSuccessfully);
        this.BLServiceShowMessage.sendMessage(this.message);
        this.ReloadTheAccount(this.modelObj.Id);
      },
      error: err => this.message.Error(err)
    });
  }
  OnSelectedSubPartner(PartnerId: string): void {

    this.BLServiceEntityManagement.memberAddEntitySubPartner(PartnerId, this.modelObj.Id).subscribe({
      next: lst => {
        this.ReloadTheAccount(this.modelObj.Id);
        this.message.Success(this.msgSavedSuccessfully);
        this.BLServiceShowMessage.sendMessage(this.message);
      },
      error: err => this.message.Error(err)
    });
  }
  ReloadTheAccount(UserId: string) {
    this.BLServiceUserProfile.getGetByid(UserId).subscribe({
      next: obj => {

        this.modelObj = obj;
        this.loadData();
      },
      error: err => this.message.Error(err)
    });
  }
  PartnerChange(obj: any) {

    this.BLServiceEntityManagement.getEntitySubPartnerListActive("", this.lstPartnersSelected).subscribe({
      next: lst => {

        this.lstSubPartners = lst;
        this.loadData();
      },
      error: err => this.message.Error(err)
    });
  }
  CityChange(obj: any) {

    if (obj != null) {
      var city = this.lstCity?.find(item => item.Id == this.modelObj.CityId);
      if (city) {
        this.lstArea = city?.Areas.filter(a => a.IsActive == true).sort((x, y) => x.Name.localeCompare(y.Name));;
      }
    }
  }
  // filterAccounts(type: number, obj: cAssignedToAccount[]) {
  //   return obj.filter(x => x.AccountType == type);
  // }
  onStartDateSelect(param) {
    if ((this.modelObj.TrainerStartDate == null && this.modelObj.TrainerEndDate == null) ||
      (this.modelObj.TrainerStartDate != null && this.modelObj.TrainerEndDate == null))
      this.IsDatesCorrect = true;
    else
      this.IsDatesCorrect = this.CompareTwoDates(this.modelObj.TrainerStartDate, this.modelObj.TrainerEndDate);
  }
  onEndDateSelect(param) {

    if ((this.modelObj.TrainerStartDate == null && this.modelObj.TrainerEndDate == null) ||
      (this.modelObj.TrainerStartDate != null && this.modelObj.TrainerEndDate == null))
      this.IsDatesCorrect = true;
    else
      this.IsDatesCorrect = this.CompareTwoDates(this.modelObj.TrainerStartDate, param);
  }
  Certificate(Id) {
    // path: 'Trainee/Create/:IsCreate/:TraineeId/:TrainingId'
    this.router.navigate(['/TrainerCertificate/' + Id]);
  }
  ChangeEmail(content, emailOld: string) {
    this.modelChangeEmail = new ModelChangeEmail();
    this.modelChangeEmail.EmailOld = emailOld;

    this.modalService.open(content, { backdropClass: 'light-blue-backdrop', centered: true });
  }
  ChangeEmailSave(modelForm: NgForm): void {
    if (!modelForm.valid)
      return;

    this.BLServiceUserProfile.UpdateUserEmail(this.modelChangeEmail).subscribe({
      next: lst => {
        this.loadData();
        this.message.Success(this.msgSavedSuccessfully);
        this.BLServiceShowMessage.sendMessage(this.message);
        this.modalService.dismissAll();
      },
      error: err => this.message.Error(err)
    });
  }
}
