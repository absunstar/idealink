import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Observable, Observer, noop, of } from 'rxjs';
import { map, switchMap, tap } from 'rxjs/operators';
import { TypeaheadMatch } from 'ngx-bootstrap/typeahead/public_api';
import { cJobItem } from 'src/app/interface/Response/Job.class';
import { ServiceJob } from 'src/app/services/job.service';
import { ServiceUserProfile } from 'src/app/services/userprofile.service';
import { cUserProfileItem } from 'src/app/interface/Response/UserProfile.class';

@Component({
  selector: 'app-search-employer',
  templateUrl: './search-employer.component.html',
  styleUrls: ['./search-employer.component.css']
})
export class SearchEmployerComponent implements OnInit {

  
 
  @Output() OnSelect : EventEmitter<string> = new EventEmitter<string>();
  
  noResult: boolean = false;
  search: string;
  suggestions$: Observable<cJobItem[]>;
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
          return this.BLService.searchActiveEmployer(query).pipe(
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
