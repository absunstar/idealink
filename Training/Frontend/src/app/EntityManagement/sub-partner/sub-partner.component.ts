import { Component, OnInit } from '@angular/core';
import { cEntitySubPartnerList, cEntitySubPartnerItem } from 'src/app/interface/Response/EntitySubPartner.class';
import { ShowMessage } from 'src/app/interface/Model/ModelShowMessage.class';
import { cEntityPartnerItem } from 'src/app/interface/Response/EntityPartner.class';
import { Page } from 'src/app/common/pagination/page';
import { ModelEntitySubPartner } from 'src/app/interface/Model/ModelEntitySubPartner.class';
import { ServiceEntityManagement } from 'src/app/services/entitymanagement.service';
import { CustomPaginationService } from 'src/app/common/pagination/services/custom-pagination.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgForm } from '@angular/forms';
import { cEntityTrainingCenterItem } from 'src/app/interface/Response/EntityTrainingCenter.class';
import { noop } from 'rxjs';
import { cTrainingTypeItem } from 'src/app/interface/Response/TrainingType.class';
import { UserType } from 'src/app/Enum/UserType.enum';
import { cUserProfileItem } from 'src/app/interface/Response/UserProfile.class';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-sub-partner',
  templateUrl: './sub-partner.component.html',
  styleUrls: ['./sub-partner.component.css']
})
export class SubPartnerComponent extends baseComponent implements OnInit {


  pageTitle: string = 'Sub Partner';
  lstEntitySubPartner: cEntitySubPartnerList;
  message: ShowMessage = new ShowMessage();
  filtertxt: string = '';
  modelNameEdited: string = '';
  modelObj: ModelEntitySubPartner;
  modelObjSubPartner: cEntitySubPartnerItem;
  modelIsCreate: boolean = true;
  page: Page<cEntitySubPartnerItem> = new Page();
  partnersList: cEntityPartnerItem[];
  TrainingCenterList: cEntityTrainingCenterItem[];
  lstMyPartners: cEntityPartnerItem[];
  popup:boolean = false;
  msgsetAddAll:string;
  msgsetRemoveAll:string;

  constructor(private BLServiceEntityManagement: ServiceEntityManagement,
    private paginationService: CustomPaginationService,
    private modalService: NgbModal,
     BLServiceShowMessage: ServiceShowMessage,BLTranslate: TranslateService,
    BLServiceLoginUser : ServiceLoginUser) { 
      super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
    }


  ngOnInit() {
    this.BLTranslate.get("msgsetAddAll").subscribe(res => { this.msgsetAddAll = res; });
    this.BLTranslate.get("msgsetRemoveAll").subscribe(res => { this.msgsetRemoveAll = res; });
   
    this.loadData();
    this.BLServiceEntityManagement.getEntityPartnerListActive().subscribe({
      next: lst => {
        this.partnersList = lst;
      },
      error: err => this.message.Error(err)
    });
    // this.BLServiceEntityManagement.getEntityTrainingCenterListActive().subscribe({
    //   next: lst => {
    //     this.TrainingCenterList = lst;
    //   },
    //   error: err => this.message.Error(err)
    // });
    this.BLServiceEntityManagement.getEntityPartnerGetMy().subscribe({
      next: lst => {
        this.lstMyPartners = lst;
      },
      error: err => this.message.Error(err)
    });
  }

