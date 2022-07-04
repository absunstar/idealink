import { Component, OnInit } from '@angular/core';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ServiceUserProfile } from 'src/app/services/userprofile.service';
import { ShowMessage } from 'src/app/interface/Model/ModelShowMessage.class';
import { Page } from 'src/app/common/pagination/page';
import { cUserProfileTrainerCertificateList, cUserProfileTrainerCertificateItem } from 'src/app/interface/Response/UserProfile.class';
import { CustomPaginationService } from 'src/app/common/pagination/services/custom-pagination.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { ModelTrainerCertificateApproval } from 'src/app/interface/Model/ModelTrainerCertificateApproval.class';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-trainer-approval',
  templateUrl: './trainer-approval.component.html',
  styleUrls: ['./trainer-approval.component.css']
})
export class TrainerApprovalComponent extends baseComponent implements OnInit {
  message: ShowMessage = new ShowMessage();

  page: Page<cUserProfileTrainerCertificateItem> = new Page();
  lstCertificate: cUserProfileTrainerCertificateList;
  msgApprovedSuccessfully:string;
  msgsetApproved:string;

  constructor(private BLServiceUserProfile: ServiceUserProfile,
    private paginationService: CustomPaginationService,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser, BLTranslate: TranslateService,) {
    super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
  }

  ngOnInit() {
    this.BLTranslate.get("msgApprovedSuccessfully").subscribe(res => { this.msgApprovedSuccessfully = res; });
    this.BLTranslate.get("msgsetApproved").subscribe(res => { this.msgsetApproved = res; });
   
    this.loadData();
  }
  loadData() {

    this.BLServiceUserProfile.GetTrainerCertificate(this.page.pageable.pageCurrent, "").subscribe({
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

  Approve(obj: cUserProfileTrainerCertificateItem): void {
    if (!confirm(this.msgsetApproved))
      return;

    var model = new ModelTrainerCertificateApproval();
    model.TrainerId = obj.TrainerId;
    model.PartnerId = obj.PartnerId;
    model.TrainingCategoryId = obj.TrainingCategoryId;

    this.BLServiceUserProfile.CertificateApprove(model).subscribe({
      next: response => {

        this.message.Success(this.msgApprovedSuccessfully);
        this.BLServiceShowMessage.sendMessage(this.message);
        this.loadData();
      },
      error: err => this.message.Error(err)
    });
  }

}
