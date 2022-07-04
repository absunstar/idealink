import { Component, OnInit, Input } from '@angular/core';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';

@Component({
  selector: 'app-employer-menu',
  templateUrl: './employer-menu.component.html',
  styleUrls: ['./employer-menu.component.css']
})
export class EmployerMenuComponent implements OnInit {

  userName: string;
  @Input() pageName: string = "";
  constructor(  protected BLServiceLoginUser: ServiceLoginUser) { 
    this.userName = this.BLServiceLoginUser.userName;
    this.BLServiceLoginUser.UserNameChanged.subscribe(obj => {
      this.userName = obj;
    });
  }

  ngOnInit(): void {
  }

}
