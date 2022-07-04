import { Component, OnInit } from '@angular/core';
import { ConfigForm } from 'src/app/Enum/ConfigForm.enum';

@Component({
  selector: 'app-config-trainee',
  templateUrl: './config-trainee.component.html',
  styleUrls: ['./config-trainee.component.css']
})
export class ConfigTraineeComponent implements OnInit {
  pageTitle: string = "Config Trainee";
  formType: ConfigForm = ConfigForm.Trainee;
  constructor() { }

  ngOnInit(): void {
  }

}
