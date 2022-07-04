import { Component, OnInit } from '@angular/core';
import { cQuestionList, cQuestionItem, cAnswerItem } from 'src/app/interface/Model/ModelQuestions.class';
import { ShowMessage } from 'src/app/interface/Model/ModelShowMessage.class';
import { ServiceQuestion } from 'src/app/services/Question.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { NgForm, FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { ServiceDataManagement } from 'src/app/services/datamanagement.service';
import { cTrainingTypeItem } from 'src/app/interface/Response/TrainingType.class';
import { ITrainingCategoryItem } from 'src/app/interface/Response/TrainingCategory.interface';
import { ActivatedRoute, Router } from '@angular/router';
import { ModelFilterTraining } from 'src/app/interface/Model/ModelFilterTraining.class';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-create',
  templateUrl: './create.component.html',
  styleUrls: ['./create.component.css']
})
export class CreateComponent extends baseComponent implements OnInit {

  pageTitle: string = 'Questions';
  lstQuestion: cQuestionList;
  message: ShowMessage = new ShowMessage();
  filtertxt: string = '';
  modelObj: cQuestionItem = new cQuestionItem();
  modelId: string = '';
  IsCreate: boolean;
  lstTrainingTypes: cTrainingTypeItem[];
  lstTrainingCategory: ITrainingCategoryItem[];
  TrainingTypeId: string;
  formQuestion: FormGroup;
  msgQuestion2Answer: string;
  msgQuestionCorrectAnswer: string;
  msgQuestionOneAnswer: string;

  get Answers(): FormArray {
    return <FormArray>this.formQuestion.get('Answers');
  }

