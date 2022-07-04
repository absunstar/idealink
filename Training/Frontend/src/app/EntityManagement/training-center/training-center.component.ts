import { Component, OnInit } from '@angular/core';
import { cEntityTrainingCenterList, cEntityTrainingCenterItem } from 'src/app/interface/Response/EntityTrainingCenter.class';
import { ShowMessage } from 'src/app/interface/Model/ModelShowMessage.class';
import { Page } from 'src/app/common/pagination/page';
import { ServiceEntityManagement } from 'src/app/services/entitymanagement.service';
import { CustomPaginationService } from 'src/app/common/pagination/services/custom-pagination.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgForm } from '@angular/forms';
import { cEntityPartnerItem } from 'src/app/interface/Response/EntityPartner.class';
import { ModelEntityTrainingCenter } from 'src/app/interface/Model/ModelEntityTrainingCenter.class';
import { ActivatedRoute, Router } from '@angular/router';
import { cTrainingTypeItem } from 'src/app/interface/Response/TrainingType.class';
import { Constants } from 'src/app/constants';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-training-center',
  templateUrl: './training-center.component.html',
  styleUrls: ['./training-center.component.scss']
})
export class TrainingCenterComponent extends baseComponent implements OnInit {

  pageTitle: string = 'Training Center';
  message: ShowMessage = new ShowMessage();
  modelNameEdited: string = '';
  modelObj: ModelEntityTrainingCenter;
  modelIsCreate: boolean = true;

  ///////
  partnerObj: cEntityPartnerItem;
  PartnerId: string;
  lstEntityTrainingCenterfilter: cEntityTrainingCenterItem[];
  page: Page<cEntityTrainingCenterItem> = new Page();

  private _filtertxt: string = '';
  public get filtertxt(): string {
    return this._filtertxt;
  }
  public set filtertxt(value: string) {
    this._filtertxt = value;
    this.lstEntityTrainingCenterfilter = this.filtertxt ? this.loadDataFilter() : this.partnerObj.TrainingCenters;
  }

  constructor(private BLServiceEntityManagement: ServiceEntityManagement,
    private paginationService: CustomPaginationService,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser,BLTranslate: TranslateService,
    private route: ActivatedRoute,
    private router: Router,
    private modalService: NgbModal) { 
      super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
    }


  ngOnInit() {
    const param = this.route.snapshot.paramMap.get('id');
    if (param) {
      this.PartnerId = param;
      this.page.pageable.pageSize = Constants.PAGE_SIZE;
      this.loadData();
    }


  }
  private loadData(): void {
    this.BLServiceEntityManagement.getEntityPartnerGetByid(this.PartnerId).subscribe({
      next: data => {
        this.partnerObj = data;
        this.lstEntityTrainingCenterfilter = this.loadDataFilter();;
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
  setActivate(Id: string): void {
    if (!confirm(this.msgsetActivate))
      return;
    this.BLServiceEntityManagement.setEntityTrainingCenterActivate(this.PartnerId,Id).subscribe({
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

    this.BLServiceEntityManagement.setEntityTrainingCenterDeactivate(this.PartnerId,Id).subscribe({
      next: response => {
        this.message.Success(this.msgDeactivatedSuccessfully);
        this.BLServiceShowMessage.sendMessage(this.message);
        this.loadData();
      },
      error: err => this.message.Error(err)
    });
  }
  loadDataFilter() {
    var lst = this.partnerObj.TrainingCenters.filter((item: cTrainingTypeItem) =>
      item.Name.toLocaleLowerCase().indexOf(this.filtertxt.toLocaleLowerCase()) !== -1);

    this.page.totalElements = lst.length;
    this.page.content = lst;

    return lst;
  }
  createBtn(content): void {
    this.modelObj = new ModelEntityTrainingCenter();
    this.modelIsCreate = true;
    this.modelNameEdited = "";
    this.modalService.open(content, { backdropClass: 'light-blue-backdrop', centered: true });
  }
  modelSaveBtn(modelForm: NgForm): void {
    if (!modelForm.valid)
      return;

    this.modelObj.PartnerId = this.PartnerId;

    if (this.modelIsCreate) {
      this.BLServiceEntityManagement.createEntityTrainingCenter(this.modelObj).subscribe({
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
      this.BLServiceEntityManagement.updateEntityTrainingCenter(this.modelObj).subscribe({
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
  openBackDropCustomClass(content, obj: cEntityTrainingCenterItem) {
    var tmpObj = new ModelEntityTrainingCenter();
    tmpObj.Id = obj.Id;
    tmpObj.Name = obj.Name;
    tmpObj.Phone = obj.Phone;
    this.modelObj = tmpObj;
    this.modelIsCreate = false;
    this.modelNameEdited = obj.Name;
    this.modalService.open(content, { backdropClass: 'light-blue-backdrop', centered: true });
  }
  onBack(): void {
    this.router.navigate(['/EntityManagement/Partner']);
  }
}
