import { Component, OnInit } from '@angular/core';
import { MyTraining, MyTrainingItems } from 'src/app/interface/Response/MyTraining.class';
import { ShowMessage } from 'src/app/interface/Model/ModelShowMessage.class';
import { Page } from 'src/app/common/pagination/page';
import { ServiceTrainee } from 'src/app/services/trainee.service';
import { CustomPaginationService } from 'src/app/common/pagination/services/custom-pagination.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { Router, ActivatedRoute } from '@angular/router';
import { Constants } from 'src/app/constants';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-my-trainings',
  templateUrl: './my-trainings.component.html',
  styleUrls: ['./my-trainings.component.css']
})
export class MyTrainingsComponent extends baseComponent implements OnInit {
  pageTitle: string = 'My Trainings';
  lstData: MyTraining;
  message: ShowMessage = new ShowMessage();
  filtertxt: string = '';
  modelName: string = '';
  modelId: string = '';
  modelNameEdited: string = '';
  TraineeId: string = "";
  page: Page<MyTrainingItems> = new Page();


  constructor(private BLService: ServiceTrainee,
    private paginationService: CustomPaginationService,
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
      this.TraineeId = param;
      this.BLService.GetTraineeTraining(this.TraineeId).subscribe({
        next: lst => {
          this.lstData = lst;
          this.page.pageable.pageSize = Constants.PAGE_SIZE;
          this.page.totalElements = lst.trainings.length;
          this.page.content = lst.trainings;
        },
        error: err => this.message.Error(err)
      });
    }
    else {
      this.BLService.GetMyTraining().subscribe({
        next: lst => {
          this.lstData = lst;
          this.page.pageable.pageSize = Constants.PAGE_SIZE;
          this.page.totalElements = lst.trainings.length;
          this.page.content = lst.trainings;
        },
        error: err => this.message.Error(err)
      });
    }
  }
  public getNextPage(): void {
    this.page.pageable = this.paginationService.getNextPage(this.page);
    this.loadData();
  }

  public getPreviousPage(): void {
    this.page.pageable = this.paginationService.getPreviousPage(this.page);
    this.loadData();
  }
  public getloadPageCurrent(): void {
    this.loadData();
  }
  TakeExam(Id) {
    this.router.navigate(['/Exam/TakeExam/' + Id]);
  }
}
