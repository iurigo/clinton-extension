// Angular
import { Component, forwardRef, Input, Output, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
// Classes
import { FormControlValueAccessor } from '../../classes/form-control-value-accessor';
// Interfaces
import { FormItemNumber, FormItemActionEvent, FormItemExtrasEvent } from '../../dynamic-form.interfaces';


@Component({
  selector: 'app-form-number',
  templateUrl: './form-number.component.html',
  styleUrls: ['./form-number.component.scss', '../../styles/dynamic-form-item.scss'],
  providers: [
    { provide: NG_VALUE_ACCESSOR, multi: true, useExisting: forwardRef(() => FormNumberComponent) }
  ]
})
export class FormNumberComponent extends FormControlValueAccessor {

  @Input() public item: FormItemNumber = null;
  @Output() public onEnterKeyPress = new EventEmitter<FormItemNumber>();
  @Output() public onAction = new EventEmitter<FormItemActionEvent<any>>();
  @Output() public onExtras = new EventEmitter<FormItemExtrasEvent<any>>();

  constructor() { super(); }


  // [ Template functions ]
  public getNumberMinValue(item: FormItemNumber): number {
    return isNaN(item.min) ? null : item.min;
  }

  public getNumberMaxValue(item: FormItemNumber): number {
    return isNaN(item.max) ? null : item.max;
  }
}
