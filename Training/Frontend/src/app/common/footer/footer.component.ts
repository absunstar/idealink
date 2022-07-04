import { Component, OnInit } from '@angular/core';
import { Constants } from 'src/app/constants';
import { cLogoPartnerItem } from 'src/app/interface/Response/LogoPartner.class';
import { ServiceMisc } from 'src/app/services/misc.service';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css']
})
export class FooterComponent implements OnInit {
  lstLogoPartner:cLogoPartnerItem[];
  FilesURL: string = Constants.FilesURL;
  
  constructor(private BLService: ServiceMisc) { }

  ngOnInit() {
    this.loadData();
  }
  loadData() {

    this.BLService.getLogoPartnerListActive().subscribe({
      next: lst => {
        this.lstLogoPartner = lst;
      }
    });
  }
}
