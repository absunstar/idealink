import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/core/auth-service.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private _authService: AuthService) { }

  ngOnInit(): void {
  }
  login() {
    this._authService.loginAdmin();
  }
}
