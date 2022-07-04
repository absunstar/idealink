import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { cTrainingItem } from 'src/app/interface/Response/Training.class';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { ServiceTraining } from 'src/app/services/training.service';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { cTraineeItem } from 'src/app/interface/Response/Trainee.class';
import { ServiceTrainee } from 'src/app/services/trainee.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-trainee-list',
  templateUrl: './trainee-list.component.html',
  styleUrls: ['./trainee-list.component.css']
})
export class TraineeListComponent extends baseComponent implements OnInit {

  pageTitle: string = "Trainee List"
  trainingId: string;
  trainingObj: cTrainingItem;
  traineelst: cTraineeItem[];
  todayDate = new Date();

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
    BLServiceLoginUser: ServiceLoginUser) {
    super(BLServiceShowMessage, BLServiceLoginUser);

  }

  ngOnInit(): void {
    
    const param = this.route.snapshot.paramMap.get('Id');
    if (param) {
      this.trainingId = param;
      this.loadData();
    }
  }
  filterbtn() {
    this.traineelst = this.filtertxt ? this.loadDataFilter() : this.trainingObj.Trainees;
  }
  loadDataFilter(): cTraineeItem[] {
    return this.trainingObj.Trainees.filter((item: cTraineeItem) =>
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
  setDeActivate(traineeId: string) {
    if (!confirm("Are you sure you want to Deactivate?"))
      return;

    this.BLServiceTrainee.RemoveTraining(this.trainingId,traineeId).subscribe({
      next: response => {
        this.message.Success("Saved successfully.");
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
  AddTrainee(content){
    this.modalService.open(content, { backdropClass: 'light-blue-backdrop', centered: true });
  }
  OnSelectedTrainee(TraineeId: string): void{
    this.BLServiceTrainee.AddTraining(this.trainingId,TraineeId).subscribe({
      next: response => {
        this.loadData();
        this.message.Success("Saved successfully.");
        this.BLServiceShowMessage.sendMessage(this.message);
        this.loadData();
      },
      error: err => this.message.Error(err)
    });
  }
  onBack(): void {
    this.router.navigate(['/Training/List']);
  }
}
