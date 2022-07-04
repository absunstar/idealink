import { Component, OnInit } from '@angular/core';
import { ConfigForm } from 'src/app/Enum/ConfigForm.enum';

@Component({
  selector: 'app-job-fair-create',
  templateUrl: './job-fair-create.component.html',
  styleUrls: ['./job-fair-create.component.css']
})
export class JobFairCreateComponent implements OnInit {

  formType: ConfigForm = ConfigForm.JobFairCreate;
  constructor() { }

  ngOnInit(): void {
  }

}
