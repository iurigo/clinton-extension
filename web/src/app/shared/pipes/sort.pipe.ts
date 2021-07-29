// Angular
import { Pipe, PipeTransform } from '@angular/core';


@Pipe({
  name: 'sort'
})
export class SortPipe implements PipeTransform {

  public transform(array: any[], property: string, type: 'string'): any[] {
    switch (type) {
      case 'string': return array.sort((a, b) => (<string>a[property]).localeCompare((<string>b[property])));
      default: return [];
    }
  }

}
