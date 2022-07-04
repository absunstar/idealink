import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { Page } from 'src/app/common/pagination/page';
import { CustomPaginationService } from 'src/app/common/pagination/services/custom-pagination.service';
import { Constants } from 'src/app/constants';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ShowMessage } from 'src/app/interface/Model/ModelShowMessage.class';
import { cLogoPartnerItem, cLogoPartnerList } from 'src/app/interface/Response/LogoPartner.class';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { ServiceLogoPartner } from 'src/app/services/logo-partner.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';

@Component({
  selector: 'app-partner-logo',
  templateUrl: './partner-logo.component.html',
  styleUrls: ['./partner-logo.component.css']
})
export class PartnerLogoComponent extends baseComponent implements OnInit {
  message: ShowMessage = new ShowMessage();

  modelIsCreate: boolean;
  modelObj: cLogoPartnerItem;
  FileToUpload: File;

  FilesURL: string = Constants.FilesURL;

  page: Page<cLogoPartnerItem> = new Page();
  lstLogoPartner: cLogoPartnerList;
  LogoPartnerSelectFileError: string;

  constructor(private BLService: ServiceLogoPartner,
    private paginationService: CustomPaginationService,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser, BLTranslate: TranslateService,
    private modalService: NgbModal) { super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate); }

  ngOnInit() {
    this.loadData();
  }
  loadData() {

    this.BLService.getLogoPartnerAll(this.page.pageable.pageCurrent, "").subscribe({
      next: lst => {
        this.lstLogoPartner = lst;
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
  createBtn(content): void {
    this.modelIsCreate = true;
    this.FileToUpload = null;
    this.modelObj = new cLogoPartnerItem();
    this.modalService.open(content, { backdropClass: 'light-blue-backdrop', centered: true });
  }
  openBackDropCustomClass(content, obj: cLogoPartnerItem) {
    this.modelIsCreate = false;
    this.FileToUpload = null;
    this.modelObj = obj;
    this.modalService.open(content, { backdropClass: 'light-blue-backdrop', centered: true });
  }
  modelSaveBtn(modelForm: NgForm): void {
    if (!modelForm.valid)
      return;
    if (this.FileToUpload == null) {
      return;
    }

    if (this.FileToUpload.type != "image/png"
      && this.FileToUpload.type != "image/jpg"
      && this.FileToUpload.type != "image/jpeg") {
      this.message.Error(this.LogoPartnerSelectFileError);
      this.BLServiceShowMessage.sendMessage(this.message);
      return;
    }
    if (this.modelIsCreate) {
      this.BLService.createLogoPartner(this.modelObj.WebsiteURL, this.FileToUpload).subscribe({
        next: fileUploadResponse => {
          this.message.Success(this.msgSavedSuccessfully);
          this.BLServiceShowMessage.sendMessage(this.message);
          this.loadData();
          this.modalService.dismissAll();
        },
        error: err => this.message.Error(err)
      });
    } else {
      this.BLService.updateLogoPartner(this.modelObj._id, this.modelObj.WebsiteURL, this.FileToUpload).subscribe({
        next: fileUploadResponse => {
          this.message.Success(this.msgSavedSuccessfully);
          this.BLServiceShowMessage.sendMessage(this.message);
          this.loadData();
          this.modalService.dismissAll();
        },
        error: err => this.message.Error(err)
      });
    }
  }
  public OnFileChanged(files) {
    this.FileToUpload = <File>files[0];
  }

  setActivate(Id: string): void {
    if (!confirm(this.msgsetActivate))
      return;

    this.BLService.setLogoPartnerActivate(Id).subscribe({
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

    this.BLService.setLogoPartnerDeactivate(Id).subscribe({
      next: response => {
        this.message.Success(this.msgDeactivatedSuccessfully);
        this.BLServiceShowMessage.sendMessage(this.message);
        this.loadData();
      },
      error: err => this.message.Error(err)
    });
  }
}
