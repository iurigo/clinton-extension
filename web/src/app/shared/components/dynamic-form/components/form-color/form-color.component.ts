// Angular
import { Component, Input, EventEmitter, Output, forwardRef, OnChanges } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
// Kendo UI
import { PaletteSettings } from '@progress/kendo-angular-inputs';
// Classes
import { FormControlValueAccessor } from '../../classes/form-control-value-accessor';
// Interfaces
import { FormItemColor, FormItemActionEvent, FormItemExtrasEvent } from '../../dynamic-form.interfaces';


@Component({
  selector: 'app-form-color',
  templateUrl: './form-color.component.html',
  styleUrls: ['./form-color.component.scss'],
  providers: [
    { provide: NG_VALUE_ACCESSOR, multi: true, useExisting: forwardRef(() => FormColorComponent) }
  ]
})
export class FormColorComponent extends FormControlValueAccessor implements OnChanges {
  public view: 'gradient' | 'palette';
  public paletteSettings: PaletteSettings = {};

  @Input() public item: FormItemColor = null;
  @Output() public onAction = new EventEmitter<FormItemActionEvent<any>>();
  @Output() public onExtras = new EventEmitter<FormItemExtrasEvent<any>>();

  constructor() { super(); }

  ngOnChanges() {
    // Set view
    this.view = this.item.colors?.length ? 'palette' : 'gradient';
    // Set colors
    if (this.view === 'palette') {
      this.paletteSettings.palette = this.item.colors;
    }
  }
}
