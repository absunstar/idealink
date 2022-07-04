; import { Component, OnInit } from '@angular/core';
import { TranslateList } from 'src/app/interface/Response/TranslateList.classl';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { TranslateService } from '@ngx-translate/core';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { ServiceTranslateList } from 'src/app/services/translate-list.service';
import { TranslateType } from 'src/app/Enum/TranslateType.enum';
import { ActivatedRoute, Router } from '@angular/router';
import { Constants } from 'src/app/constants';
import { ModelTranslateList } from 'src/app/interface/Model/ModelTranslateList.class';

@Component({
  selector: 'app-translate-list',
  templateUrl: './translate-list.component.html',
  styleUrls: ['./translate-list.component.css']
})
export class TranslateListComponent extends baseComponent implements OnInit {

  lstData: TranslateList;
  modelObj: ModelTranslateList = new ModelTranslateList();
  title: string;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private BLServiceTranslate: ServiceTranslateList,
    BLServiceShowMessage: ServiceShowMessage,
    BLTranslate: TranslateService,
    BLServiceLoginUser: ServiceLoginUser) {
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
    switch (this.modelObj.Type) {
      case TranslateType.City:
        this.BLTranslate.get("GenericCity").subscribe(res => { this.title = res; });
        break;
      case TranslateType.Area:
        this.BLTranslate.get("TrainingArea").subscribe(res => { this.title = res; });
        break;
      case TranslateType.TrainingCategory:
        this.BLTranslate.get("GenericTrainingCategory").subscribe(res => { this.title = res; });
        break;
      case TranslateType.TrainingType:
        this.BLTranslate.get("GenericTrainingType").subscribe(res => { this.title = res; });
        break;
      case TranslateType.Courses:
        this.BLTranslate.get("CoursesTitle").subscribe(res => { this.title = res; });
        break;
    }
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
      case TranslateType.City:
        this.router.navigate(['/DataManagement/City']);
        break;
      case TranslateType.Area:
        this.router.navigate(['/DataManagement/Area/' + this.modelObj.Id]);
        break;
      case TranslateType.TrainingCategory:
        this.router.navigate(['/DataManagement/TrainingCategory']);
        break;
      case TranslateType.TrainingType:
        this.router.navigate(['/DataManagement/TrainingType']);
        break;
      case TranslateType.Courses:
        this.router.navigate(['/DataManagement/Course/' + this.modelObj.Id]);
        break;
    }
  }
}
