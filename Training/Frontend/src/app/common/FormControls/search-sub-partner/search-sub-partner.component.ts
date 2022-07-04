import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { TypeaheadMatch } from 'ngx-bootstrap/typeahead/public_api';
import { switchMap, tap } from 'rxjs/operators';
import { noop, Observable, Observer, of } from 'rxjs';
import { ServiceUserProfile } from 'src/app/services/userprofile.service';
import { cUserProfileItem } from 'src/app/interface/Response/UserProfile.class';

@Component({
  selector: 'cust-search-sub-partner',
  templateUrl: './search-sub-partner.component.html',
  styleUrls: ['./search-sub-partner.component.scss']
})
export class SearchSubPartnerComponent implements OnInit {

  @Output() OnSelectedPartner : EventEmitter<string> = new EventEmitter<string>();
  
  noResult: boolean = false;
  search: string;
  suggestions$: Observable<cUserProfileItem[]>;
  errorMessage: string;
  selectedOption: any;
  typeaheadLoading: boolean;
  constructor(private BLServiceUserProfile: ServiceUserProfile) { }
  ngOnInit(): void {
    this.suggestions$ = new Observable((observer: Observer<string>) => {
      observer.next(this.search);
    }).pipe(
      switchMap((query: string) => {
        if (query) {
          return this.BLServiceUserProfile.GetSubPartnerSearch(query).pipe(
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
