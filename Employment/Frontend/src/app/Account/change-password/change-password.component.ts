import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ConfirmationDialogService } from 'src/app/common/confirmation-dialog/confirmation-dialog.service';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ModelChangePassword } from 'src/app/interface/Model/ModelChangePassword.class';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { ServiceUserProfile } from 'src/app/services/userprofile.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent extends baseComponent implements OnInit {
  
  modelObj: ModelChangePassword = new ModelChangePassword();

  constructor(private BLServiceUserProfile: ServiceUserProfile,
    private confirmationDialogService: ConfirmationDialogService,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser, BLTranslate: TranslateService,) { 
      super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
     }

  ngOnInit(): void {
  }
  Save(modelForm) {

    if (!modelForm.valid)
      return;
    
    if(this.CheckConfirm())
      return;

    this.BLServiceUserProfile.ChangePassword(this.modelObj).subscribe({
      next: response => {
        this.message.Success("Saved successfully.");
        this.BLServiceShowMessage.sendMessage(this.message);

      },
      error: err => this.message.Error(err)
    });

  }
  CheckConfirm():boolean{
    return  this.modelObj.NewPassword  && this.modelObj.ConfirmPassword  &&
    this.modelObj.NewPassword !== this.modelObj.ConfirmPassword
  }
}
