import { Component, OnInit } from '@angular/core';
import { cCityList, cCityItem } from 'src/app/interface/Response/City.class';
import { ShowMessage } from 'src/app/interface/Model/ModelShowMessage.class';
import { Page } from 'src/app/common/pagination/page';
import { ServiceDataManagement } from 'src/app/services/datamanagement.service';
import { CustomPaginationService } from 'src/app/common/pagination/services/custom-pagination.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgForm } from '@angular/forms';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { TranslateService } from '@ngx-translate/core';
import { TranslateType } from 'src/app/Enum/TranslateType.enum';

@Component({
  selector: 'app-city',
  templateUrl: './city.component.html',
  styleUrls: ['./city.component.scss']
})
export class CityComponent extends baseComponent implements OnInit {
  pageTitle: string = 'City';
  lstCity: cCityList;
  message: ShowMessage = new ShowMessage();
  filtertxt: string = '';
  modelName: string = '';
  modelId: string = '';
  modelNameEdited: string = '';

  page: Page<cCityItem> = new Page();

  constructor(private BLServiceDataManagement: ServiceDataManagement,
    private paginationService: CustomPaginationService,
    private router: Router,
    private modalService: NgbModal,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser,
    BLTranslate: TranslateService) {
    super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
  }
  ngOnInit() {
    this.loadData();
  }
  private loadData(): void {
    this.BLServiceDataManagement.getCityAll(this.page.pageable.pageCurrent, this.filtertxt).subscribe({
      next: lst => {
        this.lstCity = lst;
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

    this.BLServiceDataManagement.setCityActivate(Id).subscribe({
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

    this.BLServiceDataManagement.setCityDeactivate(Id).subscribe({
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
    this.modelName = "";
    this.modelNameEdited = "";
    this.modalService.open(content, { backdropClass: 'light-blue-backdrop', centered: true });
  }
  modelSaveBtn(modelForm: NgForm): void {
    if (!modelForm.valid)
      return;


    if (this.modelId == "-1") {
      this.BLServiceDataManagement.createCity(this.modelName).subscribe({
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
      this.BLServiceDataManagement.updateCity(this.modelId, this.modelName).subscribe({
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
    this.modalService.open(content, { backdropClass: 'light-blue-backdrop', centered: true });
  }
  Areas(Id: string) {
    this.router.navigate(['/DataManagement/Area/' + Id]);
  }
  Translate(){
    this.router.navigate(['/Translate/' + TranslateType.City + '/0']);
  }
}
