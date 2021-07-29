// Angular
import { Component, Input, forwardRef, Output, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
// Classes
import { FormControlValueAccessor } from '../../classes/form-control-value-accessor';
// Interfaces
import { FormItemDateTime, FormItemActionEvent, FormItemExtrasEvent } from '../../dynamic-form.interfaces';


@Component({
  selector: 'app-form-date',
  templateUrl: './form-date.component.html',
  styleUrls: ['./form-date.component.scss', '../../styles/dynamic-form-item.scss'],
  providers: [
    { provide: NG_VALUE_ACCESSOR, multi: true, useExisting: forwardRef(() => FormDateComponent) }
  ]
})
export class FormDateComponent extends FormControlValueAccessor {

  @Input() public item: FormItemDateTime = null;
  @Output() public onAction = new EventEmitter<FormItemActionEvent<any>>();
  @Output() public onExtras = new EventEmitter<FormItemExtrasEvent<any>>();

  constructor() { super(); }
}
