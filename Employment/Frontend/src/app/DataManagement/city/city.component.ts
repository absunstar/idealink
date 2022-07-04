import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgForm } from '@angular/forms';

import { ShowMessage } from 'src/app/interface/Model/ModelShowMessage.class';
import { CustomPaginationService } from 'src/app/common/pagination/services/custom-pagination.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { Page } from 'src/app/common/pagination/page';
import { cGenericIdNameItem, cGenericSubItem } from 'src/app/interface/Response/GenericIdName.class';
import { ServiceCountry } from 'src/app/services/country.service';
import { Router, ActivatedRoute } from '@angular/router';
import { Constants } from 'src/app/constants';
import { ModelSubEntity } from 'src/app/interface/Model/ModelSubEntity.class';
import { ConfirmationDialogService } from 'src/app/common/confirmation-dialog/confirmation-dialog.service';
import { TranslateType } from 'src/app/Enum/TranslateType.enum';

@Component({
  selector: 'app-city',
  templateUrl: './city.component.html',
  styleUrls: ['./city.component.css']
})
export class CityComponent implements OnInit {


  pageTitle: string = 'City';
  lstResult: cGenericSubItem[];
  message: ShowMessage = new ShowMessage();
  filtertxt: string = '';
  modelObj: ModelSubEntity;
  modelIsCreate: boolean;
  modelNameEdited: string = '';
  MainId: string;
  CountryObj: cGenericIdNameItem;

  page: Page<cGenericSubItem> = new Page();

  constructor(private BLService: ServiceCountry,
    private paginationService: CustomPaginationService,
    private BLServiceShowMessage: ServiceShowMessage,
    private modalService: NgbModal,
    private confirmationDialogService: ConfirmationDialogService,
    private route: ActivatedRoute,
    private router: Router) { }


  ngOnInit() {
    const param = this.route.snapshot.paramMap.get('Id');
    if (param) {
      this.MainId = param;
      this.page.pageable.pageSize = Constants.PAGE_SIZE;
      this.loadData();
    }
  }
  private loadData(): void {
    this.BLService.getGetByid(this.MainId).subscribe({
      next: lst => {
        this.CountryObj = lst;
        this.lstResult = lst.subItems;
        this.page.totalElements = lst.subItems.length;
        this.page.content = lst.subItems;
      },
      error: err => this.message.Error(err)
    });
  }
  setActivate(Id: string): void {
    this.confirmationDialogService.confirmActivation()
      .then((confirmed) => {
        if (!confirmed)
          return;

        var model = new ModelSubEntity();
        model.Id = Id;
        model.MainId = this.MainId;

        this.BLService.SubActivate(model).subscribe({
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
          
        var model = new ModelSubEntity();
        model.Id = Id;
        model.MainId = this.MainId;

        this.BLService.SubDeActivate(model).subscribe({
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
    var lst = this.CountryObj.subItems.filter((item: cGenericSubItem) =>
      item.Name.toLocaleLowerCase().indexOf(this.filtertxt.toLocaleLowerCase()) !== -1);

    this.page.totalElements = lst.length;
    this.page.content = lst;
    this.lstResult = lst;
    this.page.totalElements = lst.length;
    this.page.content = lst;
  }
  createBtn(content): void {
    this.modelIsCreate = true;
    
    this.modelObj = new ModelSubEntity();
    this.modelObj.MainId = this.MainId;

    this.modelNameEdited = "";
    this.modalService.open(content, { backdropClass: 'light-blue-backdrop', centered: true });
  }
  modelSaveBtn(modelForm: NgForm): void {
    this.modelObj.Name = this.modelObj.Name.trim();

    if (!modelForm.valid || this.modelObj.Name == "")
      return;
    
    if (this.modelIsCreate) {
      this.BLService.SubCreate(this.modelObj).subscribe({
        next: response => {
          this.message.Success("Saved successfully.");
          this.BLServiceShowMessage.sendMessage(this.message);
          this.loadData();
          this.modalService.dismissAll();
        },
        error: err => this.message.Error(err)
      });
    }
    else {
      this.BLService.SubUpdate(this.modelObj).subscribe({
        next: response => {
          this.message.Success("Saved successfully.");
          this.BLServiceShowMessage.sendMessage(this.message);
          this.loadData();
          this.modalService.dismissAll();
        },
        error: err => this.message.Error(err)
      });
    }
  }
  openBackDropCustomClass(content, obj: cGenericIdNameItem) {
    this.modelObj = new ModelSubEntity();
    this.modelObj.Id = obj._id;
    this.modelObj.Name = obj.Name;
    this.modelObj.MainId = this.MainId;

    this.modelNameEdited = obj.Name;
    this.modelIsCreate = false;
    this.modalService.open(content, { backdropClass: 'light-blue-backdrop', centered: true });
  }

  onBack(): void {
    this.router.navigate(['/DataManagement/Country']);
  }
  Translate(){
    this.router.navigate(['/Translate/' + TranslateType.City + '/' + this.MainId]);
  }
}
