import { Component, OnInit } from '@angular/core';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ServiceFavourite } from 'src/app/services/favourite.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { cFavouriteItem } from 'src/app/interface/Response/Favourite.class';
import { Router } from '@angular/router';
import { ConfirmationDialogService } from 'src/app/common/confirmation-dialog/confirmation-dialog.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-empoyer-short-list-resumes',
  templateUrl: './empoyer-short-list-resumes.component.html',
  styleUrls: ['./empoyer-short-list-resumes.component.css']
})
export class EmpoyerShortListResumesComponent extends baseComponent implements OnInit {

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

  private loadData(): void {
    this.BLService.ListAll().subscribe({
      next: lst => {
        this.lstResult = lst;
      },
      error: err => this.message.Error(err)
    });
  }
  setDeActivate(Id: string): void {
    this.confirmationDialogService.confirm("Are you sure you want to Remove from the shortlist?")
      .then((confirmed) => {
        if (!confirmed)
          return;
        this.BLService.setDeActivate(Id).subscribe({
          next: response => {
            this.message.Success("Removed Successfully.");
            this.BLServiceShowMessage.sendMessage(this.message);
            this.loadData();
          },
          error: err => this.message.Error(err)
        });
      });
  }
  onView(Id: string) {
    this.router.navigate(['JobSeeker/Resume/' + Id]);
  }
}