  private loadData(): void {
    this.BLServiceEntityManagement.getEntitySubPartnerAll(this.page.pageable.pageCurrent, this.filtertxt).subscribe({
      next: lst => {

        this.lstEntitySubPartner = lst;
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
    this.BLServiceEntityManagement.setEntitySubPartnerActivate(Id).subscribe({
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

    this.BLServiceEntityManagement.setEntitySubPartnerDeactivate(Id).subscribe({
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
    this.modelObj = new ModelEntitySubPartner();
    this.modelObjSubPartner = new cEntitySubPartnerItem();
    this.ReconstructeTrainingCenterList();
    this.modelIsCreate = true;
    this.modelNameEdited = "";
    this.modalService.open(content, {size:'lg',backdropClass: 'light-blue-backdrop', centered: true });
  }
  modelSaveBtn(modelForm: NgForm): void {
    if (!modelForm.valid)
      return;

    if (this.modelIsCreate) {
      this.BLServiceEntityManagement.createEntitySubPartner(this.modelObj).subscribe({
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
      this.BLServiceEntityManagement.updateEntitySubPartner(this.modelObj).subscribe({
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
  openBackDropCustomClass(content, obj: cEntitySubPartnerItem) {
    this.modelObjSubPartner = obj;
    this.ReconstructeTrainingCenterList();
    this.modelObj = this.ConvertEntityToModel(obj);
    this.modelIsCreate = false;
    this.modelNameEdited = obj.Name;
    this.modalService.open(content, { size:'lg',  backdropClass: 'light-blue-backdrop', centered: true });
  }
  ReconstructeTrainingCenterList() {
    
    this.TrainingCenterList = [];
    if (!this.modelObjSubPartner.Partners?.length)
      return;

    this.modelObjSubPartner.Partners.forEach(value => {
      value.TrainingCenters.length ? this.TrainingCenterList = this.TrainingCenterList.concat(value.TrainingCenters.filter(item => item.IsActive == true)) : noop;
    });
    this.TrainingCenterList = this.TrainingCenterList.sort((t1, t2) => {
      const name1 = t1.Name.toLowerCase();
      const name2 = t2.Name.toLowerCase();
      if (name1 > name2) { return 1; }
      if (name1 < name2) { return -1; }
      return 0;
    });
  }
  ConvertEntityToModel(obj: cEntitySubPartnerItem): ModelEntitySubPartner {
    var tmpObj = new ModelEntitySubPartner();
    tmpObj.Id = obj.Id;
    tmpObj.Name = obj.Name;
    tmpObj.Phone = obj.Phone;
    tmpObj.PartnerIds = obj.Partners.map(x => x.Id);
    tmpObj.TrainingCenterIds = obj.TrainingCenters.map(x => x.Id);
    return tmpObj;
  }
  //region Assign Sub Partner Account
  LoadSubPartnersMemebers() {
    this.BLServiceEntityManagement.getEntitySubPartnerGetByid(this.modelObj.Id).subscribe({
      next: response => this.modelObjSubPartner = response
    });
  }
  SavePartnersMemebers(UserId: string) {
    if (this.modelObj == null)
      return;
    
    this.BLServiceEntityManagement.memberAddEntitySubPartner(this.modelObjSubPartner.Id, UserId).subscribe({
      next: response => {
        this.message.Success(this.msgSavedSuccessfully);
        this.BLServiceShowMessage.sendMessage(this.message);
        this.loadData();
        this.LoadSubPartnersMemebers();
      },
      error: err => this.message.Error(err)
    });
  }
  Remove(UserId: string) {
    if (!confirm(this.msgsetRemove))
      return;
    this.BLServiceEntityManagement.memberRemoveEntitySubPartner(this.modelObjSubPartner.Id, UserId).subscribe({
      next: response => {
        this.message.Success(this.msgSavedSuccessfully);
        this.BLServiceShowMessage.sendMessage(this.message);
        this.loadData();
        this.LoadSubPartnersMemebers();
      },
      error: err => this.message.Error(err)
    });
  }
  OnSelectedPartner(UserId: string): void {
    this.SavePartnersMemebers(UserId);
  }
  //Endregion Assign Sub Partner Account
  /////////////////////////////
  //region Assign Partner Entity
  RemovePartnerEntity(PartnerId: string) {
    if (!confirm(this.msgsetRemove))
      return;
    this.BLServiceEntityManagement.RemovePartnerEntityToEntitySubPartner(PartnerId, this.modelObjSubPartner.Id).subscribe({
      next: response => {
        this.message.Success(this.msgSavedSuccessfully);
        this.BLServiceShowMessage.sendMessage(this.message);
        this.LoadSubPartnersEntity();
        this.loadData();
      },
      error: err => this.message.Error(err)
    });
  }
  OnSelectedPartnerEntity(UserId: string): void {
    this.SavePartnersEntity(UserId);
  }
  SavePartnersEntity(PartnerId: string) {
    if (this.modelObj == null)
      return;
    
    this.BLServiceEntityManagement.AddPartnerEntityToEntitySubPartner(PartnerId, this.modelObjSubPartner.Id).subscribe({
      next: response => {
        this.message.Success(this.msgSavedSuccessfully);
        this.BLServiceShowMessage.sendMessage(this.message);
        this.LoadSubPartnersEntity();
        this.loadData();
      },
      error: err => this.message.Error(err)
    });
  }
  
  LoadSubPartnersEntity() {
    this.BLServiceEntityManagement.getEntitySubPartnerGetByid(this.modelObj.Id).subscribe({
      next: response => {
        this.modelObjSubPartner = response;
        this.ReconstructeTrainingCenterList();
      }
    });
  }
  //Endregion Assign Partner Entity
  //region TrainingCenters
  SaveTrainingCenterEntity(TrainingCenterId: string) {
    if (this.modelObj == null)
      return;
    
    this.BLServiceEntityManagement.AddEntityTrainingCenterToSubPartner(TrainingCenterId,this.modelObjSubPartner.Id ).subscribe({
      next: response => {
        this.message.Success(this.msgSavedSuccessfully);
        this.BLServiceShowMessage.sendMessage(this.message);
        this.LoadSubPartnersEntity();
        this.loadData();
      },
      error: err => this.message.Error(err)
    });
  }

  RemoveTrainingCenterEntity(TrainingCenterId: string) {
    if (!confirm(this.msgsetRemove))
      return;
    this.BLServiceEntityManagement.RemoveEntityTrainingCenterToSubPartner(TrainingCenterId,this.modelObjSubPartner.Id).subscribe({
      next: response => {
        this.message.Success(this.msgSavedSuccessfully);
        this.BLServiceShowMessage.sendMessage(this.message);
        this.LoadSubPartnersEntity();
        this.loadData();
      },
      error: err => this.message.Error(err)
    });
  }
  AddAllPartnerTrainingCenter(PartnerId: string) {
    if (!confirm(this.msgsetAddAll))
      return;
    this.BLServiceEntityManagement.AddEntityTrainingCenterToSubPartnerByPartnerID(PartnerId,this.modelObjSubPartner.Id).subscribe({
      next: response => {
        this.message.Success(this.msgSavedSuccessfully);
        this.BLServiceShowMessage.sendMessage(this.message);
        this.LoadSubPartnersEntity();
        this.loadData();
      },
      error: err => this.message.Error(err)
    });
  }
  RemoveAllPartnerTrainingCenter(PartnerId: string) {
    if (!confirm(this.msgsetRemoveAll))
      return;
    this.BLServiceEntityManagement.RemoveEntityTrainingCenterToSubPartnerByPartnerID(PartnerId,this.modelObjSubPartner.Id).subscribe({
      next: response => {
        this.message.Success(this.msgSavedSuccessfully);
        this.BLServiceShowMessage.sendMessage(this.message);
        this.LoadSubPartnersEntity();
        this.loadData();
      },
      error: err => this.message.Error(err)
    });
  }
  OnSelectedTrainingCenter(Id:string)
  {
    
    this.SaveTrainingCenterEntity(Id);
  }
  //EndRegion training Centers
  filterSubPartnerAccount(obj:cUserProfileItem[]){;
    if(obj.length > 0)
    return obj.filter(x=>x.Type == UserType.SubPartner.toString());
  }
}
