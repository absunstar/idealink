import { AfterViewChecked, Component, OnInit } from '@angular/core';
import { AuthService } from './core/auth-service.component';
import { ServiceShowMessage } from './services/show-message.service';
import { EnumShowMessage } from './interface/Model/ModelShowMessage.class';
import { ServiceLoginUser } from './services/loginuser.service';
import { MatSnackBar } from '@angular/material';
import { filter, map, mergeMap } from 'rxjs/operators';
import { ActivatedRoute, Router, NavigationEnd } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { TryCatchStmt } from '@angular/compiler';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: []
})
export class AppComponent implements OnInit {

  msgClass: string = '';
  msgText: string = '';

  userName: string;
  userRole: string;
  isLoggedIn = false;
  isAdmin: boolean = false;
  isPartner: boolean = false;
  isCurrentPageHome: boolean = false;
  IsAppear:boolean = true;
  isLoggedInMYadd:boolean=false

  lang: string = 'ar';
  cssExt: string = '';

  constructor(private _authService: AuthService,
    private BLServiceShowMessage: ServiceShowMessage,
    private BLServiceLoginUser: ServiceLoginUser,
    BLTranslate: TranslateService,
    private _snackBar: MatSnackBar,
    private router: Router, private route: ActivatedRoute) {

    var url = localStorage.getItem('URL');
    if (url && url != null && url != "null") {
      localStorage.setItem('URL', null);
      this.router.navigate([this.router.url]);
    }

    this._authService.CheckLogin();
    this.lang = localStorage.getItem('lang') ? localStorage.getItem('lang') : "ar";
    if (this.lang == 'ar')
      this.cssExt = 'ar'
    else
      this.cssExt = '';

    this.BLServiceShowMessage.msgChanged.subscribe(msg => {
      this.msgText = msg.message;
      this.msgClass = msg.type == EnumShowMessage.Success ? 'snackbar-success' : 'snackbar-error';
      this._snackBar.open(this.msgText, null, {
        duration: 3000,
        panelClass: [this.msgClass]
      });
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

     this.InitData();  
    //  this._authService.loginChanged.subscribe(loggedIn => {
    //    this.isLoggedIn = loggedIn;
    //  });
    //  this._authService.authContextChanged.subscribe(obj => {
    //   this.AuthContext = obj;
    // });
    this.BLServiceLoginUser.UserRoleChanged.subscribe(obj => {
      this.userRole = obj;
    });
    this.BLServiceLoginUser.UserNameChanged.subscribe(obj => {
      this.userName = obj;
    });
    this.BLServiceLoginUser.UserIsAdminChanged.subscribe(obj => {
      this.isAdmin = obj;
    });
    this.BLServiceLoginUser.UserIsPartnerChanged.subscribe(obj => {
      this.isPartner = obj;
    });
  }

  async InitData() {
    var GetIsLoggedInFunction = () => {
      var self = this;
        return new Promise((resolve, reject) => {
            self.BLServiceLoginUser.UserIsLoggedInChanged.subscribe(obj => {
              resolve(obj);
            });  
            setTimeout(() => 
            {
                this.IsAppear = false;
            },
            2000);
          //resolve(self.BLServiceLoginUser.isLoggedIn);  
        });
     
    }
    var GetIsHomeFunction = () => {
      var self = this;
      return new Promise((resolve) => {
        self.router.events.pipe(
          filter(event => event instanceof NavigationEnd),
          map(() => this.route),
          map(route => {
            while (route.firstChild) route = route.firstChild
            return route
          }),
          filter(route => route.outlet === 'primary'),
          mergeMap(route => route.data)
        ).subscribe(data => {
          resolve(data.PageName);
        });

      });
    }



    var Islogged = await GetIsLoggedInFunction();
   // var HomeVal = await GetIsHomeFunction();
    //console.log(HomeVal);
    this.isLoggedIn = <any>Islogged;
    //this.isCurrentPageHome = HomeVal === 'home';
  }
  ngAfterContentInit()	
  {
    setTimeout(() => {
      this.isLoggedInMYadd = true;
    }, 7000);
  }
  ngOnInit() {
    this.BLServiceLoginUser.UserIsLoggedInChanged.subscribe(obj => {
      this.isLoggedIn = obj;
    });
  }
  
  //  isAdmin() {
  //    //return this.isLoggedIn && this._authService.authContext && this._authService.authContext.isAdmin;
  //    this.isAdmin
  //  }

}
