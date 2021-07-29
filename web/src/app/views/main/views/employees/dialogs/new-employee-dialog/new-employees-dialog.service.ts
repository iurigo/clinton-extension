// Angular
import { Injectable } from '@angular/core';
// Interfaces
import { EmployeeNew } from 'src/app/views/main/views/employees/employee.interface';
// Services
import { ApiService } from 'src/app/services/api.service';
// RxJS
import { Observable } from 'rxjs';
import { EmployeeDiscipline } from './new-employees.enum';

@Injectable()
export class NewEmployeesDialogService {

  constructor(
    private _api: ApiService
  ) { }

  public createMultipleEmployees(employees: EmployeeNew[]): Observable<void> {
    // Convert discipline
    employees = employees.map(e => { return { ...e, discipline: EmployeeDiscipline[e.discipline] } })
    return this._api.graphql(`
      mutation($employees:[EmployeeInput!]) {
        employeeMultipleCreate(employees: $employees) { }
      }`, { employees }
    )
  }
}
