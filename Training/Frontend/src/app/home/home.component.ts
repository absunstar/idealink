import { Component, OnInit } from '@angular/core';
import { ServiceShowMessage } from '../services/show-message.service';
import { ActivatedRoute } from '@angular/router';
import { ShowMessage } from '../interface/Model/ModelShowMessage.class';
import { ModelContentData } from '../interface/Model/ModelContentData.class';
import { EnumContentData } from '../Enum/ContentData.enum';
import { ServiceContentData } from '../services/content-data.service';
import { ServiceMisc } from '../services/misc.service';
import { StatsCount } from '../interface/Response/StatsCount.class';


@Component({
  selector: 'app-home',
  templateUrl: 'home.component.html'
})

export class HomeComponent implements OnInit {
  
  msgShow: ShowMessage = new ShowMessage();
  modelAboutData: ModelContentData = new ModelContentData();
  modelInfoData: ModelContentData = new ModelContentData();
  objStats: StatsCount = new StatsCount();

  constructor(private route: ActivatedRoute,
    private BLServiceMisc: ServiceMisc,
    private BLServiceMessage: ServiceShowMessage) { }

  ngOnInit() {
    const msgRedirect = this.route.snapshot.paramMap.get('msg');
    const msgIsSuccess = this.route.snapshot.paramMap.get('isSuccess');
    if (msgRedirect) {
      var isSuccess = false;
      if (msgIsSuccess)
        isSuccess = msgIsSuccess.toLowerCase() == "true" ? true : false;

      this.msgShow.Send(msgRedirect, isSuccess);
      this.BLServiceMessage.sendMessage(this.msgShow);
    }
    this.loadData();
  }
  loadData() {
    this.BLServiceMisc.ContentDataOneGetByTypeId(EnumContentData.About).subscribe({
      next: obj => {
        this.modelAboutData = obj;
      },
      error: err => this.msgShow.Error(err)
    });
    this.BLServiceMisc.ContentDataOneGetByTypeId(EnumContentData.Info).subscribe({
      next: obj => {
        this.modelInfoData = obj;
      },
      error: err => this.msgShow.Error(err)
    });
    this.BLServiceMisc.GetStats().subscribe({
      next: obj => {
        this.objStats = obj;
      },
      error: err => this.msgShow.Error(err)
    });
  }
}