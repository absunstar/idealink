import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/core/auth-service.component';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { TranslateService } from '@ngx-translate/core';
import { ServiceMisc } from 'src/app/services/misc.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Constants } from 'src/app/constants';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  isLoggedIn = false;
  userName: string;
  userRole: string;
  isAdmin: boolean = false;
  isPartner: boolean = false;
  isSubPartner: boolean = false;
  IsTrainee: boolean = false;
  IsTrainer: Boolean;
  
  lang: string = 'ar';
  PathFiles:string = Constants.FilesURL;

  constructor(private _authService: AuthService,
    private router: Router,
    private route: ActivatedRoute,
    public BLServiceMisc: ServiceMisc,
    public translate: TranslateService,
    private BLServiceLoginUser: ServiceLoginUser,
    BLTranslate: TranslateService,) {
    translate.addLangs(['en', 'ar']);

    if (localStorage.getItem('lang'))
      this.lang = localStorage.getItem('lang');

    translate.setDefaultLang(this.lang ? this.lang : "ar");
    
  }

  ngOnInit() {
    this.userName = this.BLServiceLoginUser.userName;
    this.BLServiceLoginUser.UserNameChanged.subscribe(obj => {
      this.userName = obj;
    });

    this.isLoggedIn = this.BLServiceLoginUser.isLoggedIn;
    this.BLServiceLoginUser.UserIsLoggedInChanged.subscribe(obj => {
      this.isLoggedIn = obj;
    });

    this.userRole = this.BLServiceLoginUser.userRole;
    this.BLServiceLoginUser.UserRoleChanged.subscribe(obj => {
      this.userRole = obj;
    });

    this.isAdmin = this.BLServiceLoginUser.IsAdmin;
    this.BLServiceLoginUser.UserIsAdminChanged.subscribe(obj => {
      this.isAdmin = obj;
    });

    this.isPartner = this.BLServiceLoginUser.IsPartner;
    this.BLServiceLoginUser.UserIsPartnerChanged.subscribe(obj => {
      this.isPartner = obj;
    });
    this.isSubPartner = this.BLServiceLoginUser.IsSubPartner;
    this.BLServiceLoginUser.UserIsSubPartnerChanged.subscribe(obj => {
      this.isSubPartner = obj;
    });

    this.IsTrainer = this.BLServiceLoginUser.IsTrainer;
    this.BLServiceLoginUser.UserIsTrainerChanged.subscribe(obj => {
      this.IsTrainer = obj;
    });

    this.IsTrainee = this.BLServiceLoginUser.IsTrainee;
    this.BLServiceLoginUser.UserIsTraineeChanged.subscribe(obj => {
      this.IsTrainee = obj;
    });
  }
  login() {
    this._authService.login();
  }

  logout() {
    this._authService.logout();
  }
  switchLang(lang: string) {
    this.translate.use(lang);

    localStorage.setItem('lang', lang);
    localStorage.setItem('URL', this.router.url);
    window.location.reload();
  }
}
