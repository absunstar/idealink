import { Component, OnInit } from '@angular/core';
import { ServiceShowMessage } from '../services/show-message.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ShowMessage } from '../interface/Model/ModelShowMessage.class';
import { NgForm } from '@angular/forms';
import { ModelUserProfile } from '../interface/Model/ModelUserProfile.class';
import { cUserProfileItem } from '../interface/Response/UserProfile.class';
import { ServiceUserProfile } from '../services/userprofile.service';
import { UserType } from '../Enum/UserType.enum';
import { baseComponent } from '../interface/baseComponent.class';
import { ServiceLoginUser } from '../services/loginuser.service';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from '../core/auth-service.component';
import { config } from 'process';
import { Constants } from '../constants';

@Component({
  selector: 'app-home',
  templateUrl: 'home.component.html'
})

export class HomeComponent extends baseComponent implements OnInit {
  
  passwordPattern = "//^(?=.*[A-Z])(?=.*\\d)(?=.*[$@$!%*#?&])[A-Za-z\\d$@$!%*#?&]{8,20}$//"
  isClicked = false;
  modelObj: ModelUserProfile = new ModelUserProfile();
  msgShow: ShowMessage = new ShowMessage();
  isTermAgreed: boolean = false;
  IsRoleUndefined: boolean = false;
  userRole: string;
  IsAzureLoginEnabled:Boolean;

  constructor(private BLUserProfileService: ServiceUserProfile,
    private _authService: AuthService,
    private router: Router,
    private route: ActivatedRoute,
    private BLServiceMessage: ServiceShowMessage,
    private BLServiceLoggedIn: ServiceLoginUser,BLTranslate: TranslateService,) {
    super(BLServiceMessage, BLServiceLoggedIn,BLTranslate);
    
    this.userRole = this.BLServiceLoggedIn.userRole;
    this.BLServiceLoginUser.UserRoleChanged.subscribe(obj => {
      this.userRole = obj;
      this.IsRoleUndefined = this.userRole.toLowerCase() == "undefined";
    });

    this.IsAzureLoginEnabled = Constants.IsAzureLoginEnabled;
  }

  ngOnInit() {
    const msgRedirect = this.route.snapshot.paramMap.get('msg');
    const msgIsSuccess = this.route.snapshot.paramMap.get('isSuccess');
    if (msgRedirect) {
      var isSuccess = false;
      if (msgIsSuccess)
        isSuccess = msgIsSuccess.toLowerCase() == "true" ? true : false;

      this.msgShow.Send(msgRedirect, isSuccess);
      this.BLServiceMessage.sendMessage(this.msgShow);
      this.IsRoleUndefined = this.userRole.toLowerCase() == "undefined";
    }
    // if(this.IsLoggedIn)
    // {
    //   this.router.navigate(['Employer/Dashboard']);
    // }
  }
  modelSaveBtn(modelForm: NgForm): void {
    if (!modelForm.valid)
      return;
    this.isClicked = true;
    
    this.BLUserProfileService.create(this.modelObj).subscribe({
      next: obj => {
        this.isClicked = false;
        this.router.navigate(['home/RegisterSuccess']);
      },
      error: err => {
        this.message.Error(err)
        this.isClicked = false;
      }
    });
  }
  AssignRole(userType){
    this.BLUserProfileService.UpdateUserRole(userType).subscribe({
      next: obj => {
        this.message.Success("Role updated successfully.");
        this.BLServiceShowMessage.sendMessage(this.message);
        this._authService.logout();
        this.router.navigate(['/']);
      },
      error: err => {
        this.message.Error(err)
        this.BLServiceShowMessage.sendMessage(this.message);
        this.isClicked = false;
      }
    });
  }
}