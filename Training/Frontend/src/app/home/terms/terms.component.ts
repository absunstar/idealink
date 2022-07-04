import { Component, OnInit } from '@angular/core';
import { ModelContentData } from 'src/app/interface/Model/ModelContentData.class';
import { EnumContentData } from 'src/app/Enum/ContentData.enum';
import { ServiceMisc } from 'src/app/services/misc.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { ShowMessage } from 'src/app/interface/Model/ModelShowMessage.class';

@Component({
  selector: 'app-terms',
  templateUrl: './terms.component.html',
  styleUrls: ['./terms.component.css']
})
export class TermsComponent implements OnInit {

  msgShow: ShowMessage = new ShowMessage();
  modelData: ModelContentData = new ModelContentData();

  constructor(private BLServiceMisc: ServiceMisc,
    private BLServiceMessage: ServiceShowMessage) { 

  }

  ngOnInit(): void {
    this.BLServiceMisc.ContentDataOneGetByTypeId(EnumContentData.Terms).subscribe({
      next: obj => {
        this.modelData = obj;
      },
      error: err => this.msgShow.Error(err)
    });
  }

}
