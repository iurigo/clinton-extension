// Angular
import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Validators } from '@angular/forms';
// RxJS
import { Subject } from 'rxjs';
import { bufferCount, filter, map, min } from 'rxjs/operators';
// Kendo
import { State } from '@progress/kendo-data-query';
import { CellClickEvent, DataStateChangeEvent, GridDataResult } from '@progress/kendo-angular-grid';
// Services
import { EmployeesService } from './employees.service';
import { ApiService } from 'src/app/services/api.service';
import { NotifyService } from 'src/app/services/notify.service';
import { UtilitiesService } from 'src/app/services/utilities.service';
import { DialogFormService } from 'src/app/shared/components/dialog-form/dialog-form.service';
import { DialogConfirmService } from 'src/app/shared/components/dialog-confirm/dialog-confirm.service';
import { DialogService as NewEmployeesDialog } from 'src/app/views/main/views/employees/dialogs/new-employee-dialog/dialog.service';
// Interfaces
import { ActionItem } from 'src/app/shared/components/menu-button/menu-button.interfaces';
import { Employee, EmployeeDialog, EmployeeNew, EmployeeUpdate } from './employee.interface';
import { FormItem, FormItemBoolean, FormItemDropDown, FormItemNumber, FormItemOption, FormItemText } from 'src/app/shared/components/dynamic-form/dynamic-form.interfaces';
// Enums
import { FormItemType } from 'src/app/shared/components/dynamic-form/dynamic-form.enums';
// Consts
const MENU_UPDATE = 'update';
const MENU_DELETE = 'delete';

@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.scss']
})
export class EmployeesComponent implements OnInit, OnDestroy {

  @ViewChild('fileUploadInput', { static: true }) public fileInput: ElementRef<HTMLInputElement>;

  public gridData: GridDataResult;
  public state: State = { skip: 0, take: 25, sort: [{ field: 'lastName', dir: 'asc' }], filter: null };
  public selectedEmployeeIds: number[] = [];
  public employeeActions: ActionItem[] = [];
  public isBusy: boolean = true;

  public searchText: string = '';

  public gridClick$ = new Subject<CellClickEvent>();

  public disciplineOptions: FormItemOption[] = [];
  public disciplineFilterOptions: FormItemOption[] = [];

  constructor(
    private _api: ApiService,
    private _service: EmployeesService,
    private _notify: NotifyService,
    private _utils: UtilitiesService,
    private _dialogForm: DialogFormService,
    private _dialogConfirm: DialogConfirmService,
    private _dialogNewEmployee: NewEmployeesDialog
  ) { }

  public async ngOnInit() {

    // Grid double click custom implementation
    this.onGridDoubleClick();

    // Get employee grid menu items
    this.employeeActions = this.getGridActions();

    // Get discipline options
    this.disciplineOptions = this.getDisciplineOptions();
    this.disciplineFilterOptions = this.getDisciplineFilterOptions();

    // Get the list of employees
    await this.loadEmployees();
    this.isBusy = false;
  }

  public async ngOnDestroy() {
    this.gridClick$.unsubscribe();
  }

  // [ Template functions ]
  public async gridStateChange(e: DataStateChangeEvent): Promise<void> {
    this.state = e;
    await this.loadEmployees();
  }

  /**
   * Search through employees
   */
  public async onSearchClick(): Promise<void> {
    await this.loadEmployees();
  }

  /**
   * Grid double click
   */
  private onGridDoubleClick(): void {
    this.gridClick$.pipe(
      map((e: CellClickEvent) => { return { cell: e, time: new Date().getTime() }; }),
      bufferCount(2, 1),
      filter(e => e[0].time > new Date().getTime() - 300)
    ).subscribe(e => this.onEmployeeUpdate(e[0].cell.dataItem));
  }

  public getSelectedEmployeeIds(selectedEmployeeIds: number[]): void {
    this.selectedEmployeeIds = selectedEmployeeIds;
  }

  public async onEmployeeCreate(): Promise<void> {
    const dialogItems = this.getEmployeeDialogItems();
    dialogItems.find(i => i.field === 'isActive').value = true;

    const dialogResult = await this._dialogForm.show<EmployeeDialog>('Create a new Employee', dialogItems, { actionName: 'Create' });

    if (dialogResult.positive) {
      // Create a new employee model
      const model = dialogResult.model
      const newEmployee: EmployeeNew = {
        employeeId: parseInt(model.employeeId),
        firstName: model.firstName,
        lastName: model.lastName,
        discipline: this.getDiscipline(model.discipline),
        rate: model.rate,
        isActive: model.isActive,
      };

      try {
        this.isBusy = true;
        await this._service.createEmployee(newEmployee).toPromise();
        // Load employees
        await this.loadEmployees();
        this._notify.success('The employee was successfully created.');
      } finally {
        this.isBusy = false;
      }
    }
  }

  public async onEmployeeAction(e, employee: Employee): Promise<void> {
    switch (e.id) {
      case MENU_UPDATE: await this.onEmployeeUpdate(employee); break;
      case MENU_DELETE: await this.onEmployeeDelete(employee); break;
    }
  }


