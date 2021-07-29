// Angular
import { Component, forwardRef, Input, Output, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
// Classes
import { FormControlValueAccessor } from '../../classes/form-control-value-accessor';
// Interfaces
import { FormItemUrl, FormItemActionEvent, FormItemExtrasEvent } from '../../dynamic-form.interfaces';


@Component({
  selector: 'app-form-url',
  templateUrl: './form-url.component.html',
  styleUrls: ['./form-url.component.scss', '../../styles/dynamic-form-item.scss'],
  providers: [
    { provide: NG_VALUE_ACCESSOR, multi: true, useExisting: forwardRef(() => FormUrlComponent) }
  ]
})
export class FormUrlComponent extends FormControlValueAccessor {

  @Input() public item: FormItemUrl = null;
  @Output() public onEnterKeyPress = new EventEmitter<FormItemUrl>();
  @Output() public onAction = new EventEmitter<FormItemActionEvent<any>>();
  @Output() public onExtras = new EventEmitter<FormItemExtrasEvent<any>>();

  constructor() { super(); }


  // [ Template functions ]
  public onAltViewClick(): void {
    this.item.focus = true;
    this.item.altView = !this.item.altView;
  }
}
