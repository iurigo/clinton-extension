// Angular
import { Component, Input, Output, EventEmitter } from '@angular/core';
// Interfaces
import { FormItem, FormItemActionEvent } from '../../dynamic-form.interfaces';


@Component({
  selector: 'app-form-actions',
  templateUrl: './form-actions.component.html',
  styleUrls: ['./form-actions.component.scss']
})
export class FormActionsComponent {
  @Input() public item: FormItem = null;
  @Output() public onAction = new EventEmitter<FormItemActionEvent<any>>();
}