  // [ Internal ]
  private async onEmployeeUpdate(employee: Employee): Promise<void> {

    const dialogItems = this.getEmployeeDialogItems();
    dialogItems.find(i => i.field === 'employeeId').value = `${employee.employeeId}`;
    dialogItems.find(i => i.field === 'firstName').value = employee.firstName;
    dialogItems.find(i => i.field === 'lastName').value = employee.lastName;
    dialogItems.find(i => i.field === 'discipline').value = employee.discipline;
    dialogItems.find(i => i.field === 'rate').value = employee.rate;
    dialogItems.find(i => i.field === 'isActive').value = employee.isActive;

    const dialogResult = await this._dialogForm.show<EmployeeDialog>('Update Employee', dialogItems, { actionName: 'Update' });

    if (dialogResult.positive) {
      const model = dialogResult.model;
      // Create an update employee model
      const existingEmployee: EmployeeUpdate = {
        id: employee.id,
        employeeId: parseInt(model.employeeId),
        firstName: model.firstName,
        lastName: model.lastName,
        discipline: this.getDiscipline(model.discipline),
        rate: model.rate,
        isActive: model.isActive
      };

      try {
        this.isBusy = true;
        await this._service.updateEmployee(existingEmployee).toPromise();
        // Load employees
        await this.loadEmployees();
        this._notify.success('The employee was successfully updated.');
      } finally {
        this.isBusy = false;
      }
    }
  }

  public async onUpdateMultiple(): Promise<void> {
    // Generate dialog items
    const dialogItems = this.getEmployeesUpdateDialogItems();

    // Show dialog
    const dialogResult = await this._dialogForm.show<any>('Update Employees ', dialogItems, { actionName: 'Update' }, async (item, form) => {
      if (item.field === 'field') {
        // Load new property fields based on selected type
        form.items = this.getEmployeesUpdateDialogItems(item.value);
      }
    });

    if (dialogResult.positive) {
      const model = dialogResult.model;
      // Create an update employees list
      let employeesToUpdate: EmployeeUpdate[] = [];
      for (let i = 0; i < this.selectedEmployeeIds.length; i++) {
        // Find existing
        let existingEmployee: Employee = this.gridData.data.find(e => e.id === this.selectedEmployeeIds[i]);
        // Populate the list
        employeesToUpdate.push({
          id: existingEmployee.id,
          employeeId: existingEmployee.employeeId,
          firstName: existingEmployee.firstName,
          lastName: existingEmployee.lastName,
          discipline: model.field === 1 ? this.getDiscipline(model.discipline) : this.getDiscipline(existingEmployee.discipline),
          rate: model.field === 2 ? model.rate : existingEmployee.rate,
          isActive: model.field === 3 ? model.isActive : existingEmployee.isActive,
        });
      }
      try {
        this.isBusy = true;
        // Update multiple employees
        await this._service.updateMultipleEmployees(employeesToUpdate).toPromise();
        this.selectedEmployeeIds = [];
        // Load employees
        await this.loadEmployees();
        this._notify.success('The employees were successfully updated.');
      } finally {
        this.isBusy = false;
      }
    }
  }

  public async onEmployeeDelete(employee: Employee): Promise<void> {
    const ok = await this._dialogConfirm.show(`Delete "${employee.firstName} ${employee.lastName}"`, 'Are you sure?');
    if (ok) {
      try {
        this.isBusy = true;
        await this._service.deleteEmployee(employee.id).toPromise();
        // Load employees
        await this.loadEmployees();
        this._notify.success('The employee was successfully deleted.');
      } finally {
        this.selectedEmployeeIds = [];
        this.isBusy = false;
      }
    }
  }

  // [ Helper Functions ]
  private getDisciplineOptions(): FormItemOption[] {
    return [
      { id: 'PA', value: 'PA' },
      { id: 'PCA', value: 'PCA' },
      { id: 'HHA', value: 'HHA' },
      { id: 'PCA | HHA', value: 'PCA | HHA' }
    ];
  }

  private getDisciplineFilterOptions(): FormItemOption[] {
    return [
      { id: 1, value: 'PA' },
      { id: 2, value: 'PCA' },
      { id: 3, value: 'HHA' },
      { id: 4, value: 'PCA | HHA' }
    ];
  }

  private getGridActions(): ActionItem[] {
    return [
      { id: MENU_UPDATE, icon: 'k-icon k-i-edit', name: 'Update' },
      { id: MENU_DELETE, icon: 'k-icon k-i-delete', name: 'Delete' }
    ];
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

  private async loadEmployees(): Promise<void> {
    try {
      this.isBusy = true;
      this.gridData = await this._service.getEmployees(this.searchText, this._utils.getGridState(this.state)).toPromise();
    } finally {
      this.isBusy = false;
    }
  }

  private getDiscipline(discipline: string): string {
    if (discipline === 'PCA | HHA') return 'PCA_OR_HHA';
    else return discipline;
  }

  public async importEmployees(files: FileList): Promise<void> {
    try {
      this.isBusy = true;
      const newEmployees = await this._api.uploadFile<EmployeeNew[]>('/api/import/employees', files.item(0)).toPromise();
      const dialogResult = await this._dialogNewEmployee.showWindow(newEmployees);
      if (dialogResult.positive) { await this.loadEmployees(); }
    } catch {
    } finally {
      this.isBusy = false;
    }
  }
}
