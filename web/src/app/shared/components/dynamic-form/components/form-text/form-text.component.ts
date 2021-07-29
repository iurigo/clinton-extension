// Angular
import { Component, Input, forwardRef, Output, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
// Classes
import { FormControlValueAccessor } from '../../classes/form-control-value-accessor';
// Interfaces
import { FormItemText, FormItemActionEvent, FormItemExtrasEvent } from '../../dynamic-form.interfaces';

@Component({
  selector: 'app-form-text',
  templateUrl: './form-text.component.html',
  styleUrls: ['./form-text.component.scss', '../../styles/dynamic-form-item.scss'],
  providers: [
    { provide: NG_VALUE_ACCESSOR, multi: true, useExisting: forwardRef(() => FormTextComponent) }
  ]
})
export class FormTextComponent extends FormControlValueAccessor {

  @Input() public item: FormItemText = null;
  @Output() public onEnterKeyPress = new EventEmitter<FormItemText>();
  @Output() public onAction = new EventEmitter<FormItemActionEvent<any>>();
  @Output() public onExtras = new EventEmitter<FormItemExtrasEvent<any>>();

  constructor() { super(); }
}
