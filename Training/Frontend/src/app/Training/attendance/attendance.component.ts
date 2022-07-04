import { Component, OnInit, ViewChild, ElementRef, QueryList, ViewChildren } from '@angular/core';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ActivatedRoute, Router } from '@angular/router';
import { ServiceTraining } from 'src/app/services/training.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { cTrainingItem, cAttendance, cSessionItem, cAttendanceTrainee } from 'src/app/interface/Response/Training.class';
import { cTraineeItem } from 'src/app/interface/Response/Trainee.class';
import { MatSlideToggle } from '@angular/material';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-attendance',
  templateUrl: './attendance.component.html',
  styleUrls: ['./attendance.component.css']
})
export class AttendanceComponent extends baseComponent implements OnInit {

  pageTitle: string = 'Attendance';
  trainingId: string;
  trainingObj: cTrainingItem;
  attendance: cAttendance[];
  sessions: cSessionItem[];
  trainees: cTraineeItem[];
  selectedSessionId: string = "";
  ModelAttendance: cAttendance;


  @ViewChildren("toggleElement") ref: QueryList<MatSlideToggle>;

  constructor(private route: ActivatedRoute,
    private BLServiceTraining: ServiceTraining,
    BLServiceShowMessage: ServiceShowMessage,
    private router: Router,
    BLServiceLoginUser: ServiceLoginUser,BLTranslate: TranslateService,) {
    super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);

  }

  ngOnInit(): void {
    const param = this.route.snapshot.paramMap.get('Id');
    if (param) {
      this.trainingId = param;
      this.loadData();
    }
  }
  loadData() {
    this.BLServiceTraining.getGetByid(this.trainingId).subscribe({
      next: obj => {
        this.trainingObj = obj;
        this.attendance = obj.Attendances;
        this.sessions = obj.Sessions;
        this.trainees = obj.Trainees;
      },
      error: err => this.message.Error(err)
    });
  }
  getTraineeAttendance(sessionId: string, TraineeId: string): string {
    if (!this.attendance)
      return "NA";

    var attSession = this.attendance.filter(x => x.SessionId == sessionId);
    if (!attSession)
      return "NA";

    var attTrainee = attSession[0]?.Attendances?.filter(x => x.TraineeId == TraineeId);
    if (!attTrainee)
      return "NA";

    return attTrainee[0]?.IsAttendant ? "true" : "false";
  }
  EnterAttendance(sessionId: string) {
    this.selectedSessionId = sessionId;
    this.ModelAttendance = new cAttendance();

    this.ModelAttendance.SessionId = sessionId;
    this.ModelAttendance.Attendances = [];
    this.trainees.forEach(element => {
      var att = new cAttendanceTrainee();
      att.TraineeId = element.Id;
      att.IsAttendant = this.getTraineeAttendance(sessionId, element.Id) == "true" ? true : false;
      this.ModelAttendance.Attendances.push(att);
    });
  }
  CancelAttendance() {
    this.selectedSessionId = "";
    this.ModelAttendance = new cAttendance();
  }
  AttendanceTraineeadded(traineeId: string, event: any) {
    this.updateAttendanceModel(traineeId, event.checked);
  }
  updateAttendanceModel(traineeId: string, isChecked: boolean) {
    var attIndex = this.ModelAttendance.Attendances.findIndex(x => x.TraineeId == traineeId);
    this.ModelAttendance.Attendances[attIndex].IsAttendant = isChecked;
  }
  MarkAll() {
    this.ref.toArray().forEach(item => {item.checked = true;});
    this.ModelAttendance.Attendances.forEach(item => { item.IsAttendant = true });
  }
  UnMarkAll() {
    this.ref.toArray().forEach(item => {item.checked = false;});
    this.ModelAttendance.Attendances.forEach(item => { item.IsAttendant = false });
  }
  Save() {
    this.BLServiceTraining.SaveAttendnace(this.trainingId, this.ModelAttendance).subscribe({
      next: obj => {
        this.selectedSessionId = "";
        this.loadData();
      },
      error: err => this.message.Error(err)
    });
  }
  onBack(): void {
    this.router.navigate(['/Training/List']);
  }
}
