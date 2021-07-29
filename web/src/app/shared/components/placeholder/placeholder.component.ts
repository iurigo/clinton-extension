// Angular
import { Component, Input } from '@angular/core';


@Component({
  selector: 'app-placeholder',
  templateUrl: './placeholder.component.html',
  styleUrls: ['./placeholder.component.scss']
})
export class PlaceholderComponent {
  
  @Input() public iconSize: string = '128px';
}
