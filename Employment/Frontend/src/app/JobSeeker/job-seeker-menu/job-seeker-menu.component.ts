import { Component, OnInit, Input } from '@angular/core';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-job-seeker-menu',
  templateUrl: './job-seeker-menu.component.html',
  styleUrls: ['./job-seeker-menu.component.css']
})
export class JobSeekerMenuComponent extends baseComponent implements OnInit {
  userName: string;
  @Input() pageName: string = "";
  constructor(protected BLServiceShowMessage: ServiceShowMessage,
    protected BLServiceLoginUser: ServiceLoginUser,BLTranslate: TranslateService,) {
    super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
    this.userName = this.BLServiceLoginUser.userName;
    this.BLServiceLoginUser.UserNameChanged.subscribe(obj => {
      this.userName = obj;
    });
  }

  ngOnInit(): void {
  }

}
