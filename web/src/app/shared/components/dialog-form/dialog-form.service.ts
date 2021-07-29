// Angular
import { Injectable } from '@angular/core';
import { Subscription } from 'rxjs';
// KendoUI
import { DialogService } from '@progress/kendo-angular-dialog';
// Components
import { DialogFormComponent } from './dialog-form.component';
// Interfaces
import { FormItem, FormItemActionEvent } from '../../components/dynamic-form/dynamic-form.interfaces';
import { DialogFormOptions } from './dialog-form.interfaces';
import { DialogResult } from '../../interfaces/dialog-form-result.interface';


@Injectable({
  providedIn: 'root'
})
export class DialogFormService {

  constructor(private _dialog: DialogService) { }

  public show<T>(
    title: string,
    formItems: FormItem[],
    options?: DialogFormOptions,
    onValueChange?: (item: FormItem, form: DialogFormComponent) => void,
    onAction?: (e: FormItemActionEvent<any>, form: DialogFormComponent) => void
  ): Promise<DialogResult<T>> {
    return new Promise(resolve => {
      // Generate form popup
      const dialog = this._dialog.open({
        title: title,
        maxWidth: '95vw',
        content: DialogFormComponent
      });
      
      // Initialize the dialog component
      const dialogForm: DialogFormComponent = dialog.content.instance;
      dialogForm.items = formItems;
      dialogForm.dialogInstance = dialog;
      dialogForm.options = options;
      
      // Listen to dialog events
      const subscriptions$ = new Subscription();

      // Close dialog event
      subscriptions$.add(dialog.result.subscribe((result: any) => {
        subscriptions$.unsubscribe();
        if (Object.keys(result).length > 0) {
          resolve({ positive: true, model: result });
        } else {
          resolve({ positive: false, model: null });
        }
      }));
      
      // Value change event
      if (!!onValueChange) {
        subscriptions$.add(dialogForm.onValueChange.subscribe((item: FormItem) => onValueChange(item, dialogForm)));
      }

      // Action event
      if (!!onAction) {
        subscriptions$.add(dialogForm.onAction.subscribe((e: FormItemActionEvent<any>) => onAction(e, dialogForm)));
      }
    });

  }
}
