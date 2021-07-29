// Angular
import { Component, OnInit, ViewChild, Input, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
// KendoUI
import { DialogRef } from '@progress/kendo-angular-dialog';
// Components
import { DynamicFormComponent } from '../../components/dynamic-form/dynamic-form.component';
// Interfaces
import { FormItem, FormItemActionEvent } from '../../components/dynamic-form/dynamic-form.interfaces';
import { DialogFormOptions } from './dialog-form.interfaces';


@Component({
  selector: 'app-dialog-form',
  templateUrl: './dialog-form.component.html',
  styleUrls: ['./dialog-form.component.scss']
})
export class DialogFormComponent implements OnInit {

  @ViewChild(DynamicFormComponent, { static: true }) public form: DynamicFormComponent;

  @Output() public onValueChange = new EventEmitter<FormItem>();
  @Output() public onAction = new EventEmitter<FormItemActionEvent<any>>();

  @Input() public dialogInstance: DialogRef = null;
  @Input() public items: FormItem[] = [];
  @Input() public options: DialogFormOptions = null;

  public ngOnInit() {
    if (this.options == null) { this.options = {}; }
    this.options.actionName = this.options.actionName || 'OK';
    this.options.cancelName = this.options.cancelName || 'Cancel';
    this.options.isChangeRequired = this.options.isChangeRequired || false;
  }

  // [ Template functions ]
  public isDisabled(): boolean {
    return this.form.form.invalid || (this.options.isChangeRequired && this.form.form.pristine);
  }

  public onOkClick(): void {
    this.dialogInstance.close(this.form.form.value);
  }

  public onCancelClick(): void {
    this.dialogInstance.close({});
  }
}
