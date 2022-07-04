import { Component, OnInit } from '@angular/core';
import { ConfigForm } from 'src/app/Enum/ConfigForm.enum';

@Component({
  selector: 'app-config-request-register',
  templateUrl: './config-request-register.component.html',
  styleUrls: ['./config-request-register.component.css']
})
export class ConfigRequestRegisterComponent implements OnInit {
  pageTitle: string = "Config Request Registeration";
  formType: ConfigForm = ConfigForm.RequestRegister;
  constructor() { }

  ngOnInit(): void {
  }

}
