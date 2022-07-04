import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { tap, switchMap, map } from 'rxjs/operators';
import { of, noop, Observer, Observable } from 'rxjs';
import { ServiceUserProfile } from 'src/app/services/userprofile.service';
import { TypeaheadMatch } from 'ngx-bootstrap/typeahead/typeahead-match.class';
import { cUserProfileItem } from 'src/app/interface/Response/UserProfile.class';

@Component({
  selector: 'cust-search-partner',
  templateUrl: './search-partner.component.html',
  styleUrls: ['./search-partner.component.scss']
})
export class SearchPartnerComponent implements OnInit {
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
          return this.BLServiceUserProfile.GetPartnerSearch(query).pipe(
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
