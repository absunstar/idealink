import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ServiceCompany } from 'src/app/services/service-company.service';
import { cCompanyList, cCompanyItem } from 'src/app/interface/Response/Company.class';
import { ShowMessage } from 'src/app/interface/Model/ModelShowMessage.class';
import { Page } from 'src/app/common/pagination/page';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { CustomPaginationService } from 'src/app/common/pagination/services/custom-pagination.service';
import { CompanyEmployers } from 'src/app/interface/Response/CompanyEmployer.class';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmationDialogService } from 'src/app/common/confirmation-dialog/confirmation-dialog.service';
import { ServiceUserProfile } from 'src/app/services/userprofile.service';
import { cUserProfileItem } from 'src/app/interface/Response/UserProfile.class';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-employer-my-companies',
  templateUrl: './employer-my-companies.component.html',
  styleUrls: ['./employer-my-companies.component.css']
})
export class EmployerMyCompaniesComponent extends baseComponent implements OnInit {

  pageTitle: string = 'My Companies';
  lstResult: cCompanyList;
  message: ShowMessage = new ShowMessage();
  filtertxt: string = '';
  page: Page<cCompanyItem> = new Page();
  modelCompnayId: string;
  modellstCompanyEmployer: CompanyEmployers[];
  objUser : cUserProfileItem;

  constructor(private router: Router,
    private BLService: ServiceCompany,
    private BLUserProfile: ServiceUserProfile,
    private paginationService: CustomPaginationService,
    private confirmationDialogService: ConfirmationDialogService,
    private modalService: NgbModal,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser,BLTranslate: TranslateService,) {
    super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
  }

  ngOnInit(): void {
    this.loadData();
    this.BLUserProfile.GetMyUser().subscribe({
      next: lst => {
        this.objUser = lst;
      },
      error: err => this.message.Error(err)
    });
  }
  
  private loadData(): void {
    this.BLService.getAll(this.page.pageable.pageCurrent, this.filtertxt).subscribe({
      next: lst => {
        this.lstResult = lst;
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
    this.confirmationDialogService.confirmActivation()
      .then((confirmed) => {
        if (!confirmed)
          return;
        this.BLService.setActivate(Id).subscribe({
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
        this.BLService.setDeActivate(Id).subscribe({
          next: response => {
            this.message.Success("Deactivated successfully.");
            this.BLServiceShowMessage.sendMessage(this.message);
            this.loadData();
          },
          error: err => this.message.Error(err)
        });
      });
  }
  filterbtn() {
    this.loadData();
  }
  createBtn() {
    this.router.navigate(['Employer/Profile']);
  }
  onEdit(Id: string) {
    this.router.navigate(['Employer/Profile/' + Id]);
  }
  onView(Id:string){
    this.router.navigate(['Employer/Company/' + Id]);
  }
  AssignEmployer(content, Id:string): void {
    this.modelCompnayId = Id;
    this.getCompanyEmployers();
    this.modalService.open(content, { backdropClass: 'light-blue-backdrop', centered: true });
  }
  OnSelectEmployer(UserId: string): void {
    this.BLService.AddEmployer(UserId,this.modelCompnayId).subscribe({
      next: response => {
        this.message.Success("Employer added successfully.");
        this.BLServiceShowMessage.sendMessage(this.message);
        this.getCompanyEmployers();
      },
      error: err => this.message.Error(err)
    });
  }
   getCompanyEmployers(){
    this.BLService.ListCompanyEmployers(this.modelCompnayId).subscribe({
      next: lst => {
        this.modellstCompanyEmployer = lst;
      },
      error: err => this.message.Error(err)
    });
  }
   RemoveEmployer(Id:string ){
    this.BLService.RemoveEmployer(Id,this.modelCompnayId).subscribe({
      next: response => {
        this.message.Success("Employer removed successfully.");
        this.BLServiceShowMessage.sendMessage(this.message);
        this.getCompanyEmployers();
      },
      error: err => this.message.Error(err)
    });
  }
}
