import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ShowMessage } from 'src/app/interface/Model/ModelShowMessage.class';
import { cUserProfileTrainerCertificateItem, cUserProfileTrainerCertificateItemWithProfile } from 'src/app/interface/Response/UserProfile.class';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { ServiceUserProfile } from 'src/app/services/userprofile.service';

@Component({
  selector: 'app-trainer-certificate',
  templateUrl: './trainer-certificate.component.html',
  styleUrls: ['./trainer-certificate.component.css']
})
export class TrainerCertificateComponent extends baseComponent implements OnInit {
  pageTitle: string = 'My Trainings';
  lstData: cUserProfileTrainerCertificateItemWithProfile;
  message: ShowMessage = new ShowMessage();
  filtertxt: string = '';
  modelName: string = '';
  modelId: string = '0';
  modelNameEdited: string = '';

  constructor(private BLService: ServiceUserProfile,
    private router: Router,
    private route: ActivatedRoute,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser, BLTranslate: TranslateService,) {
    super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
  }

  ngOnInit(): void {
    this.loadData();
  }
  private loadData(): void {
    const param = this.route.snapshot.paramMap.get('Id');
    if (param) {
      this.modelId = param;
    }
    this.BLService.MyTrainerCertificate(this.modelId).subscribe({
      next: lst => {
        this.lstData = lst;
      },
      error: err => this.message.Error(err)
    });
  }
}
