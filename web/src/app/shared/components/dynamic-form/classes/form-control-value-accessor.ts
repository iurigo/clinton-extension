// Angular
import { ControlValueAccessor } from '@angular/forms';
// Interfaces
import { FormItem } from '../dynamic-form.interfaces';


export class FormControlValueAccessor implements ControlValueAccessor {

  public item: FormItem = null;
  public onChange: Function = () => { };
  public onTouched: Function = () => { };


  // [ Interface implementation ]
  public writeValue(value: any): void {
    if (this.item.value === value) { return; }

    this.item.value = value;
    this.onChange(this.item.value);
    this.onTouched();
  }

  public registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  public registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  public setDisabledState(isDisabled: boolean): void {
    this.item.isReadOnly = isDisabled;
  }
}