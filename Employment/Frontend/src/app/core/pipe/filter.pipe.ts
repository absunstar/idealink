// // filter.pipe.ts

// import { Pipe, PipeTransform } from '@angular/core';

// @Pipe({ name: 'Filter' })
// export class FilterPipe implements PipeTransform {
//   transform(items: any[], searchText: string): any[] {
//     if (!items) {
//       console.log("1",items);
//       return [];
//     }
//     if (!searchText) {
//       console.log("11",items);
//       return items;
//     }
//     searchText = searchText.toLocaleLowerCase();
//     console.log("10",items);
//     return items.filter(it => {
//       return it.toLocaleLowerCase().includes(searchText);
//     });
//   }
// }


import { Pipe, PipeTransform } from '@angular/core';


@Pipe({
  name: 'Filter'
})
export class FilterPipe implements PipeTransform {

  transform(value, args): any {
    // return value.filter(function(search) {
    //   return search.name.indexOf(searchTerm) > -1
    // })
    console.log(value); 

    if(!args) {
      return value;
    }

     return value.filter(item =>{
       console.log(item);
       item.LkupIndustry.Name.indexOf(args) > -1}); 
  }
}
 