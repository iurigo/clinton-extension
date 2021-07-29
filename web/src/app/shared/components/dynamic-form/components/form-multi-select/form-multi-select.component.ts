// Angular
import { Component, forwardRef, Input, Output, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { PreventableEvent } from '@progress/kendo-angular-dropdowns';
// Classes
import { FormControlValueAccessor } from '../../classes/form-control-value-accessor';
// Interfaces
import { FormItemMultiSelect, FormItemActionEvent, FormItemExtrasEvent, FormItemOption } from '../../dynamic-form.interfaces';
// Constants
import { ACTION_EMPTY_SPACE_CLICK, ACTION_ITEM_CLICK, ACTION_ITEM_DELETE, ACTION_POPUP_OPEN } from './form-multi-select.consts';

@Component({
  selector: 'app-form-multi-select',
  templateUrl: './form-multi-select.component.html',
  styleUrls: ['./form-multi-select.component.scss', '../../styles/dynamic-form-item.scss'],
  providers: [
    { provide: NG_VALUE_ACCESSOR, multi: true, useExisting: forwardRef(() => FormMultiSelectComponent) }
  ]
})
export class FormMultiSelectComponent extends FormControlValueAccessor {

  @Input() public item: FormItemMultiSelect = null;
  @Output() public onAction = new EventEmitter<FormItemActionEvent<any>>();
  @Output() public onExtras = new EventEmitter<FormItemExtrasEvent<any>>();

  constructor() { super(); }

  public onClick(e: any): void {
    if (e.target.className === 'ng-star-inserted') {
      const index = this.getIndex(e.target.parentNode);
      if (index !== -1) {
        this.onAction.emit({ action: ACTION_ITEM_CLICK, item: this.item, e: this.item.value[index] });
      }
    } else if (!(<string>e.target.className).includes('k-i-close')) {
      this.onAction.emit({ action: ACTION_EMPTY_SPACE_CLICK, item: this.item });
    }
  }

  public onPopupOpen(e: PreventableEvent): void {
    if (this.item.hideOptions) { e.preventDefault(); }
    this.onAction.emit({ action: ACTION_POPUP_OPEN, item: this.item });
  }

  public onRemoveTag(e: { dataItem: FormItemOption }): void {
    this.onAction.emit({ action: ACTION_ITEM_DELETE, item: this.item, e: e.dataItem.id });
  }

  private getIndex(node): number {
    var childs = node.parentNode.childNodes;
    for (let i = 0; i < childs.length; i++) {
      if (node == childs[i]) return i;
    }
    return -1;
  }
}
