// Angular
import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
// Interfaces
import { FormItem } from '../../dynamic-form.interfaces';
import { FormItemType } from '../../dynamic-form.enums';


@Component({
  selector: 'app-form-extras',
  templateUrl: './form-extras.component.html',
  styleUrls: ['./form-extras.component.scss']
})
export class FormExtrasComponent implements OnInit {

  @Input() public item: FormItem = null; // Root item. Use item.extras to get extras information
  @Output() public onExtras = new EventEmitter<any>();

  public get itemType() { return FormItemType; }
  public showLeftLabel: boolean = false;
  public showRightLabel: boolean = false;
  public containerWidth: string = 'initial';

  public ngOnInit(): void {
    // Make sure that form item type is compatible with the extras
    const implementedTypes: FormItemType[] = [FormItemType.BOOLEAN, FormItemType.SWITCH, FormItemType.DROPDOWN];
    if (!implementedTypes.some(t => t === this.item.extras.type)) {
      throw new Error(`[${FormItemType[this.item.extras.type]}] FormItemExtras type is not implemented.`);
    }
    // Configure labels
    this.showLeftLabel = !!this.item.extras.title && this.item.extras.titlePosition === 'left';
    this.showRightLabel = !!this.item.extras.title && this.item.extras.titlePosition !== 'left';
    // Configure width
    if (!!this.item.extras.width) {
      this.containerWidth = `${this.item.extras.width}px`;
    }
  }


  // [ Template functions ]
  public onValueChange(item: FormItem, value: any): void {
    item.extras.value = value;
    this.onExtras.emit(item);
  }
}
