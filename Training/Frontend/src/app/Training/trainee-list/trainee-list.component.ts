import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { cTrainingItem } from 'src/app/interface/Response/Training.class';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { ServiceTraining } from 'src/app/services/training.service';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { cTraineeItem, cTraineeItemInfo } from 'src/app/interface/Response/Trainee.class';
import { ServiceTrainee } from 'src/app/services/trainee.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ModelTraineeTraining } from 'src/app/interface/Model/ModelTraineeTraining.class';
import { TranslateService } from '@ngx-translate/core';
import { Constants } from 'src/app/constants';
import * as FileSaver from 'file-saver';
import { constants } from 'buffer';

@Component({
  selector: 'app-trainee-list',
  templateUrl: './trainee-list.component.html',
  styleUrls: ['./trainee-list.component.css']
})
export class TraineeListComponent extends baseComponent implements OnInit {

  @ViewChild('fileInput') fileInput: any;
  pageTitle: string = "Trainee List"
  trainingId: string;
  trainingObj: cTrainingItem;
  traineelst: cTraineeItemInfo[];
  todayDate = new Date();
  todayDateMinus2 = new Date();
  ExcelOnly: string;
  DonwloadURL: string;
  DownloadURLError:string;
  msgTrainingNoCertificates: string;
  IsTraineeUpload:boolean = false;

  private _filtertxt: string = '';
  public get filtertxt(): string {
    return this._filtertxt;
  }
  public set filtertxt(value: string) {
    this._filtertxt = value;
    this.filterbtn();
  }
  constructor(private route: ActivatedRoute,
    private BLServiceTraining: ServiceTraining,
    private modalService: NgbModal,
    private BLServiceTrainee: ServiceTrainee,
    private router: Router,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser, BLTranslate: TranslateService,) {
    super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
    this.BLTranslate.get("ExcelOnly").subscribe(res => { this.ExcelOnly = res; });
    this.BLTranslate.get("TrainingNoCertificates").subscribe(res => { this.msgTrainingNoCertificates = res; });
  }

  ngOnInit(): void {
    this.todayDateMinus2.setDate(this.todayDateMinus2.getDate() - 2);
    this.DonwloadURL = Constants.FilesURL + "ImportTrainee.xlsx";
    const param = this.route.snapshot.paramMap.get('Id');
    if (param) {
      this.trainingId = param;
      this.loadData();
    }
  }
  filterbtn() {
    this.traineelst = this.filtertxt ? this.loadDataFilter() : this.trainingObj.Trainees;
  }
  loadDataFilter(): cTraineeItemInfo[] {
    return this.trainingObj.Trainees.filter((item: cTraineeItemInfo) =>
      item.Name.toLocaleLowerCase().indexOf(this.filtertxt.toLocaleLowerCase()) !== -1
      || item.Email.toLocaleLowerCase().indexOf(this.filtertxt.toLocaleLowerCase()) !== -1
      || item.Mobile.toLocaleLowerCase().indexOf(this.filtertxt.toLocaleLowerCase()) !== -1
      || item.NationalId.toLocaleLowerCase().indexOf(this.filtertxt.toLocaleLowerCase()) !== -1);
  }
  loadData() {
    this.BLServiceTraining.getGetByid(this.trainingId).subscribe({
      next: obj => {
        this.trainingObj = obj;
        this.filterbtn();
      },
      error: err => this.message.Error(err)
    });
  }
  setDeactivate(traineeId: string) {
    if (!confirm(this.msgsetDeleted))
      return;

    this.BLServiceTrainee.RemoveTraining(this.trainingId, traineeId).subscribe({
      next: response => {
        this.message.Success(this.msgDeletedSuccessfully);
        this.BLServiceShowMessage.sendMessage(this.message);
        this.loadData();
      },
      error: err => this.message.Error(err)
    });
  }
  isCourseStarted() {
    if (this.trainingObj.StartDate < this.todayDate) //if start date less than today, course has started
      return true;

    return false;
  }
  AddTrainee(content) {
    this.modalService.open(content, { backdropClass: 'light-blue-backdrop', centered: true });
  }
  OnSelectedTrainee(TraineeId: string): void {
    this.IsTraineeUpload = true;
    this.BLServiceTrainee.AddTraining(this.trainingId, TraineeId).subscribe({
      next: response => {
        this.IsTraineeUpload = false;
        this.loadData();
        this.message.Success(this.msgSavedSuccessfully);
        this.BLServiceShowMessage.sendMessage(this.message);
        this.loadData();
      },
      error: err => {
        this.message.Error(err);
        this.BLServiceShowMessage.sendMessage(this.message);
        this.IsTraineeUpload = false;
      }
    });
  }
  onBack(): void {
    this.router.navigate(['/Training/List']);
  }
  createBtn() {
    // path: 'Trainee/Create/:IsCreate/:TraineeId/:TrainingId'
    this.router.navigate(['/Trainee/Create/1/0/' + this.trainingId]);
  }
  Approve(TraineeId: string) {
    var model = new ModelTraineeTraining();
    model.TraineeId = TraineeId;
    model.TrainingId = this.trainingId;

    this.BLServiceTrainee.ApproveTraineeRegister(model).subscribe({
      next: response => {
        this.loadData();
        this.message.Success(this.msgSavedSuccessfully);
        this.BLServiceShowMessage.sendMessage(this.message);
        this.loadData();
      },
      error: err => this.message.Error(err)
    });
  }
  Certificate(TraineeId) {
    // path: 'Trainee/Create/:IsCreate/:TraineeId/:TrainingId'
    this.router.navigate(['/MyTraining/' + TraineeId]);
  }
  public uploadFile = (files) => {
    if (files.length === 0) {
      return;
    }

    let fileToUpload = <File>files[0];
    if (fileToUpload.type != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
      this.message.Error(this.ExcelOnly);

      this.BLServiceShowMessage.sendMessage(this.message);
    } else {
      this.IsTraineeUpload = true;
      this.BLServiceTrainee.ImportTrainee(fileToUpload, this.trainingId).subscribe({
        next: response => {
          this.fileInput.nativeElement.value = '';
          this.IsTraineeUpload = false;
          if(response.IsValid)
          {
            this.message.Success(this.msgSavedSuccessfully);
            this.BLServiceShowMessage.sendMessage(this.message);
          }
          else{
            this.message.Error(response.Error);
            this.BLServiceShowMessage.sendMessage(this.message);
            if(response.FileURL != "")
            {
              this.DownloadURLError = Constants.FilesURL + response.FileURL;
            }
          }
          this.loadData();
          
        },

        error: err => {
          this.fileInput.nativeElement.value = '';
          this.message.Error(err);
          this.BLServiceShowMessage.sendMessage(this.message);
          this.IsTraineeUpload = false;
        }
      });
    }
  }
  DownloadCertificate() {
    this.BLServiceTrainee.DownloadTrainingCertificate(this.trainingId).subscribe({
      next: data => {
        window.open(Constants.FilesURL + data, "_blank");
      },

      error: err => {
        this.message.Error(this.msgTrainingNoCertificates);
        this.BLServiceShowMessage.sendMessage(this.message);
      }
    });
  }
  ExportBtn(){
    this.BLServiceTraining.ExportTrainingTrainee(this.trainingId).subscribe({
      next: data => {
        this.downloadFile(data);
      },
      error: err => this.message.Error(err)
    });
  }
}
