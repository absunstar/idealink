import { Component, OnInit } from '@angular/core';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { cJobItem } from 'src/app/interface/Response/Job.class';
import { ServiceJob } from 'src/app/services/job.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { ActivatedRoute, Router } from '@angular/router';
import { JobType } from 'src/app/Enum/JobType.enum';
import { UserGender } from 'src/app/Enum/UserGender.enum';
import { ServiceFavourite } from 'src/app/services/favourite.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgForm } from '@angular/forms';
import { ServiceApply } from 'src/app/services/apply.service';
import { ConfirmationDialogService } from 'src/app/common/confirmation-dialog/confirmation-dialog.service';
import { TranslateService } from '@ngx-translate/core';
import { Location } from '@angular/common';
import { takeWhile } from 'rxjs/operators';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-job-view',
  templateUrl: './job-view.component.html',
  styleUrls: ['./job-view.component.css']
})
export class JobViewComponent extends baseComponent implements OnInit {

  objJob: cJobItem;
  JobId: string;
  modelMessage: string;
  EmployerJobViewNoDesc: string;
  EmployerJobViewNoSkill: string;
  EmployerJobViewNoBenefit: string;
  IsJobApply: boolean = false;
  IsJobFavourite: boolean = false;

  constructor(
    private BLJob: ServiceJob,
    private confirmationDialogService: ConfirmationDialogService,
    private modalService: NgbModal,
    private BLApply: ServiceApply,
    private router: Router,
    BLServiceShowMessage: ServiceShowMessage,
    BLJobLoginUser: ServiceLoginUser, BLTranslate: TranslateService,
    private BLFavourite: ServiceFavourite,
    private route: ActivatedRoute,
    private cookies :CookieService,
    private location : Location) {
    super(BLServiceShowMessage, BLJobLoginUser, BLTranslate)
  }
  loadData() {
    if (this.IsLoggedIn) {
      this.BLFavourite.CheckMyFavourite(this.JobId).subscribe({
        next: obj => {
          this.IsJobFavourite = obj;
        },
        error: err => {
          this.message.Error(err);
          this.BLServiceShowMessage.sendMessage(this.message);
        }
      });
      this.BLApply.CheckMyApply(this.JobId).subscribe({
        next: obj => {
          this.IsJobApply = obj;
        },
        error: err => {
          this.message.Error(err);
          this.BLServiceShowMessage.sendMessage(this.message);
        }
      });
    }
  }
  GetData(param) {
    this.BLJob.getGetByid(param).subscribe({
      next: obj => {
        this.objJob = obj;
        this.loadData();
      },
      error: err => {
        this.message.Error(err);
        this.BLServiceShowMessage.sendMessage(this.message);
      }
    });
  }
  ngOnInit(): void {
    const param = this.route.snapshot.paramMap.get('Id');
   this.cookies.set('privT',JSON.stringify(true))

    if (param) {
      this.JobId = param;

      // if (this.IsLoggedIn) {
      this.GetData(param);
      //}
      // else {
      //   this.BLServiceLoginUser.UserIsLoggedInChanged.pipe(takeWhile(val => true)).subscribe(obj => {
      //     if (obj) {
      //       this.GetData(param);

      //     }
      //   });
      // }
    }
    this.BLTranslate.get("EmployerJobViewNoDesc").subscribe(res => { this.EmployerJobViewNoDesc = res; });
    this.BLTranslate.get("EmployerJobViewNoSkill").subscribe(res => { this.EmployerJobViewNoSkill = res; });
    this.BLTranslate.get("EmployerJobViewNoBenefit").subscribe(res => { this.EmployerJobViewNoBenefit = res; });
  }
  GetJobType(type) {
    return JobType[type];
  }
  GetGender(gender) {
    return UserGender[gender];
  }
  onRemoveFavourite(): void {
    this.confirmationDialogService.confirm("Are you sure you want to Remove from the shortlist?")
      .then((confirmed) => {
        if (!confirmed)
          return;
        this.BLFavourite.DeActivateByJobId(this.JobId).subscribe({
          next: response => {
            this.message.Success("Removed Successfully.");
            this.BLServiceShowMessage.sendMessage(this.message);
            this.loadData();
          },
          error: err => this.message.Error(err)
        });
      });
  }

  onAddFavourite() {

    this.BLFavourite.Create(this.JobId).subscribe({
      next: obj => {
        this.message.Success("Saved successfully.");
        this.BLServiceShowMessage.sendMessage(this.message);
        this.loadData();
      },
      error: err => {
        this.message.Error(err);
        this.BLServiceShowMessage.sendMessage(this.message);
      }
    });
  }
  createBtn(content): void {
    this.modalService.open(content, { size: 'lg', backdropClass: 'light-blue-backdrop', centered: true });
  }
  modelSaveBtn(modelForm: NgForm): void {
    if (!modelForm.valid)
      return;

    this.BLApply.Create(this.JobId, this.modelMessage).subscribe({
      next: response => {
        this.message.Success("Saved successfully.");
        this.BLServiceShowMessage.sendMessage(this.message);
        this.modalService.dismissAll();
        this.loadData();
      },
      error: err => this.message.Error(err)
    });
  }
  Approve(Id: string): void {
    this.confirmationDialogService.confirm("Are you sure you want to Approve?")
      .then((confirmed) => {
        if (!confirmed)
          return;

        this.BLJob.setApproved(Id).subscribe({
          next: response => {
            // if (response) {
            this.message.Success("Approved successfully.");
            this.BLServiceShowMessage.sendMessage(this.message);
            this.router.navigate(['/Admin/JobApproval']);
            // }
          },
          error: err => this.message.Error(err)
        });
      });
  }
  Reject(Id: string): void {
    this.confirmationDialogService.confirm("Are you sure you want to Reject?")
      .then((confirmed) => {
        if (!confirmed)
          return;

        this.BLJob.setRejected(Id).subscribe({
          next: response => {
            this.message.Success("Reject successfully.");
            this.BLServiceShowMessage.sendMessage(this.message);
            this.router.navigate(['/Admin/JobApproval']);
          },
          error: err => this.message.Error(err)
        });
      });
  }


  goback() {
    this.location.back();
  }
}
