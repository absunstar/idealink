import { Component, OnInit } from '@angular/core';
import { ConfigForm } from 'src/app/Enum/ConfigForm.enum';

@Component({
  selector: 'app-job-fair-register',
  templateUrl: './job-fair-register.component.html',
  styleUrls: ['./job-fair-register.component.css']
})
export class JobFairRegisterComponent implements OnInit {

  formType: ConfigForm = ConfigForm.JobFairRegister;
  constructor() { }

  ngOnInit(): void {
  }

}
