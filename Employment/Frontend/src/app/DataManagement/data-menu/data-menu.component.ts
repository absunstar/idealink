import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-data-menu',
  templateUrl: './data-menu.component.html',
  styleUrls: ['./data-menu.component.css']
})
export class DataMenuComponent implements OnInit {
  @Input() pageName:string = "";
  constructor() { }

  ngOnInit(): void {
  }

}
