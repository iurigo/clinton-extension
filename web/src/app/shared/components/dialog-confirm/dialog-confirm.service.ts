// Angular
import { Injectable } from '@angular/core';
// KendoUI
import { DialogService } from '@progress/kendo-angular-dialog';
// Components
import { DialogConfirmComponent } from './dialog-confirm.component';
// Enums
import { TextAlign } from './dialog-confirm.enums';

@Injectable({
  providedIn: 'root'
})
export class DialogConfirmService {

  constructor(private _dialog: DialogService) { }

  public show(
    title: string,
    message: string,
    actions: string[] = ['Yes', 'No', null],
    textAlign: TextAlign = TextAlign.LEFT
  ): Promise<boolean> {
    return new Promise(resolve => {
      // Generate form popup
      const dialog = this._dialog.open({
        title: title,
        content: DialogConfirmComponent
      });

      // Fill the dialog
      const dialogForm: DialogConfirmComponent = dialog.content.instance;
      dialogForm.message = message;
      dialogForm.dialogInstance = dialog;
      dialogForm.yesActionName = actions[0];
      dialogForm.NoActionName = actions[1];
      dialogForm.CancelActionName = actions[2];
      dialogForm.messageAlignment = textAlign;

      // Subscribe to actions
      dialog.result.subscribe((e: { result: boolean }) => {
        resolve(e.result);
      });
    });
  }
}
