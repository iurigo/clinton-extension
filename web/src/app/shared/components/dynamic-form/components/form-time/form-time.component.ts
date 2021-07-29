// Angular
import { Component, forwardRef, Input, Output, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
// Classes
import { FormControlValueAccessor } from '../../classes/form-control-value-accessor';
// Interfaces
import { FormItemDateTime, FormItemActionEvent, FormItemExtrasEvent } from '../../dynamic-form.interfaces';


@Component({
  selector: 'app-form-time',
  templateUrl: './form-time.component.html',
  styleUrls: ['./form-time.component.scss', '../../styles/dynamic-form-item.scss'],
  providers: [
    { provide: NG_VALUE_ACCESSOR, multi: true, useExisting: forwardRef(() => FormTimeComponent) }
  ]
})
export class FormTimeComponent extends FormControlValueAccessor {

  @Input() public item: FormItemDateTime = null;
  @Output() public onFormSubmit = new EventEmitter<void>();
  @Output() public onAction = new EventEmitter<FormItemActionEvent<any>>();
  @Output() public onExtras = new EventEmitter<FormItemExtrasEvent<any>>();

  constructor() { super(); }
}
