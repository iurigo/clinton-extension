// Angular
import { Component, Input } from '@angular/core';
// Enums
import { FormItemAlignment } from '../../dynamic-form.enums';
// Interfaces
import { FormItemSeparator } from '../../dynamic-form.interfaces';


@Component({
  selector: 'app-form-separator',
  templateUrl: './form-separator.component.html',
  styleUrls: ['./form-separator.component.scss']
})
export class FormSeparatorComponent {

  @Input() public item: FormItemSeparator = null;
  @Input() public labelStyle: FormItemSeparator = null;

  public get align() { return FormItemAlignment; }
}
