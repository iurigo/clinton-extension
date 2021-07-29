// Angular
import { Component, forwardRef, Input, Output, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
// Classes
import { FormControlValueAccessor } from '../../classes/form-control-value-accessor';
// Interfaces
import { FormItemBoolean, FormItemActionEvent, FormItemExtrasEvent } from '../../dynamic-form.interfaces';


@Component({
  selector: 'app-form-boolean',
  templateUrl: './form-boolean.component.html',
  styleUrls: ['./form-boolean.component.scss', '../../styles/dynamic-form-item.scss'],
  providers: [
    { provide: NG_VALUE_ACCESSOR, multi: true, useExisting: forwardRef(() => FormBooleanComponent) }
  ]
})
export class FormBooleanComponent extends FormControlValueAccessor {

  @Input() public item: FormItemBoolean = null;
  @Output() public onAction = new EventEmitter<FormItemActionEvent<any>>();
  @Output() public onExtras = new EventEmitter<FormItemExtrasEvent<any>>();

  constructor() { super(); }


  // [ Override default behavior]
  public writeValue(value: any): void {
    this.item.value = value;
    this.onChange(value);
    this.onTouched();
  }
}
