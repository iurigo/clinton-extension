// Angular
import { Injectable } from '@angular/core';
import { WindowRef, WindowService } from '@progress/kendo-angular-dialog';
import { merge, Subscription } from 'rxjs';
import { UtilitiesService } from 'src/app/services/utilities.service';
import { DialogResult } from 'src/app/shared/interfaces/dialog-form-result.interface';
import { EmployeeNew } from '../../employee.interface';
import { NewEmployeesDialogComponent } from './new-employees-dialog.component';

@Injectable({
  providedIn: 'root'
})
export class DialogService {

  constructor(
    private _kendoWindow: WindowService,
    private _utils: UtilitiesService
  ) { }

  public async showWindow(employees: EmployeeNew[]): Promise<DialogResult<void>> {
    const subscriptions$ = new Subscription();
    
    return new Promise((resolve) => {
      
      const windowRef: WindowRef = this._kendoWindow.open({
        title: 'New Employees',
        content: NewEmployeesDialogComponent,
        width: Math.min(window.innerWidth * 0.95, 680),
        height: Math.min(window.innerHeight * 0.95, 720),
        minWidth: 450,
        minHeight: 370,
      });

      const component = <NewEmployeesDialogComponent>windowRef.content.instance;
      component.newEmployeesData = employees;

      // Prevent window from disappearing off screen when dragging or resizing 
      subscriptions$.add((merge(
        windowRef.window.instance.dragEnd,
        windowRef.window.instance.resizeEnd
      )).subscribe(() => this._utils.keepWindowVisible(windowRef)));

      // return window result
      subscriptions$.add(windowRef.result.subscribe((res: DialogResult<void>) => {
        subscriptions$.unsubscribe();
        resolve(res);
      }));
    });
  }
}