// Angular
import { Component, Input, Output, EventEmitter } from '@angular/core';


@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent {
  @Input() public search: string = '';
  @Input() public placeholder: string = 'Search';
  @Output() public onSearch = new EventEmitter<string>();
}
