import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-config-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {

  @Input() pageName :string;
  constructor() { }

  ngOnInit(): void {
  }

}
