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
import { ConfirmationDialogService } from 'src/app/common/confirmation-dialog/confirmation-dialog.service';
import { TranslateService } from '@ngx-translate/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss']
})
export class AccountComponent extends baseComponent implements OnInit {
  
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
  isClicked = false;
  AccountActivationLinkFailed: string;
  AccountActivationLinkSuccess: string;
  AccountPasswordSucess:string;
  AccountPasswordFailed:string;

  constructor(private BLServiceUserProfile: ServiceUserProfile,
    private router: Router,
    private paginationService: CustomPaginationService,
    private modalService: NgbModal,
    private confirmationDialogService: ConfirmationDialogService,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser,BLTranslate: TranslateService,) {
    super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
    this.BLTranslate.get("AccountActivationLinkFailed").subscribe(res => { this.AccountActivationLinkFailed = res; });
    this.BLTranslate.get("AccountActivationLinkSuccess").subscribe(res => { this.AccountActivationLinkSuccess = res; });
    this.BLTranslate.get("AccountPasswordSucess").subscribe(res => { this.AccountPasswordSucess = res; });
    this.BLTranslate.get("AccountPasswordFailed").subscribe(res => { this.AccountPasswordFailed = res; });
   
  }


  ngOnInit() {
    this.loadData();
    this.lstUserType = new UserTypeList().getUserTypes();
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
  setActivate(Id: string): void {
    this.confirmationDialogService.confirmActivation()
      .then((confirmed) => {
        if (!confirmed)
          return;
        this.BLServiceUserProfile.setActivate(Id).subscribe({
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
        this.BLServiceUserProfile.setDeActivate(Id).subscribe({
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
    this.modelObj = new cUserProfileItem;
    this.modelIsCreate = true;
    this.isClicked = false;
    this.modelNameEdited = "";
    this.modalService.open(content, { backdropClass: 'light-blue-backdrop', centered: true });
  }
  modelSaveBtn(modelForm: NgForm): void {
    if (!modelForm.valid)
      return;
    this.isClicked = true;
    var model = new ModelUserProfile();
    model.Id = this.modelObj.Id;
    model.Name = this.modelObj.Name;
    model.Type = this.modelObj.Type;
    model.Email = this.modelObj.Email;
    

    if (this.modelIsCreate) {
      model.Type = this.EnumUserTypes.Admin;
      this.BLServiceUserProfile.create(model).subscribe({
        next: response => {
          this.message.Success("Saved successfully.");
          this.BLServiceShowMessage.sendMessage(this.message);
          this.loadData();
          this.isClicked = false;
          this.modalService.dismissAll();
        },
        error: err => {this.message.Error(err)
          this.isClicked = false;}
      });
    }
    else {
      this.BLServiceUserProfile.update(model).subscribe({
        next: response => {
          this.message.Success("Saved successfully.");
          this.BLServiceShowMessage.sendMessage(this.message);
          this.loadData();
          this.isClicked = false;
          this.modalService.dismissAll();
        },
        error: err => { this.message.Error(err);
          this.isClicked = false; }
      });
    }
  }
  openBackDropCustomClass(content, obj: cUserProfileItem) {
    this.modelObj = obj;
    this.modelIsCreate = false;
    this.isClicked = false;
    this.modelNameEdited = obj.Name;
    this.modalService.open(content, { backdropClass: 'light-blue-backdrop', centered: true });
  }
  ConvertUserType(type: number) {
    return UserType[type];
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
  onView(Id: string) {
    this.router.navigate(['JobSeeker/Resume/' + Id]);
  }
  onLimit(UserId: string) {
    this.BLServiceUserProfile.Limit(UserId).subscribe({
      next: obj => {
        this.loadData();
      },
      error: err => this.message.Error(err)
    });
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
}
