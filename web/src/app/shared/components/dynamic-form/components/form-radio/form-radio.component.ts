// Angular
import { Component, forwardRef, Input, Output, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
// Classes
import { FormControlValueAccessor } from '../../classes/form-control-value-accessor';
// Interfaces
import { FormItemRadio, FormItemOption, FormItemActionEvent, FormItemExtrasEvent } from '../../dynamic-form.interfaces';


@Component({
  selector: 'app-form-radio',
  templateUrl: './form-radio.component.html',
  styleUrls: ['./form-radio.component.scss', '../../styles/dynamic-form-item.scss'],
  providers: [
    { provide: NG_VALUE_ACCESSOR, multi: true, useExisting: forwardRef(() => FormRadioComponent) }
  ]
})
export class FormRadioComponent extends FormControlValueAccessor {

  @Input() public item: FormItemRadio = null;
  @Output() public onAction = new EventEmitter<FormItemActionEvent<any>>();
  @Output() public onExtras = new EventEmitter<FormItemExtrasEvent<any>>();

  constructor() { super(); }


  // [ Template functions ]
  public getRadioOptionId(option: FormItemOption): string {
    return `${this.item.field}_${option.id}`;
  }
}
