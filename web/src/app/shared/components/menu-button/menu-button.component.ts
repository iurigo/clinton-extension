// Angular
import { Component, Input, Output, EventEmitter } from '@angular/core';
// Interfaces
import { ActionItem } from './menu-button.interfaces';


@Component({
  selector: 'app-menu-button',
  templateUrl: './menu-button.component.html',
  styleUrls: ['./menu-button.component.scss']
})
export class MenuButtonComponent {
  @Input() public actions: ActionItem[] = [];
  @Input() public disabled: boolean = false;
  @Output() public onAction = new EventEmitter<ActionItem>();
}
