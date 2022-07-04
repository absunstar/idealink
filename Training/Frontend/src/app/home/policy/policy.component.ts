import { Component, OnInit } from '@angular/core';
import { ShowMessage } from 'src/app/interface/Model/ModelShowMessage.class';
import { ModelContentData } from 'src/app/interface/Model/ModelContentData.class';
import { ServiceMisc } from 'src/app/services/misc.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { EnumContentData } from 'src/app/Enum/ContentData.enum';

@Component({
  selector: 'app-policy',
  templateUrl: './policy.component.html',
  styleUrls: ['./policy.component.css']
})
export class PolicyComponent implements OnInit {

  msgShow: ShowMessage = new ShowMessage();
  modelData: ModelContentData = new ModelContentData();

  constructor(private BLServiceMisc: ServiceMisc,
    private BLServiceMessage: ServiceShowMessage) { 

  }

  ngOnInit(): void {
    this.BLServiceMisc.ContentDataOneGetByTypeId(EnumContentData.Policy).subscribe({
      next: obj => {
        this.modelData = obj;
      },
      error: err => this.msgShow.Error(err)
    });
  }

}
