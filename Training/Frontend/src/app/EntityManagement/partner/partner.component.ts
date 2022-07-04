import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgForm } from '@angular/forms';

import { cEntityPartnerList, cEntityPartnerItem } from 'src/app/interface/Response/EntityPartner.class';
import { ShowMessage } from 'src/app/interface/Model/ModelShowMessage.class';
import { Page } from 'src/app/common/pagination/page';
import { CustomPaginationService } from 'src/app/common/pagination/services/custom-pagination.service';

import { ServiceEntityManagement } from 'src/app/services/entitymanagement.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { cUserProfileItem } from 'src/app/interface/Response/UserProfile.class';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';


@Component({
  selector: 'app-partner',
  templateUrl: './partner.component.html',
  styleUrls: ['./partner.component.scss']
})
export class PartnerComponent extends baseComponent implements OnInit {

  pageTitle: string = 'Partner';
  lstEntityPartner: cEntityPartnerList;
  PartnerMembers: cUserProfileItem[];
  filtertxt: string = '';
  modelNameEdited: string = '';
  modelObj: cEntityPartnerItem;
  modelIsCreate: boolean = true;
  page: Page<cEntityPartnerItem> = new Page();
  MemberId: string;
  IsHoursCorrect: boolean = false;

  constructor(private BLServiceEntityManagement: ServiceEntityManagement,
    private paginationService: CustomPaginationService,
    private modalService: NgbModal,
    private router: Router,
    BLServiceShowMessage: ServiceShowMessage, BLTranslate: TranslateService,
    BLServiceLoginUser: ServiceLoginUser) {
    super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
  }


  ngOnInit() {
    this.loadData();
  }
  private loadData(): void {
    this.BLServiceEntityManagement.getEntityPartnerAll(this.page.pageable.pageCurrent, this.filtertxt).subscribe({
      next: lst => {
        this.lstEntityPartner = lst;
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
    this.BLServiceEntityManagement.setEntityPartnerActivate(Id).subscribe({
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

    this.BLServiceEntityManagement.setEntityPartnerDeactivate(Id).subscribe({
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
    // this.modelId = "-1";
    // this.modelName = "";
    this.modelObj = new cEntityPartnerItem;
    this.modelIsCreate = true;
    this.modelNameEdited = "";
    this.modalService.open(content, { backdropClass: 'light-blue-backdrop', centered: true });
  }
  modelSaveBtn(modelForm: NgForm): void {
    if (!modelForm.valid)
      return;

    if (this.modelIsCreate) {
      this.BLServiceEntityManagement.createEntityPartner(this.modelObj).subscribe({
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
      this.BLServiceEntityManagement.updateEntityPartner(this.modelObj).subscribe({
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
  openBackDropCustomClass(content, obj: cEntityPartnerItem) {
    this.modelObj = obj;
    this.LoadPartnersMemebers();
    this.modelIsCreate = false;
    this.modelNameEdited = obj.Name;
    this.modalService.open(content, { backdropClass: 'light-blue-backdrop', centered: true });
  }
  LoadPartnersMemebers() {
    this.BLServiceEntityManagement.getEntityPartnerGetByid(this.modelObj.Id).subscribe({
      next: response => this.modelObj = response
    });
  }
  SavePartnersMemebers(UserId: string) {
    if (this.modelObj == null)
      return;
    this.BLServiceEntityManagement.memberAddEntityPartner(this.modelObj.Id, UserId).subscribe({
      next: response => {
        this.message.Success(this.msgSavedSuccessfully);
        this.BLServiceShowMessage.sendMessage(this.message);
        this.loadData();
        this.LoadPartnersMemebers();
      },
      error: err => this.message.Error(err)
    });
  }
  Remove(UserId: string) {
    if (!confirm(this.msgsetRemove))
      return;
    this.BLServiceEntityManagement.memberRemoveEntityPartner(this.modelObj.Id, UserId).subscribe({
      next: response => {
        this.message.Success(this.msgSavedSuccessfully);
        this.BLServiceShowMessage.sendMessage(this.message);
        this.loadData();
        this.LoadPartnersMemebers();
      },
      error: err => this.message.Error(err)
    });
  }
  OnSelectedPartner(UserId: string): void {
    this.SavePartnersMemebers(UserId);
  }
  GoToTrainingCenters(Id: string) {
    this.router.navigate(['/EntityManagement/TrainingCenter/' + Id]);
  }
  onHoursSelect(event: any) {
    this.IsHoursCorrect = this.modelObj.MinHours > this.modelObj.MaxHours;
    
  }
}
