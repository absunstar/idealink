import { Component, OnInit } from '@angular/core';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { ServiceExam } from 'src/app/services/exam.service';
import { ShowMessage } from 'src/app/interface/Model/ModelShowMessage.class';
import { cQuestionItemTemplate, cAnswerItem } from 'src/app/interface/Model/ModelQuestions.class';
import { Constants } from 'src/app/constants';
import { ActivatedRoute, Router } from '@angular/router';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-take-exam',
  templateUrl: './take-exam.component.html',
  styleUrls: ['./take-exam.component.css']
})
export class TakeExamComponent extends baseComponent implements OnInit {

  message: ShowMessage = new ShowMessage();
  pageTitle: string = 'Exam';
  lstQuestion: cQuestionItemTemplate[];
  ExamId: string;
  config: any;
  modelId: string;
  ExamSuccess:string;
  ExamFailed:string;
  notFoundlist:boolean=false
  constructor(private BLServiceExam: ServiceExam,
    private route: ActivatedRoute,
    private router: Router,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser,BLTranslate: TranslateService) {
      super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
     }

  ngOnInit() {
    this.BLTranslate.get("ExamSuccess").subscribe(res => { this.ExamSuccess = res; });
    this.BLTranslate.get("ExamFailed").subscribe(res => { this.ExamFailed = res; });

    const param = this.route.snapshot.paramMap.get('Id');
    if (param) {
      this.modelId = param;
      this.loadData();
    }
  }
  private loadData(): void {
    this.BLServiceExam.takeExam(this.modelId).subscribe({
      next: examModel => {
        
        this.lstQuestion = examModel.questions;
        this.ExamId = examModel.ExamId;
        this.config = {};
        this.config.leftTime = Constants.EXAM_TIMER * 60;
        this.config.format = 'm:s';
        this.notFoundlist=false;
      },

      error: err => {  
        this.BLTranslate.get(err).subscribe(res => {this.notFoundlist=true; this.message.Error(res); });
        
      }
    });
  }
  onChange(e, questionItem: cQuestionItemTemplate, answer: cAnswerItem) {
    if (e.target.value == "on")
      questionItem.SelectedAnswer = answer._id;
  }

  submitExam() {
    this.BLServiceExam.SubmitExam(this.ExamId, this.lstQuestion).subscribe({
      next: isSuccess => {
        if (isSuccess) {
          this.message.Success(this.ExamSuccess);
          this.BLServiceShowMessage.sendMessage(this.message);
        }
        else{
          this.message.Error(this.ExamFailed);
          this.BLServiceShowMessage.sendMessage(this.message);
        }
        this.router.navigate(['/MyTraining']);
      },
      error: err => this.message.Error(err)
    });

  }

  handleEvent(event) {
    //TODO enhance the way to check timer is ended
    if (event.text == "0:0")
      this.submitExam();
  }




}
