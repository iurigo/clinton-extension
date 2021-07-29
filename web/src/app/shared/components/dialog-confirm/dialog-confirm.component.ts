// Angular
import { Component } from '@angular/core';
// KendoUI
import { DialogRef } from '@progress/kendo-angular-dialog';
import { TextAlign } from './dialog-confirm.enums';


@Component({
  selector: 'app-dialog-confirm',
  templateUrl: './dialog-confirm.component.html',
  styleUrls: ['./dialog-confirm.component.scss']
})
export class DialogConfirmComponent {

  public message: string = null;
  public messageAlignment: TextAlign = null;
  public dialogInstance: DialogRef = null;
  public yesActionName: string = 'Yes';
  public NoActionName: string = 'No';
  public CancelActionName: string = null;


  public onActionClick(result: boolean): void {
    this.dialogInstance.close({ result });
  }

  public getMessageClass(): string {
    return this.messageAlignment === TextAlign.CENTER ? 'align-center' : this.messageAlignment === TextAlign.RIGHT ? 'align-right' : '';
  }
}
