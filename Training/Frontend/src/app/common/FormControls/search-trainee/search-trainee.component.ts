import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { TypeaheadMatch } from 'ngx-bootstrap/typeahead/public_api';
import { switchMap, tap, map } from 'rxjs/operators';
import { noop, Observable, Observer, of } from 'rxjs';
import { cTraineeItem } from 'src/app/interface/Response/Trainee.class';
import { ServiceTrainee } from 'src/app/services/trainee.service';

@Component({
  selector: 'cust-search-trainee',
  templateUrl: './search-trainee.component.html',
  styleUrls: ['./search-trainee.component.css']
})
export class SearchTraineeComponent implements OnInit {
  @Output() OnSelectedTrainee: EventEmitter<string> = new EventEmitter<string>();
  noResult: boolean = false;
  search: string;
  suggestions$: Observable<string[]>;
  errorMessage: string;
  selectedOption: any;
  typeaheadLoading: boolean;
  constructor(private BLServiceTrainee: ServiceTrainee) { }
  ngOnInit(): void {
    this.suggestions$ = new Observable((observer: Observer<string>) => {
      observer.next(this.search);
    }).pipe(
      switchMap((query: string) => {
        if (query) {
          return this.BLServiceTrainee.ListSearch(query).pipe(
            map((items: cTraineeItem[]) => {
                return items.map((item: cTraineeItem) => (
                  { Name: item.Name + "<" + item.Email + ">" + item.Mobile + " - " + item.NationalId,
                Id:item.Id }))
              }),
            tap(() => noop, err => {
              // in case of http error
              this.errorMessage = err && err.message || 'Something goes wrong';
            }));
            // .forEach((items: cTraineeItem[]) => {
              
            //   return items.map((item: cTraineeItem) => (
            //     { Name: item.Name + "<" + item.Email + ">" + item.Mobile + " - " + item.NationalId }))
            // });
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
    this.OnSelectedTrainee.emit(event.item.Id);
  }
  changeTypeaheadLoading(e: boolean): void {
    this.typeaheadLoading = e;
  }


}
