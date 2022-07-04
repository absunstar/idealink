import { Component, OnInit } from '@angular/core';
import { cArea } from 'src/app/interface/Response/Area.class';
import { ShowMessage } from 'src/app/interface/Model/ModelShowMessage.class';
import { Page } from 'src/app/common/pagination/page';
import { cCityItem } from 'src/app/interface/Response/City.class';
import { ServiceDataManagement } from 'src/app/services/datamanagement.service';
import { Constants } from 'src/app/constants';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Router, ActivatedRoute } from '@angular/router';
import { CustomPaginationService } from 'src/app/common/pagination/services/custom-pagination.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { NgForm } from '@angular/forms';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { TranslateService } from '@ngx-translate/core';
import { TranslateType } from 'src/app/Enum/TranslateType.enum';

@Component({
  selector: 'app-area',
  templateUrl: './area.component.html',
  styleUrls: ['./area.component.scss']
})
export class AreaComponent extends baseComponent implements OnInit {
  pageTitle: string = 'Area';
  message: ShowMessage = new ShowMessage();
  modelName: string = '';
  modelId: string = '';
  modelNameEdited: string = '';
  ModelIsCreate: boolean = true;
  page: Page<cArea> = new Page();

  filtertxt: string = '';

  /////////////

  objCity: cCityItem = new cCityItem();
  AreaId: string = '';
  filterArea: cArea[] = [];

  constructor(private BLServiceDataManagement: ServiceDataManagement,
    private route: ActivatedRoute,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser, BLTranslate: TranslateService,
    private paginationService: CustomPaginationService,
    private router: Router,
    private modalService: NgbModal) {
    super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
  }
  ngOnInit() {
    const param = this.route.snapshot.paramMap.get('id');
    if (param) {
      this.AreaId = param;
      this.page.pageable.pageSize = Constants.PAGE_SIZE;
      this.loadData();
    }
  }
  loadData() {
    this.BLServiceDataManagement.getCityGetByid(this.AreaId).subscribe({
      next: obj => {
        this.objCity = obj;
        this.objCity.Areas = obj.Areas.sort((a,b)=> b.Id.localeCompare(a.Id));
        
        this.filterArea = this.loadDataFilter();
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
  loadDataFilter(): cArea[] {


    var lst = this.filtertxt == "" ? this.objCity.Areas : this.objCity.Areas.filter((item: cArea) =>
      item.Name.toLocaleLowerCase().indexOf(this.filtertxt.toLocaleLowerCase()) !== -1);

    this.page.totalElements = lst.length;
    this.page.content = lst;
    this.page.pageable.pageCurrent = 1
    return lst;
  }
  setActivate(Id: string): void {
    if (!confirm(this.msgsetActivate))
      return;

    this.BLServiceDataManagement.setAreaActivate(this.objCity.Id, Id).subscribe({
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

    this.BLServiceDataManagement.setAreaDeactivate(this.objCity.Id, Id).subscribe({
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
    this.modelId = this.objCity.Id;
    this.modelName = "";
    this.modelNameEdited = "";
    this.ModelIsCreate = true;
    this.modalService.open(content, { backdropClass: 'light-blue-backdrop', centered: true });
  }
  modelSaveBtn(modelForm: NgForm): void {
    if (!modelForm.valid)
      return;


    if (this.ModelIsCreate) {
      this.BLServiceDataManagement.createArea(this.objCity.Id, this.modelName).subscribe({
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
      this.BLServiceDataManagement.updateArea(this.objCity.Id, this.modelId, this.modelName).subscribe({
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
    this.router.navigate(['/DataManagement/City']);
  }
  Translate() {
    this.router.navigate(['/Translate/' + TranslateType.Area + '/' + this.AreaId]);
  }
}
