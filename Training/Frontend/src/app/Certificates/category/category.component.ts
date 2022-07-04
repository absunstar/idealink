import { Component, OnInit } from '@angular/core';
import { ShowMessage } from 'src/app/interface/Model/ModelShowMessage.class';
import { cEntityPartnerItem } from 'src/app/interface/Response/EntityPartner.class';
import { ServiceCertificate } from 'src/app/services/certificate.service';
import { ServiceEntityManagement } from 'src/app/services/entitymanagement.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { HttpClient } from '@angular/common/http';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgForm } from '@angular/forms';
import { cTrainingTypeItem } from 'src/app/interface/Response/TrainingType.class';
import { ITrainingCategoryItem } from 'src/app/interface/Response/TrainingCategory.interface';
import { ServiceDataManagement } from 'src/app/services/datamanagement.service';
import { Constants } from 'src/app/constants';
import { Page } from 'src/app/common/pagination/page';
import { cCertificateItem, cCertificateList } from 'src/app/interface/Response/Certificate.class';
import { CustomPaginationService } from 'src/app/common/pagination/services/custom-pagination.service';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { CertificateType } from 'src/app/Enum/CertificateType.enum';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.css']
})
export class CategoryComponent extends baseComponent implements OnInit {
  message: ShowMessage = new ShowMessage();
  public progress: number;
  public modelName;
  public isTraineeChecked: boolean = true;
  public isTrainerChecked: boolean = false;
  public lstPartners: cEntityPartnerItem[];
  public lstPartnersSelected: any;
  public partnerFileToUpload: File;
  public selectedTrainingTypeId: string;
  public selectedTrainingCategory: string;

  lstTrainingTypes: cTrainingTypeItem[];
  lstTrainingCategory: ITrainingCategoryItem[];
  TrainingTypeId: string;

  FilesURL: string = Constants.FilesURL;

  page: Page<cCertificateItem> = new Page();
  lstCertificate: cCertificateList;


  constructor(private BLServiceCertificate: ServiceCertificate,
    private BLServiceDataManagement: ServiceDataManagement,
    private BLServiceEntityManagement: ServiceEntityManagement,
    private paginationService: CustomPaginationService,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser,BLTranslate: TranslateService,
    private modalService: NgbModal) { super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate); }

  ngOnInit() {
    this.loadData();
    this.BLServiceDataManagement.getTrainingTypeListActive().subscribe({
      next: lst => {
        this.lstTrainingTypes = lst;
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
  loadData() {

    this.BLServiceCertificate.CertificateListAllByPartnerId(this.page.pageable.pageCurrent, "").subscribe({
      next: lst => {
        this.lstCertificate = lst;
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
    this.modelName = "Upload partner certificate for category";
    this.isTraineeChecked = true;
    this.isTrainerChecked = false;
    this.lstPartnersSelected = "";
    this.selectedTrainingTypeId = "";
    this.selectedTrainingCategory = "";
    this.modalService.open(content, { backdropClass: 'light-blue-backdrop', centered: true });
  }

  onTrainingTypeSelect(event) {
    this.selectedTrainingTypeId = event.value;
    this.BLServiceDataManagement.getTrainingCategoryListByTrainingType(this.selectedTrainingTypeId).subscribe({
      next: lst => {
        this.lstTrainingCategory = lst;
      },
      error: err => this.message.Error(err)
    });
  }

  PartnerChange(event) {
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
    var fileType = 2;
    if (this.isTraineeChecked)
      fileType = 2;
    else if (this.isTrainerChecked)
      fileType = 1;
    this.BLServiceCertificate.uploadCategoryCertificateFile(this.partnerFileToUpload, fileType, this.lstPartnersSelected, this.selectedTrainingTypeId, this.selectedTrainingCategory).subscribe({
      next: fileUploadResponse => {
        this.message.Success(this.msgSavedSuccessfully);
        this.BLServiceShowMessage.sendMessage(this.message);
        this.loadData();
        this.modalService.dismissAll();
      },
      error: err => this.message.Error(err)
    });
    /* this.BLServiceUserProfile.create(model).subscribe({
       next: response => {
         this.message.Success(this.msgSavedSuccessfully);
         this.BLServiceShowMessage.sendMessage(this.message);
         this.loadData();
         this.isClicked = false;
         this.modalService.dismissAll();
       },
       error: err => {this.message.Error(err)
         this.isClicked = false;}
     });*/


  }
  /**
   * name
   */
  public OnParentFileChanged(files) {
    this.partnerFileToUpload = <File>files[0];
  }

  public uploadFile = (files, fileType) => {
    if (files.length === 0) {
      return;
    }
    let fileToUpload = <File>files[0];
    if (fileToUpload.type != "application/pdf") {
      this.message.Error(this.CertificateSelectFileError);
      this.BLServiceShowMessage.sendMessage(this.message);
    } else {
      this.BLServiceCertificate.uploadFile(fileToUpload, fileType).subscribe({
        next: fileUploadResponse => {
          this.loadData();
          this.message.Success(this.msgSavedSuccessfully);
          this.BLServiceShowMessage.sendMessage(this.message);
          this.modalService.dismissAll();
        },

        error: err => this.message.Error(err)
      });
    }
  }

  public onTrainingCategorySelect(event) {
    this.selectedTrainingCategory = event.value;
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
