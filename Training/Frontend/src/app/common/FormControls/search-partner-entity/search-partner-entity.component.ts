import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { TypeaheadMatch } from 'ngx-bootstrap/typeahead/public_api';
import { tap, switchMap } from 'rxjs/operators';
import { noop, of, Observable, Observer } from 'rxjs';
import { ServiceEntityManagement } from 'src/app/services/entitymanagement.service';
import { cEntityPartnerItem } from 'src/app/interface/Response/EntityPartner.class';

@Component({
  selector: 'cust-search-partner-entity',
  templateUrl: './search-partner-entity.component.html',
  styleUrls: ['./search-partner-entity.component.css']
})
export class SearchPartnerEntityComponent implements OnInit {

 
  @Output() OnSelectedPartner : EventEmitter<string> = new EventEmitter<string>();
  
  noResult: boolean = false;
  search: string;
  suggestions$: Observable<cEntityPartnerItem[]>;
  errorMessage: string;
  selectedOption: any;
  typeaheadLoading: boolean;
  constructor(private BLServiceEntity: ServiceEntityManagement) { }
  ngOnInit(): void {
    this.suggestions$ = new Observable((observer: Observer<string>) => {
      observer.next(this.search);
    }).pipe(
      switchMap((query: string) => {
        if (query) {
          return this.BLServiceEntity.getEntityPartnerListActive(query).pipe(
            tap(() => noop, err => {
              // in case of http error
              this.errorMessage = err && err.message || 'Something goes wrong';
            }));
        }

        return of([]);
      })
    );
  }
  typeaheadNoResults(event: boolean): void {
    this.noResult = event;
  }
  onSelect(event: TypeaheadMatch): void {
    this.selectedOption = event.item.Id;
    this.OnSelectedPartner.emit(event.item.Id);
  }
  changeTypeaheadLoading(e: boolean): void {
    this.typeaheadLoading = e;
  }



}
