import { Component, OnInit } from '@angular/core';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { cFavouriteItem } from 'src/app/interface/Response/Favourite.class';
import { ServiceFavourite } from 'src/app/services/favourite.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { Router } from '@angular/router';
import { ConfirmationDialogService } from 'src/app/common/confirmation-dialog/confirmation-dialog.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-short-list-jobs',
  templateUrl: './short-list-jobs.component.html',
  styleUrls: ['./short-list-jobs.component.css']
})
export class ShortListJobsComponent extends baseComponent implements OnInit {

  lstResult: cFavouriteItem[];

  constructor(private BLService: ServiceFavourite,
    private confirmationDialogService: ConfirmationDialogService,
    private router: Router,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser,BLTranslate: TranslateService,) {
    super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
  }

  ngOnInit(): void {
    this.loadData();
  }
  onView(Id: string) {
    this.router.navigate(['Employer/Job/' + Id]);
  }

  private loadData(): void {
    this.BLService.ListAll().subscribe({
      next: lst => {
        this.lstResult = lst;
      },
      error: err => this.message.Error(err)
    });
  }
  setDeActivate(Id: string): void {
    this.confirmationDialogService.confirm("Are you sure you want to Remove from the favourite list?")
      .then((confirmed) => {
        if (!confirmed)
          return;
        this.BLService.setDeActivate(Id).subscribe({
          next: response => {
            this.message.Success("Removed successfully.");
            this.BLServiceShowMessage.sendMessage(this.message);
            this.loadData();
          },
          error: err => this.message.Error(err)
        });
      });
  }
}
