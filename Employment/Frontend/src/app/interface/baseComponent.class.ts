import { ServiceShowMessage } from '../services/show-message.service';
import { ShowMessage } from './Model/ModelShowMessage.class';
import { ServiceLoginUser } from '../services/loginuser.service';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';

export class baseComponent {
    emailPattern = "^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$";
    message: ShowMessage = new ShowMessage();
    userRole: string;
    IsAdmin: Boolean;
    isJobSeeker: boolean = false;
    isEmployer: boolean = false;
    IsLoggedIn: Boolean;
    userId: string;
    dateToday: NgbDateStruct;
   

    msgSavedSuccessfully: string;
    msgDeactivatedSuccessfully: string;
    msgActivatedSuccessfully: string;
    msgsetDeactivate: string;
    msgsetActivate: string;
    msgsetDeleted: string;
    msgDeletedSuccessfully: string;
    msgUpdatedsuccessfully:string;
    msgsetRemove:string;

    textEditorConfig: AngularEditorConfig = {
        editable: true,
        spellcheck: true,
        minHeight: '5rem',
        translate: 'yes',
        defaultParagraphSeparator: 'p',
        defaultFontName: 'Arial',
        toolbarHiddenButtons: [
            ['insertImage',
                'insertVideo',
                'toggleEditorMode',
                'link',
                'unlink',
                'backgroundColor',
                'justifyLeft',
                'justifyCenter',
                'justifyRight',
                'justifyFull'
            ]
        ]
    };

    constructor(protected BLServiceShowMessage: ServiceShowMessage,
        protected BLServiceLoginUser: ServiceLoginUser,
        protected BLTranslate: TranslateService) {
        this.Init();
    }
    Init() {
        this.BLTranslate.get("msgSavedSuccessfully").subscribe(res => { this.msgSavedSuccessfully = res; });
        this.BLTranslate.get("msgDeactivatedSuccessfully").subscribe(res => { this.msgDeactivatedSuccessfully = res; });
        this.BLTranslate.get("msgActivatedSuccessfully").subscribe(res => { this.msgActivatedSuccessfully = res; });
        this.BLTranslate.get("msgsetDeactivate").subscribe(res => { this.msgsetDeactivate = res; });
        this.BLTranslate.get("msgsetActivate").subscribe(res => { this.msgsetActivate = res; });
        this.BLTranslate.get("msgDeletedSuccessfully").subscribe(res => { this.msgDeletedSuccessfully = res; });
        this.BLTranslate.get("msgsetDeleted").subscribe(res => { this.msgsetDeleted = res; });
        this.BLTranslate.get("msgUpdatedsuccessfully").subscribe(res => { this.msgUpdatedsuccessfully = res; });
        this.BLTranslate.get("msgsetRemove").subscribe(res => { this.msgsetRemove = res; });
       
        var d = new Date();
        this.dateToday = {
            year: d.getUTCFullYear(), month: d.getUTCMonth() + 1
            , day: d.getUTCDate() 
        };
        
        this.IsLoggedIn = this.BLServiceLoginUser.isLoggedIn;
        this.BLServiceLoginUser.UserIsLoggedInChanged.subscribe(obj => {
            this.IsLoggedIn = obj;
        });

        this.userId = this.BLServiceLoginUser.userId;
        this.BLServiceLoginUser.UserIdChanged.subscribe(obj => {
            this.userId = obj;
        });
        this.userRole = this.BLServiceLoginUser.userRole;
        this.BLServiceLoginUser.UserRoleChanged.subscribe(obj => {
            this.userRole = obj;
        });

        this.IsAdmin = this.BLServiceLoginUser.IsAdmin;
        this.BLServiceLoginUser.UserIsAdminChanged.subscribe(obj => {
            this.IsAdmin = obj;
        });

        this.isEmployer = this.BLServiceLoginUser.isEmployer;
        this.BLServiceLoginUser.UserIsEmployerChanged.subscribe(obj => {
            this.isEmployer = obj;
        });

        this.isJobSeeker = this.BLServiceLoginUser.isJobSeeker;
        this.BLServiceLoginUser.UserIsJobSeekerChanged.subscribe(obj => {
            this.isJobSeeker = obj;
        });
    }
    CheckDateBiggerThanToday(date: Date) {
        var today = new Date();
        return date > today;
    }
    CompareDates(dateFrom,dateTo) {
        if (dateFrom && dateTo) {

            var d1 = Date.parse(dateTo);
            var d2 = Date.parse(dateFrom);

            if (d1 > d2) {
                return true;
            }
            return false
        }
    }
    CompareDatesEqual(dateFrom,dateTo) {
        if (dateFrom && dateTo) {

            var d1 = Date.parse(dateTo);
            var d2 = Date.parse(dateFrom);

            if (d1 >= d2) {
                return true;
            }
            return false
        }
    }
}