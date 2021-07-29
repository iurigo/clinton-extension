// Angular
import { Component, Input, OnInit } from '@angular/core';
// Kendo
import { WindowRef } from '@progress/kendo-angular-dialog';
// Interfaces
import { FormItem, FormItemBoolean, FormItemDropDown, FormItemNumber, FormItemOption, FormItemText } from 'src/app/shared/components/dynamic-form/dynamic-form.interfaces';
import { ActionItem } from 'src/app/shared/components/menu-button/menu-button.interfaces';
import { EmployeeDialog, EmployeeNew } from '../../employee.interface';
// Services
import { NewEmployeesDialogService } from './new-employees-dialog.service';
import { DialogFormService } from 'src/app/shared/components/dialog-form/dialog-form.service';
import { DialogConfirmService } from 'src/app/shared/components/dialog-confirm/dialog-confirm.service';
import { FormItemType } from 'src/app/shared/components/dynamic-form/dynamic-form.enums';
import { Validators } from '@angular/forms';
import { NotifyService } from 'src/app/services/notify.service';
import { EmployeeDiscipline } from './new-employees.enum';
// Consts
const MENU_UPDATE = 'update';
const MENU_DELETE = 'delete';

@Component({
  selector: 'app-new-employees-dialog',
  templateUrl: './new-employees-dialog.component.html',
  styleUrls: ['./new-employees-dialog.component.scss'],
  providers: [NewEmployeesDialogService]
})
export class NewEmployeesDialogComponent implements OnInit {

  @Input() public newEmployeesData: EmployeeNew[] = [];

  public totalNumberOfAssets: number = 0;
  public isLoading: boolean = false;
  public loadingMessage: string = 'Loading Employees...';
  public newEmployeeActions: ActionItem[] = [];
  public disciplineOptions: FormItemOption[] = [];
  public selectedEmployeeIds: number[] = [];

  constructor(
    private _service: NewEmployeesDialogService,
    private _dialog: WindowRef,
    private _dialogForm: DialogFormService,
    private _dialogConfirm: DialogConfirmService,
    private _notify: NotifyService
  ) { }

  public async ngOnInit() {
    this.totalNumberOfAssets = this.newEmployeesData.length;

    // Get employee grid menu items
    this.newEmployeeActions = this.getGridActions();

    // Get discipline options
    this.disciplineOptions = this.getDisciplineOptions();
  }

  public async onSubmitNewEmployees(): Promise<void> {
    this.isLoading = true;
    // Create employees
    await this._service.createMultipleEmployees(this.newEmployeesData).toPromise();
    this.isLoading = false;
    this._dialog.close({ positive: true });
  }

  private getGridActions(): ActionItem[] {
    return [
      { id: MENU_UPDATE, icon: 'k-icon k-i-edit', name: 'Update' },
      { id: MENU_DELETE, icon: 'k-icon k-i-delete', name: 'Delete' }
    ];
  }

  public async onEmployeeAction(e, employee: EmployeeNew): Promise<void> {
    switch (e.id) {
      case MENU_UPDATE: await this.onEmployeeUpdate(employee); break;
      case MENU_DELETE: await this.onEmployeeDelete(employee); break;
    }
  }

  // [ Internal ]
  private async onEmployeeUpdate(employee: EmployeeNew): Promise<void> {
    // Get dialog items
    const dialogItems = this.getEmployeeDialogItems();
    dialogItems.find(i => i.field === 'employeeId').value = `${employee.employeeId}`;
    dialogItems.find(i => i.field === 'firstName').value = employee.firstName;
    dialogItems.find(i => i.field === 'lastName').value = employee.lastName;
    dialogItems.find(i => i.field === 'discipline').value = employee.discipline;
    dialogItems.find(i => i.field === 'rate').value = employee.rate;
    dialogItems.find(i => i.field === 'isActive').value = employee.isActive;
    // Get dialog result
    const dialogResult = await this._dialogForm.show<EmployeeDialog>('Update Employee', dialogItems, { actionName: 'Update' });

    if (dialogResult.positive) {
      // Create an update employee model
      const model = dialogResult.model;
      const existingEmployee: EmployeeNew = {
        employeeId: parseInt(model.employeeId),
        firstName: model.firstName,
        lastName: model.lastName,
        discipline: model.discipline,
        rate: model.rate,
        isActive: model.isActive,
      };

      try {
        this.isLoading = true;
        // Update employee
        this.newEmployeesData = this.newEmployeesData.map(e => (e.employeeId === existingEmployee.employeeId ? existingEmployee : e));
      } finally {
        this.isLoading = false;
      }
    }
  }

