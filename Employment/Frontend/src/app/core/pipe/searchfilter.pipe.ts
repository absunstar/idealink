import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'searchfilter'
})
export class SearchfilterPipe implements PipeTransform {

  // transform(value: unknown, args: unknown[]): unknown {
  //   console.log(value); 
    
    
  //   if(!args) {
  //     return value;
  //   }

  //   //  return value.filter(item =>{
  //     //  console.log(item);
  //     //  item.LkupIndustry.Name.indexOf(args) > -1}); 
  //   return null;
  // }

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
