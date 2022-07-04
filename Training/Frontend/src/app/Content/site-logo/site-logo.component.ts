import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ShowMessage } from 'src/app/interface/Model/ModelShowMessage.class';
import { ServiceContentData } from 'src/app/services/content-data.service';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';

@Component({
  selector: 'app-site-logo',
  templateUrl: './site-logo.component.html',
  styleUrls: ['./site-logo.component.css']
})
export class SiteLogoComponent extends baseComponent implements OnInit {

  message: ShowMessage = new ShowMessage();
  msgPngOnly: string;

  @ViewChild('fileInput') fileInput: any;

  constructor(private BLService: ServiceContentData,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser, BLTranslate: TranslateService,) {
    super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
    this.BLTranslate.get("msgPngOnly").subscribe(res => { this.msgPngOnly = res; });
  }

  ngOnInit(): void {

  }
  modelSaveBtn(files): void {
    if (files.length === 0) {
      return;
    }
    let fileToUpload = <File>files[0]; 
    if (fileToUpload.type != "image/png") {
      this.message.Error(this.msgPngOnly);
      this.BLServiceShowMessage.sendMessage(this.message);
    } else {
      this.BLService.updateSiteLogo(fileToUpload).subscribe({
        next: response => {
          this.fileInput.nativeElement.value = '';
          this.message.Success(this.msgSavedSuccessfully);
          this.BLServiceShowMessage.sendMessage(this.message);
        },
        error: err => {
          this.fileInput.nativeElement.value = '';
          this.message.Error(err);
          this.BLServiceShowMessage.sendMessage(this.message);
        }
      });
    }
  }
}
