// Angular
import { Pipe, PipeTransform } from '@angular/core';


@Pipe({
  name: 'filter'
})
export class FilterPipe implements PipeTransform {

  public transform(array: any[], property: string, operator: 'isnotnull' | 'equal', value: any): any[] {
    switch (operator) {
      case 'isnotnull': return array.filter(a => a[property] != null);
      case 'equal': return array.filter(a => a[property] == value);
      default: return [];
    }
  }

}
