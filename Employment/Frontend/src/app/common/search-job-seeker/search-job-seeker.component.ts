import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { cJobItem } from 'src/app/interface/Response/Job.class';
import { ServiceUserProfile } from 'src/app/services/userprofile.service';
import { Observer, noop, of } from 'rxjs';
import { switchMap, tap, map } from 'rxjs/operators';
import { TypeaheadMatch } from 'ngx-bootstrap/typeahead/public_api';
import { cUserProfileItem } from 'src/app/interface/Response/UserProfile.class';

@Component({
  selector: 'app-search-job-seeker',
  templateUrl: './search-job-seeker.component.html',
  styleUrls: ['./search-job-seeker.component.css']
})
export class SearchJobSeekerComponent implements OnInit {

 
  @Output() OnSelect : EventEmitter<string> = new EventEmitter<string>();
  
  noResult: boolean = false;
  search: string;
  suggestions$: Observable<cUserProfileItem[]>;
  errorMessage: string;
  selectedOption: any;
  typeaheadLoading: boolean;

  constructor(private BLService: ServiceUserProfile) { 
    
  }

  ngOnInit(): void {
    this.suggestions$ = new Observable((observer: Observer<string>) => {
      observer.next(this.search);
    }).pipe(
      switchMap((query: string) => {
        if (query) {
          return this.BLService.searchActiveJobSeeker(query).pipe(
            map((items: cUserProfileItem[]) => {
              return items.map((item: cUserProfileItem) => (
                { Name: item.Name + " <" + item.Email + ">",Id:item.Id }))
            }),
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
    this.OnSelect.emit(event.item.Id);
  }
  changeTypeaheadLoading(e: boolean): void {
    this.typeaheadLoading = e;
  }



}
