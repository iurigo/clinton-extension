// Angular
import { Component, Input } from '@angular/core';
// Interfaces
import { FormItem } from '../../dynamic-form.interfaces';


@Component({
  selector: 'app-form-label',
  templateUrl: './form-label.component.html',
  styleUrls: ['./form-label.component.scss', '../../styles/dynamic-form-item.scss']
})
export class FormLabelComponent {
  @Input() public item: FormItem = null;
}
