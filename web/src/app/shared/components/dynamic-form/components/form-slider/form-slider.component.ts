// Angular
import { Component, forwardRef, Input, Output, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
// Classes
import { FormControlValueAccessor } from '../../classes/form-control-value-accessor';
// Interfaces
import { FormItemSlider, FormItemActionEvent, FormItemExtrasEvent } from '../../dynamic-form.interfaces';


@Component({
  selector: 'app-form-slider',
  templateUrl: './form-slider.component.html',
  styleUrls: ['./form-slider.component.scss', '../../styles/dynamic-form-item.scss'],
  providers: [
    { provide: NG_VALUE_ACCESSOR, multi: true, useExisting: forwardRef(() => FormSliderComponent) }
  ]
})
export class FormSliderComponent extends FormControlValueAccessor {

  @Input() public item: FormItemSlider = null;
  @Output() public onEnterKeyPress = new EventEmitter<FormItemSlider>();
  @Output() public onAction = new EventEmitter<FormItemActionEvent<any>>();
  @Output() public onExtras = new EventEmitter<FormItemExtrasEvent<any>>();

  constructor() { super(); }


  // [ Template functions ]
  public getNumberMinValue(item: FormItemSlider): number {
    return item.min || 0;
  }

  public getNumberMaxValue(item: FormItemSlider): number {
    return item.max || 0;
  }

  public getSliderSmallStep(item: FormItemSlider): number {
    return Math.max(Math.ceil((item.max - item.min) / 10), 1);
  }
}
