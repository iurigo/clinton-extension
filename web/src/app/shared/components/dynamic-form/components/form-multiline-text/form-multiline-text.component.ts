// Angular
import { Component, forwardRef, Input, Output, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
// Classes
import { FormControlValueAccessor } from '../../classes/form-control-value-accessor';
// Interfaces
import { FormItemMultilineText, FormItemActionEvent, FormItemExtrasEvent } from '../../dynamic-form.interfaces';


@Component({
  selector: 'app-form-multiline-text',
  templateUrl: './form-multiline-text.component.html',
  styleUrls: ['./form-multiline-text.component.scss', '../../styles/dynamic-form-item.scss'],
  providers: [
    { provide: NG_VALUE_ACCESSOR, multi: true, useExisting: forwardRef(() => FormMultilineTextComponent) }
  ]
})
export class FormMultilineTextComponent extends FormControlValueAccessor {

  @Input() public item: FormItemMultilineText = null;
  @Output() public onAction = new EventEmitter<FormItemActionEvent<any>>();
  @Output() public onExtras = new EventEmitter<FormItemExtrasEvent<any>>();

  constructor() { super(); }
}
