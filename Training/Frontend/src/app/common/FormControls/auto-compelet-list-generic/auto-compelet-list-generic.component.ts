import { Component, OnInit, Output, Input, EventEmitter } from '@angular/core';
import { TypeaheadMatch } from 'ngx-bootstrap/typeahead/public_api';

@Component({
  selector: 'cust-auto-compelet-list-generic',
  templateUrl: './auto-compelet-list-generic.component.html',
  styleUrls: ['./auto-compelet-list-generic.component.scss']
})
export class AutoCompeletListGenericComponent implements OnInit {
 
  @Output() OnSelectedItem : EventEmitter<string> = new EventEmitter<string>();
  @Input() lstTrainingCenters: any[];

  search: string;
  noResult: boolean = false;
  errorMessage: string;
  selectedOption: any;
  typeaheadLoading: boolean;
  constructor() { }
  ngOnInit(){}
  onSelect(event: TypeaheadMatch): void {
    this.selectedOption = event.item.Id;
    this.OnSelectedItem.emit(event.item.Id);
  }
  changeTypeaheadLoading(e: boolean): void {
    this.typeaheadLoading = e;
  }
  typeaheadNoResults(event: boolean): void {
    this.noResult = event;
  }

}
