import { Component, OnInit } from '@angular/core';
import { ConfigForm } from 'src/app/Enum/ConfigForm.enum';

@Component({
  selector: 'app-company-config',
  templateUrl: './company-config.component.html',
  styleUrls: ['./company-config.component.css']
})
export class CompanyConfigComponent implements OnInit {

  formType: ConfigForm = ConfigForm.Company;
  constructor() { }

  ngOnInit(): void {
  }

}