  constructor(private BLServiceQuestion: ServiceQuestion,
    private route: ActivatedRoute,
    private BLServiceDataManagement: ServiceDataManagement,
    private fb: FormBuilder,
    private router: Router,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser, BLTranslate: TranslateService,) { super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate); }

  ngOnInit(): void {
    this.BLTranslate.get("msgQuestion2Answer").subscribe(res => { this.msgQuestion2Answer = res; });
    this.BLTranslate.get("msgQuestionCorrectAnswer").subscribe(res => { this.msgQuestionCorrectAnswer = res; });
    this.BLTranslate.get("msgQuestionOneAnswer").subscribe(res => { this.msgQuestionOneAnswer = res; });

    this.modelId = this.route.snapshot.paramMap.get('Id');
    this.IsCreate = this.modelId == "0";

    this.InitForm();
    if (!this.IsCreate) {
      this.BLServiceQuestion.getQuestionGetByid(this.modelId).subscribe({
        next: lst => {
          
          if (lst != null) {
            this.modelObj = lst;
            //this.InitForm();
            this.formQuestion.patchValue({
              Question: this.modelObj.Name,
              TrainingTypeId: this.modelObj.TrainingTypeId,
              TrainingCategoryId: this.modelObj.TrainingCategoryId,
              Difficulty: this.modelObj.Difficulty.toString(),
            });
            if (this.modelObj.Answer.length > 0) {
              this.Answers.removeAt(0);
              this.modelObj.Answer.forEach(item => {
                const control = <FormArray>this.formQuestion.get('Answers');
                control.push(this.buildAnswer(item.Name, item.IsCorrectAnswer));
              });
            }
            this.onTrainingTypeSelect()
          }
        },
        error: err => this.message.Error(err)
      });
    }

    this.BLServiceDataManagement.getTrainingTypeListActive().subscribe({
      next: lst => {
        this.lstTrainingTypes = lst;
      },
      error: err => this.message.Error(err)
    });

  }
  InitForm() {
    this.formQuestion = this.fb.group({
      Question: ['', [Validators.required]],
      TrainingTypeId: ['', [Validators.required]],
      TrainingCategoryId: ['', [Validators.required]],
      Difficulty: ['', [Validators.required]],
      Answers: this.fb.array([this.buildAnswer()]),
    });
  }
  buildAnswer(value: string = '', IsCorrect = false) {
    return this.fb.group({
      Answer: [value, [Validators.required]],
      IsCorrectAnswer: IsCorrect
    });
  }
  addAnswer() {
    const control = <FormArray>this.formQuestion.get('Answers');
    control.push(this.buildAnswer());
  }
  onTrainingTypeSelect() {
    if (this.formQuestion.get('TrainingTypeId').value == "")
      return;

    this.BLServiceDataManagement.getTrainingCategoryListByTrainingType(this.formQuestion.get('TrainingTypeId').value).subscribe({
      next: lst => {
        this.lstTrainingCategory = lst;
      },
      error: err => this.message.Error(err)
    });
  }
  removeAnswer(index: number) {
    var c = <FormArray>this.formQuestion.get('Answers')
    c.removeAt(index);
  }
  modelSaveBtn(): void {
    if (!this.formQuestion.valid)
      return;

    var model = new cQuestionItem();
    model.Id = this.modelId;
    model.Difficulty = (<FormArray>this.formQuestion.get('Difficulty')).value;
    model.Name = (<FormArray>this.formQuestion.get('Question')).value;
    model.TrainingCategoryId = (<FormArray>this.formQuestion.get('TrainingCategoryId')).value;
    model.TrainingTypeId = (<FormArray>this.formQuestion.get('TrainingTypeId')).value;
    model.Answer = <cAnswerItem[]>[];

    var ans = <FormArray>this.formQuestion.get('Answers');
    ans.value.forEach(item => {
      if (item.IsCorrectAnswer == undefined)
        item.IsCorrectAnswer = true;
    });
    
    if (!ans || ans.length < 2) {
      this.message.Error(this.msgQuestion2Answer);
      this.BLServiceShowMessage.sendMessage(this.message);
    }
    else {
      var correctAnswersCount = this.getCorrectAnswersCount(ans);
      if (correctAnswersCount == 0) {//Case no correct answer selected
        this.message.Error(this.msgQuestionCorrectAnswer);
        this.BLServiceShowMessage.sendMessage(this.message);
      }
      else if (correctAnswersCount > 1) { //Case selecting more than corrent answer
        this.message.Error(this.msgQuestionOneAnswer);
        this.BLServiceShowMessage.sendMessage(this.message);
      }
      else {
        ans.value.forEach(item => {

          var obj = new cAnswerItem();

          obj.Name = item.Answer;
          obj.IsCorrectAnswer = item.IsCorrectAnswer;

          model.Answer.push(obj);
        });
        if (this.IsCreate) {
          this.BLServiceQuestion.createQuestion(model).subscribe({
            next: response => {
              this.message.Success(this.msgSavedSuccessfully);
              this.BLServiceShowMessage.sendMessage(this.message);
              this.router.navigate(['/Questions/List']);
            },
            error: err => this.message.Error(err)
          });
        }
        else {
          this.BLServiceQuestion.updateQuestion(model).subscribe({
            next: response => {
              this.message.Success(this.msgUpdatedsuccessfully);
              this.BLServiceShowMessage.sendMessage(this.message);
              this.router.navigate(['/Questions/List']);
            },
            error: err => this.message.Error(err)
          });
        }
      }
    }
  }

  getCorrectAnswersCount(ans: FormArray): number {
    var correctAnswersCount = 0;
    ans.value.forEach(item => {
      if (item.IsCorrectAnswer) {
        correctAnswersCount++;
      }
    });
    return correctAnswersCount;
  }
  OnRedrirectBack() {
    this.router.navigate(['/Questions/List']);
  }
  CorrectAnswer(obj) {
    
    var ans = <FormArray>this.formQuestion.get('Answers');
    ans.value.forEach(item => {
      item.IsCorrectAnswer = false;
    });
    ans.value[obj].IsCorrectAnswer = true;
  }
}
