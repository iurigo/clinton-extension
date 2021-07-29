import { Component } from '@angular/core';
import { DialogRef } from '@progress/kendo-angular-dialog';

@Component({
  selector: 'app-warning-dialog',
  templateUrl: './warning-dialog.component.html',
  styleUrls: ['./warning-dialog.component.scss']
})
export class WarningDialogComponent {

  public title: string;
  public message: string;
  public primary: string;
  public secondary: string;

  public dialog: DialogRef;

  public onConfirm(): void {
    this.dialog.close({ success: true });
  }

  public onCancel(): void {
    this.dialog.close({ success: false });
  }

}
