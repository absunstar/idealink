import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Observable, noop, Observer, of } from 'rxjs';
import { ServiceEntityManagement } from 'src/app/services/entitymanagement.service';
import { switchMap, tap } from 'rxjs/operators';
import { cEntityPartnerItem } from 'src/app/interface/Response/EntityPartner.class';
import { TypeaheadMatch } from 'ngx-bootstrap/typeahead/public_api';
import { cEntitySubPartnerItem } from 'src/app/interface/Response/EntitySubPartner.class';

@Component({
  selector: 'cust-search-sub-partner-entity',
  templateUrl: './search-sub-partner-entity.component.html',
  styleUrls: ['./search-sub-partner-entity.component.scss']
})
export class SearchSubPartnerEntityComponent implements OnInit {

  @Output() OnSelectedPartner : EventEmitter<string> = new EventEmitter<string>();
  
  noResult: boolean = false;
  search: string;
  suggestions$: Observable<cEntitySubPartnerItem[]>;
  errorMessage: string;
  selectedOption: any;
  typeaheadLoading: boolean;
  constructor(private BLServiceEntityManagement: ServiceEntityManagement) { }
  ngOnInit(): void {
    this.suggestions$ = new Observable((observer: Observer<string>) => {
      observer.next(this.search);
    }).pipe(
      switchMap((query: string) => {
        if (query) {
          return this.BLServiceEntityManagement.getEntitySubPartnerListActive(query).pipe(
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
