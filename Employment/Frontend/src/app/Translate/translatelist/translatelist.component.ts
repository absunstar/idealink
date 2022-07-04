import { Component, OnInit } from '@angular/core';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { TranslateList } from 'src/app/interface/Response/TranslateList.class';
import { ModelTranslateList } from 'src/app/interface/Model/ModelTranslateList.class';
import { ActivatedRoute, Router } from '@angular/router';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { TranslateService } from '@ngx-translate/core';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { TranslateType } from 'src/app/Enum/TranslateType.enum';
import { ServiceTranslateList } from 'src/app/services/translate-service.service';

@Component({
  selector: 'app-translatelist',
  templateUrl: './translatelist.component.html',
  styleUrls: ['./translatelist.component.css']
})
export class TranslatelistComponent extends baseComponent implements OnInit {

  lstData: TranslateList;
  modelObj: ModelTranslateList = new ModelTranslateList();
  title: string;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private BLServiceTranslate: ServiceTranslateList,
    BLServiceShowMessage: ServiceShowMessage,
    BLTranslate: TranslateService,
    BLServiceLoginUser: ServiceLoginUser,) {
    super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
  }

  ngOnInit(): void {

    const paramType = this.route.snapshot.paramMap.get('Type');
    const paramId = this.route.snapshot.paramMap.get('Id');
    if (paramId) {
      this.modelObj.Id = paramId
    }
    if (paramType) {
      this.modelObj.Type = +paramType;
      this.loadData();
      this.loadTitle();
    }
  }
  loadTitle() {
    // switch (this.modelObj.Type) {
    //   case TranslateType.Country:
    //     this.BLTranslate.get("GenericCity").subscribe(res => { this.title = res; });
    //     break;
    //   case TranslateType.City:
    //     this.BLTranslate.get("TrainingArea").subscribe(res => { this.title = res; });
    //     break;
    //   case TranslateType.TrainingCategory:
    //     this.BLTranslate.get("GenericTrainingCategory").subscribe(res => { this.title = res; });
    //     break;
    //   case TranslateType.TrainingType:
    //     this.BLTranslate.get("GenericTrainingType").subscribe(res => { this.title = res; });
    //     break;
    //   case TranslateType.Courses:
    //     this.BLTranslate.get("CoursesTitle").subscribe(res => { this.title = res; });
    //     break;
    // }
  }
  private loadData(): void {
    this.BLServiceTranslate.ListTranslationByType(this.modelObj).subscribe({
      next: lst => {
        this.lstData = lst;
      },
      error: err => this.message.Error(err)
    });
  }
  modelSaveBtn() {
    this.BLServiceTranslate.SaveListTranslation(this.lstData).subscribe({
      next: response => {
        this.message.Success(this.msgSavedSuccessfully);
        this.BLServiceShowMessage.sendMessage(this.message);
        this.OnRedrirectBack();
      },
      error: err => this.message.Error(err)
    });
  }
  OnRedrirectBack() {
    switch (this.modelObj.Type) {
      case TranslateType.Country:
        this.router.navigate(['/DataManagement/Country']);
        break;
      case TranslateType.City:
        this.router.navigate(['/DataManagement/City/' + this.modelObj.Id]);
        break;
      case TranslateType.JobField:
        this.router.navigate(['/DataManagement/JobFields']);
        break;
      case TranslateType.JobSubFields:
        this.router.navigate(['/DataManagement/JobSubFields/' + this.modelObj.Id]);
        break;
      case TranslateType.Qualification:
        this.router.navigate(['/DataManagement/Qualification']);
        break;
      case TranslateType.YearsOfExperience:
        this.router.navigate(['/DataManagement/YearsOfExperience']);
        break;
      case TranslateType.Languages:
        this.router.navigate(['/DataManagement/Languages']);
        break;
      case TranslateType.Industry:
        this.router.navigate(['/DataManagement/Industry']);
        break;

    }
  }
}