  public async onUpdateMultiple(): Promise<void> {

    const dialogItems = this.getEmployeesUpdateDialogItems();

    // Show dialog
    const dialogResult = await this._dialogForm.show<any>('Update Employees ', dialogItems, { actionName: 'Update' }, async (item, form) => {
      if (item.field === 'field') {
        // Load new property fields based on selected type
        form.items = this.getEmployeesUpdateDialogItems(item.value);
      }
    });

    if (dialogResult.positive) {
      // Create an update employee model
      const model = dialogResult.model;
      try {
        this.isLoading = true;
        for (let i = 0; i < this.selectedEmployeeIds.length; i++) {
          this.newEmployeesData = this.newEmployeesData.map(e =>
            e.employeeId === this.selectedEmployeeIds[i]
              ? {
                ...e,
                discipline: model.field === 1 ? model.discipline : e.discipline,
                rate: model.field === 2 ? model.rate : e.rate,
                isActive: model.field === 3 ? model.isActive : e.isActive
              }
              : e
          );
        }
      } finally {
        this.selectedEmployeeIds = [];
        this.isLoading = false;
      }
    }
  }

  public async onEmployeeDelete(employee: EmployeeNew): Promise<void> {
    const ok = await this._dialogConfirm.show(`Delete "${employee.firstName} ${employee.lastName}"`, 'Are you sure?');
    if (ok) {
      try {
        this.isLoading = true;
        // Delete employee
        this.newEmployeesData = this.newEmployeesData.filter(e => e.employeeId !== employee.employeeId);
        // Update total number of employees
        this.totalNumberOfAssets = this.newEmployeesData.length;
      } finally {
        this.isLoading = false;
      }
    }
  }

  private getEmployeesUpdateDialogItems(field?: number): FormItem[] {
    let formItems: FormItem[] = [
      <FormItemDropDown>{ type: FormItemType.DROPDOWN, field: 'field', title: 'Select field to update', options: this.getEmployeeFieldOptions(), isRequired: true, value: field }
    ];
    if (field == null) { return formItems; }
    switch (field) {
      case 1: return [...formItems, <FormItemDropDown>{ type: FormItemType.DROPDOWN, field: 'discipline', title: 'Discipline', options: this.getDisciplineOptions(), isRequired: true }]
      case 2: return [...formItems, <FormItemNumber>{ type: FormItemType.NUMBER, field: 'rate', title: 'Rate', isRequired: true, validators: [Validators.min(1)] }]
      case 3: return [...formItems, <FormItemBoolean>{ type: FormItemType.BOOLEAN, field: 'isActive', title: 'Is Active', isRequired: true }]
    }
  }

  private getEmployeeFieldOptions(): FormItemOption[] {
    return [
      { id: 1, value: 'Discipline' },
      { id: 2, value: 'Rate' },
      { id: 3, value: 'Active' }
    ]
  }

  private getEmployeeDialogItems(): FormItem[] {
    const formItems: FormItem[] = [
      <FormItemText>{ type: FormItemType.TEXT, field: 'employeeId', title: 'Employee Id', isRequired: true, focus: true, validators: [Validators.min(1), Validators.pattern('^[0-9]*$')] },
      <FormItemText>{ type: FormItemType.TEXT, field: 'firstName', title: 'First Name', isRequired: true },
      <FormItemText>{ type: FormItemType.TEXT, field: 'lastName', title: 'Last Name', isRequired: true },
      <FormItemDropDown>{ type: FormItemType.DROPDOWN, field: 'discipline', title: 'Discipline', options: this.disciplineOptions, isRequired: true },
      <FormItemNumber>{ type: FormItemType.NUMBER, field: 'rate', title: 'Rate', isRequired: true, validators: [Validators.min(1)] },
      <FormItemBoolean>{ type: FormItemType.BOOLEAN, field: 'isActive', title: 'Is Active', isRequired: true }
    ];
    return formItems;
  }

  private getDisciplineOptions(): FormItemOption[] {
    return [
      { id: 1, value: 'PA' },
      { id: 2, value: 'PCA' },
      { id: 3, value: 'HHA' },
      { id: 4, value: 'PCA | HHA' }
    ];
  }

  public getDisciplineName(discipline: number): string {
    return this.disciplineOptions.find(o => o.id === discipline).value;
  }
}
