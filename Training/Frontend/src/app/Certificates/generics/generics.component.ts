import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { HttpEventType, HttpClient } from '@angular/common/http';
import { ServiceCertificate } from 'src/app/services/certificate.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { ShowMessage } from 'src/app/interface/Model/ModelShowMessage.class';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgForm } from '@angular/forms';
import { cEntityPartnerItem } from 'src/app/interface/Response/EntityPartner.class';
import { ServiceEntityManagement } from 'src/app/services/entitymanagement.service';
import { ModelCertificateUpload } from 'src/app/interface/Model/ModelCertificateUpload.class';
import { CertificateType } from 'src/app/Enum/CertificateType.enum';
import { cCertificateItem, cCertificateList } from 'src/app/interface/Response/Certificate.class';
import { Page } from 'src/app/common/pagination/page';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { CustomPaginationService } from 'src/app/common/pagination/services/custom-pagination.service';
import { ConfigForm } from 'src/app/Enum/ConfigForm.enum';
import { Constants } from 'src/app/constants';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-generics',
  templateUrl: './generics.component.html',
  styleUrls: ['./generics.component.css']
})
export class GenericsComponent extends baseComponent implements OnInit {
  message: ShowMessage = new ShowMessage();
  public progress: number;
  public modelName;
  public isTraineeChecked: boolean = false;
  public isTrainerChecked: boolean = false;
  public lstPartners: cEntityPartnerItem[];
  public lstPartnersSelected: any;
  public partnerFileToUpload: File;
  FilesURL: string = Constants.FilesURL;

  page: Page<cCertificateItem> = new Page();
  lstCertificate: cCertificateList;
  lstSystemCertificate: cCertificateList;

  @Output() public onUploadFinished = new EventEmitter();

  constructor(private BLServiceCertificate: ServiceCertificate,
    private BLServiceEntityManagement: ServiceEntityManagement,
    private paginationService: CustomPaginationService,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser,BLTranslate: TranslateService,
    private http: HttpClient, private modalService: NgbModal) {
    super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
  }

  ngOnInit() {
    this.BLServiceEntityManagement.getEntityPartnerListActive("").subscribe({
      next: lst => {
        this.lstPartners = lst;
      },
      error: err => this.message.Error(err)
    });
    this.loadData();
  }
  loadData() {
    this.BLServiceCertificate.CertificateListAllGenericByPartnerId(this.page.pageable.pageCurrent, "").subscribe({
      next: lst => {
        this.lstCertificate = lst;
        this.page.pageable.pageSize = lst.pageSize;
        this.page.totalElements = lst.totalCount;
        this.page.content = lst.lstResult;
      },
      error: err => this.message.Error(err)
    });
    this.BLServiceCertificate.CertificateListAllSystemGeneric().subscribe({
      next: lst => {
        
        this.lstSystemCertificate = lst;
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
    this.modelName = "Upload partner certificate";
    this.isTraineeChecked = true;
    this.isTrainerChecked = false;
    this.lstPartnersSelected = "";
    this.modalService.open(content, { backdropClass: 'light-blue-backdrop', centered: true });
  }

  onPartnerFileTypeChange(e, fileType: string) {
    if (e.target.value == "on") {
      if (fileType == "Trainee") {
        this.isTraineeChecked = true;
        this.isTrainerChecked = false;
      }
      else {
        this.isTraineeChecked = false;
        this.isTrainerChecked = true;
      }
    }
  }


  modelSaveBtn(modelForm: NgForm): void {

    if (!modelForm.valid)
      return;

    if (!this.partnerFileToUpload) {
      this.message.Error(this.CertificateSelectFileError);
      this.BLServiceShowMessage.sendMessage(this.message);
      return;
    }

    var fileType = 0;
    if (this.isTraineeChecked)
      fileType = 2;
    else
      fileType = 1;

    this.BLServiceCertificate.uploadFile(this.partnerFileToUpload, fileType, this.lstPartnersSelected).subscribe({
      next: fileUploadResponse => {
        this.loadData();
        this.message.Success(this.msgSavedSuccessfully);
        this.BLServiceShowMessage.sendMessage(this.message);
        this.modalService.dismissAll();
      },

      error: err => this.message.Error(err)
    });
  }

  public OnParentFileChanged(files) {
    this.partnerFileToUpload = <File>files[0];
  }

  public uploadFile = (files, fileType,event) => {

    console.log(event);
    this.OnParentFileChanged(files);
    if (files.length === 0) {
      return;
    }

    if (!this.partnerFileToUpload) {
      this.message.Error(this.CertificateSelectFileError);
      this.BLServiceShowMessage.sendMessage(this.message);
      return;
    }

    let fileToUpload = <File>files[0];
    console.log(fileToUpload);
    
    if (fileToUpload.type != "application/pdf") {
      this.message.Error(this.CertificatePDFOnly);
      this.BLServiceShowMessage.sendMessage(this.message);
    } else {
      this.BLServiceCertificate.uploadFile(fileToUpload, fileType).subscribe({
        next: fileUploadResponse => {
          this.message.Success(this.msgSavedSuccessfully);
          this.BLServiceShowMessage.sendMessage(this.message);
        },

        error: err => this.message.Error(err)
      });
    }
  }
  GetType(Type) {
    return CertificateType[Type];
  }
  setActivate(Id: string): void {
    if (!confirm(this.msgsetActivate))
      return;

    this.BLServiceCertificate.CertificateActivate(Id).subscribe({
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

    this.BLServiceCertificate.CertificateDeActivate(Id).subscribe({
      next: response => {
        this.message.Success(this.msgDeactivatedSuccessfully);
        this.BLServiceShowMessage.sendMessage(this.message);
        this.loadData();
      },
      error: err => this.message.Error(err)
    });
  }
}
