import { Component, OnInit } from '@angular/core';
import { ModelContentData } from 'src/app/interface/Model/ModelContentData.class';
import { ShowMessage } from 'src/app/interface/Model/ModelShowMessage.class';
import { ServiceContentData } from 'src/app/services/content-data.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { EnumContentData } from 'src/app/Enum/ContentData.enum';
import { NgForm } from '@angular/forms';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-term-edit',
  templateUrl: './term-edit.component.html',
  styleUrls: ['./term-edit.component.css']
})
export class TermEditComponent extends baseComponent implements OnInit {

  
  pageTitle: string = 'Editing Terms & Conditios Section';
  modelObj: ModelContentData = new ModelContentData();
  message: ShowMessage = new ShowMessage();
  
  constructor(private BLService: ServiceContentData,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser : ServiceLoginUser,BLTranslate: TranslateService,) { 
      super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
    }

  ngOnInit(): void {
    this.loadData();
  }
loadData() {
    this.BLService.ContentDataOneGetByTypeId(EnumContentData.Terms).subscribe({
      next: obj => {
        this.modelObj = obj;
      },
      error: err => this.message.Error(err)
    });
  }
  modelSaveBtn(modelForm: NgForm): void {
    if (!modelForm.valid)
      return;

      this.BLService.updateContentData(this.modelObj).subscribe({
        next: response => {
          this.message.Success(this.msgSavedSuccessfully);
          this.BLServiceShowMessage.sendMessage(this.message);
        },
        error: err => this.message.Error(err)
      });
    }

}
