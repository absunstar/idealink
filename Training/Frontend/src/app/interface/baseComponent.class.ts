import { ServiceShowMessage } from '../services/show-message.service';
import { ShowMessage } from './Model/ModelShowMessage.class';
import { ServiceLoginUser } from '../services/loginuser.service';
import { Constants } from '../constants';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { TranslateService } from '@ngx-translate/core';
import * as fileSaver from 'file-saver';

export class baseComponent {
    message: ShowMessage = new ShowMessage();
    userRole: string;
    IsAdmin: Boolean;
    IsPartner: Boolean;
    IsSubPartner: Boolean;
    IsTrainer: Boolean;
    IsTrainee: Boolean;
    emailPattern = "^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$";
    todayDate = new Date();
    FilesURL: string = Constants.FilesURL;

    msgGenericError: string;
    msgSavedSuccessfully: string;
    msgDeactivatedSuccessfully: string;
    msgActivatedSuccessfully: string;
    msgsetDeactivate: string;
    msgsetActivate: string;
    msgsetDeleted: string;
    msgDeletedSuccessfully: string;
    msgUpdatedsuccessfully: string;
    msgsetRemove: string;
    CertificatePDFOnly: string;
    CertificateSelectFileError: string;

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

    constructor(
        protected BLServiceShowMessage: ServiceShowMessage,
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
        this.BLTranslate.get("CertificatePDFOnly").subscribe(res => { this.CertificatePDFOnly = res; });
        this.BLTranslate.get("CertificateSelectFileError").subscribe(res => { this.CertificateSelectFileError = res; });
        this.BLTranslate.get("GenericError").subscribe(res => { this.msgGenericError = res; });

        this.userRole = this.BLServiceLoginUser.userRole;
        this.BLServiceLoginUser.UserRoleChanged.subscribe(obj => {
            this.userRole = obj;
        });

        this.IsAdmin = this.BLServiceLoginUser.IsAdmin;
        this.BLServiceLoginUser.UserIsAdminChanged.subscribe(obj => {
            this.IsAdmin = obj;
        });

        this.IsPartner = this.BLServiceLoginUser.IsPartner;
        this.BLServiceLoginUser.UserIsPartnerChanged.subscribe(obj => {
            this.IsPartner = obj;
        });

        this.IsSubPartner = this.BLServiceLoginUser.IsSubPartner;
        this.BLServiceLoginUser.UserIsSubPartnerChanged.subscribe(obj => {
            this.IsSubPartner = obj;
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
    compareDate(date1: Date, date2: Date): number {
        // With Date object we can compare dates them using the >, <, <= or >=.
        // The ==, !=, ===, and !== operators require to use date.getTime(),
        // so we need to create a new instance of Date with 'new Date()'
        let d1 = new Date(new Date(date1).getFullYear(), new Date(date1).getMonth(), new Date(date1).getDate());
        let d2 = new Date(new Date(date2).getFullYear(), new Date(date2).getMonth(), new Date(date2).getDate());

        // Check if the dates are equal
        let same = d1.getTime() === d2.getTime();
        if (same) return 0;

        // Check if the first is greater than second
        if (d1 > d2) return 1;

        // Check if the first is less than second
        if (d1 < d2) return -1;
    }
    compareDateAttendanceTraining(date1: Date, date2: Date): number {
        // With Date object we can compare dates them using the >, <, <= or >=.
        // The ==, !=, ===, and !== operators require to use date.getTime(),
        // so we need to create a new instance of Date with 'new Date()'
        let d1 = new Date(new Date(date1).getFullYear(), new Date(date1).getMonth(), new Date(date1).getDate());
        let d2 = new Date(new Date(date2).getFullYear(), new Date(date2).getMonth(), new Date(date2).getDate(), 12, 0, 0);

        // Check if the dates are equal
        let same = d1.getTime() === d2.getTime();
        if (same) return 0;

        // Check if the first is greater than second
        if (d1 > d2) return 1;

        // Check if the first is less than second
        if (d1 < d2) return -1;
    }
    CompareTwoDates(dateFrom, dateTo) {
        if (dateFrom && dateTo) {

            var d1 = Date.parse(dateTo);
            var d2 = Date.parse(dateFrom);

            if (d1 > d2) {
                return true;
            }
            return false
        }
    }
    downloadFile(data: any) {
        

        window.open(Constants.FilesURL + data);
    }
}