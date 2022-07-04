import { Component, OnInit } from '@angular/core';
import { cTraineeItem, cTraineeList } from 'src/app/interface/Response/Trainee.class';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { Page } from 'src/app/common/pagination/page';
import { CustomPaginationService } from 'src/app/common/pagination/services/custom-pagination.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { NgForm } from '@angular/forms';
import { ModelTrainee } from 'src/app/interface/Model/ModelTrainee.class';
import { ServiceTrainee } from 'src/app/services/trainee.service';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ServiceUserProfile } from 'src/app/services/userprofile.service';
import { ModelChangeEmail } from 'src/app/interface/Model/ModelChangeEmail.class';

@Component({
  selector: 'app-trainee',
  templateUrl: './trainee.component.html',
  styleUrls: ['./trainee.component.css']
})
export class TraineeComponent extends baseComponent implements OnInit {


  pageTitle: string = 'Trainee List';
  lstTrainee: cTraineeList;
  filterType: string = "0";
  filtertxt: string = '';
  modelChangeEmail: ModelChangeEmail = new ModelChangeEmail();


  page: Page<cTraineeItem> = new Page();
  AccountActivationLinkFailed:string;
  AccountActivationLinkSuccess:string;
  AccountPasswordSucess:string;
  AccountPasswordFailed:string;

  constructor(private BLServiceTrainee: ServiceTrainee,
    private BLServiceUserProfile: ServiceUserProfile,
    private paginationService: CustomPaginationService,
    private router: Router,
    private modalService: NgbModal,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser,BLTranslate: TranslateService,) {
    super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
    this.BLTranslate.get("AccountActivationLinkFailed").subscribe(res => { this.AccountActivationLinkFailed = res; });
    this.BLTranslate.get("AccountActivationLinkSuccess").subscribe(res => { this.AccountActivationLinkSuccess = res; });
    this.BLTranslate.get("AccountPasswordSucess").subscribe(res => { this.AccountPasswordSucess = res; });
    this.BLTranslate.get("AccountPasswordFailed").subscribe(res => { this.AccountPasswordFailed = res; });
  }


  ngOnInit() {
    this.loadData();
  }
  public loadData(): void {
    this.BLServiceTrainee.getAll(this.page.pageable.pageCurrent, this.filtertxt).subscribe({
      next: lst => {

        this.lstTrainee = lst;
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
    this.BLServiceTrainee.setActivate(Id).subscribe({
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

    this.BLServiceTrainee.setDeactivate(Id).subscribe({
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
  createBtn() {
    // path: 'Trainee/Create/:IsCreate/:TraineeId/:TrainingId'
    this.router.navigate(['/Trainee/Create/1/0/0']);
  }
  Edit(TraineeId) {
    // path: 'Trainee/Create/:IsCreate/:TraineeId/:TrainingId'
    this.router.navigate(['/Trainee/Create/0/' + TraineeId + '/0']);
  }
  Certificate(TraineeId) {
    // path: 'Trainee/Create/:IsCreate/:TraineeId/:TrainingId'
    this.router.navigate(['/MyTraining/' + TraineeId ]);
  }
  ResendActivationLink(Email: string){
    this.BLServiceTrainee.ResendActivationLink(Email).subscribe({
      next: response => {
        this.message.Success(this.AccountActivationLinkSuccess);
        this.BLServiceShowMessage.sendMessage(this.message);
        this.loadData();
      },
      error: err => {
        this.message.Error(this.AccountActivationLinkFailed);
        this.BLServiceShowMessage.sendMessage(this.message);
    }
    });
  }
  ResendPasswordLink(Email: string) {
    this.BLServiceTrainee.ResendPasswordLink(Email).subscribe({
      next: response => {
        this.message.Success(this.AccountPasswordSucess);
        this.BLServiceShowMessage.sendMessage(this.message);
        //this.loadData();
      },
      error: err => {
        this.message.Error(this.AccountPasswordFailed);
        this.BLServiceShowMessage.sendMessage(this.message);
      }
    });
  }
  ChangeEmail(content, emailOld: string) {
    this.modelChangeEmail = new ModelChangeEmail();
    this.modelChangeEmail.EmailOld = emailOld;

    this.modalService.open(content, { backdropClass: 'light-blue-backdrop', centered: true });
  }
  ChangeEmailSave(modelForm: NgForm): void {
    if (!modelForm.valid)
      return;

    this.BLServiceUserProfile.UpdateUserEmail(this.modelChangeEmail).subscribe({
      next: lst => {
        this.loadData();
        this.message.Success(this.msgSavedSuccessfully);
        this.BLServiceShowMessage.sendMessage(this.message);
        this.modalService.dismissAll();
      },
      error: err => this.message.Error(err)
    });
  }
}
