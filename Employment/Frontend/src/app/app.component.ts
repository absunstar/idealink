import { Component, OnInit } from '@angular/core';
import { AuthService } from './core/auth-service.component';
import { ServiceShowMessage } from './services/show-message.service';
import { EnumShowMessage } from './interface/Model/ModelShowMessage.class';
import { ServiceLoginUser } from './services/loginuser.service';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router, NavigationEnd } from '@angular/router';
import { filter, map, mergeMap } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { Constants } from './constants';
import { ServiceJob } from './services/job.service';
import { ServiceCompany } from './services/service-company.service';
import { interval } from 'rxjs';
import { MiscService } from './services/misc.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: []
})
export class AppComponent implements OnInit {

  msgClass: string = '';
  msgText: string = '';

  userName: string;
  userId: string;
  userRole: string;
  isLoggedIn = false;
  isAdmin: boolean = false;
  isJobSeeker: boolean = false;
  isEmployer: boolean = false;
  isCurrentPageHome: boolean = true;
  IsRoleUndefined: boolean = false;
  currentYear = (new Date()).getFullYear();
  selectLang: string;
  IsRTL: boolean;
  IsAzureLoginEnabled: boolean;
  MCTWebsiteURL: string;
  IsAppear:boolean = true;
  CountApproval:number = 0;
  MCTURL:string;
  isLoggedInMYadd:boolean=false
  constructor(private _authService: AuthService,
    private BLServiceMisc:MiscService,
    private BLServiceShowMessage: ServiceShowMessage,
    private BLServiceLoginUser: ServiceLoginUser,
    private BLServiceJob:ServiceJob,
    private BLServiceCompany:ServiceCompany,
    private _snackBar: MatSnackBar,
    public translate: TranslateService,
    private router: Router, private route: ActivatedRoute) {

    this._authService.CheckLogin();

    translate.addLangs(['en', 'ar']);
    this.selectLang = localStorage.getItem('lang');
    this.selectLang = this.selectLang ? this.selectLang : "en";
    translate.setDefaultLang(this.selectLang );

    this.IsRTL = this.selectLang == 'ar';

    this.IsAzureLoginEnabled = Constants.IsAzureLoginEnabled;
    this.MCTWebsiteURL = Constants.MCTWebsiteURL;

    this.GetApprovalCounts();
    interval(60000).subscribe(x => { //60 Seconds. 1 Min
      this.CountApproval = 0;
      this.GetApprovalCounts();
    });

    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd),
      map(() => this.route),
      map(route => {
        while (route.firstChild) route = route.firstChild
        return route
      }),
      filter(route => route.outlet === 'primary'),
      mergeMap(route => route.data)
    ).subscribe(data => {
      this.isCurrentPageHome = data.PageName === 'home';
    })

    this.BLServiceShowMessage.msgChanged.subscribe(msg => {
      this.msgText = msg.message;
      this.msgClass = msg.type == EnumShowMessage.Success ? 'snackbar-success' : 'snackbar-error';
      this._snackBar.open(this.msgText, null, {
        duration: 5000,
        panelClass: [this.msgClass]
      });
    });

    this.BLServiceLoginUser.UserIsLoggedInChanged.subscribe(obj => {
      this.isLoggedIn = obj;
    });
    this.BLServiceLoginUser.UserIdChanged.subscribe(obj => {
      this.userId = obj;
    });
    this.BLServiceLoginUser.UserRoleChanged.subscribe(obj => {
      this.userRole = obj;
      this.IsRoleUndefined = this.userRole?.toLowerCase() == "undefined";
      this.ForceUserSelectRole();
    });
    this.BLServiceLoginUser.UserNameChanged.subscribe(obj => {
      this.userName = obj;
    });
    this.BLServiceLoginUser.UserIsAdminChanged.subscribe(obj => {
      this.isAdmin = obj;
      if(this.CountApproval == 0 && this.isAdmin)
        this.GetApprovalCounts();
    });
    this.BLServiceLoginUser.UserIsEmployerChanged.subscribe(obj => {
      this.isEmployer = obj;
    });
    this.BLServiceLoginUser.UserIsJobSeekerChanged.subscribe(obj => {
      this.isJobSeeker = obj;
    });
    this.BLServiceMisc.MCTURL().subscribe({
      next: lst => {
        this.MCTURL = lst;
      }
    });
  }

  async ngOnInit() {
    var GetIsLoggedInFunction = () => {
      var self = this;
        return new Promise((resolve, reject) => {
            // self.BLServiceLoginUser.UserIsLoggedInChanged.subscribe(obj => {
            //   resolve(obj);
            // });  
            setTimeout(() => 
            {
                this.IsAppear = false;
            },
            2000);
          //resolve(self.BLServiceLoginUser.isLoggedIn);  
        });
     
    }
    var Islogged = await GetIsLoggedInFunction();
    //  this.isLoggedIn = <any>Islogged;
    
    this._authService.isLoggedIn().then(loggedIn => {
      this.isLoggedIn = loggedIn;
    });
    this.IsRoleUndefined = this.userRole?.toLowerCase() == "undefined";
    this.ForceUserSelectRole();

   
  }

  ngAfterContentInit()	
  {
    setTimeout(() => {
      this.isLoggedInMYadd = true;
    }, 7000);
  }
  login() {
    this._authService.login();
  }

  logout() {
    this._authService.logout();
  }

  ForceUserSelectRole() {
    this.IsRoleUndefined = this.userRole?.toLowerCase() == "undefined";
    if (this.IsRoleUndefined && !this.isCurrentPageHome)
      this.router.navigate(['/']);
  }
  //  isAdmin() {
  //    //return this.isLoggedIn && this._authService.authContext && this._authService.authContext.isAdmin;
  //    this.isAdmin
  //  }
  switchLang() {

    this.translate.use(this.selectLang);

    localStorage.setItem('lang', this.selectLang);

    window.location.reload();
  }
  GetApprovalCounts(){
    if(!this.isAdmin)
      return;

    this.BLServiceJob.GetJobWaitingApprovalCount().subscribe({
      next: count => {
       this.CountApproval += count; 
      }
    });
    this.BLServiceCompany.GetCompanyWaitingApprovalCount().subscribe({
      next: count => {
       this.CountApproval += count; 
      }
    });
  }
}
