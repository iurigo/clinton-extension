// Angular
import { Injectable } from '@angular/core';
import { State } from '@progress/kendo-data-query';
// Interfaces
import { Employee, EmployeeNew, EmployeeUpdate } from 'src/app/views/main/views/employees/employee.interface';
// Services
import { ApiService } from 'src/app/services/api.service';
import { UtilitiesService } from 'src/app/services/utilities.service';
// RxJS
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
// Enums
import { PageableResult } from 'src/app/shared/interfaces/pageable-result.interface';
import { EmployeeDiscipline } from './employee.enum';

@Injectable({
  providedIn: 'root'
})
export class EmployeesService {
  
  constructor(
    private _api: ApiService,
    private _utils: UtilitiesService
  ) { }

  public getEmployees(search: string, state: State): Observable<PageableResult<Employee>> {
    if (search) {
      return this.getEmployeesSearch(search, state);
    } else {
      return this.getAllEmployees(state);
    }
  }

  public getAllEmployees(state: State): Observable<PageableResult<Employee>> {
    return this._api.graphql<{ employees: PageableResult<Employee> }>(`
      query($skip: Int, $take: Int) {
        employees: employeesPageable(skip: $skip, take: $take)
        ${this._utils.getSortExpression(state)}
        ${this._utils.getFilterExpression(state)} {
          data {
            id
            employeeId
            firstName
            lastName
            discipline
            rate
            isActive
          }
          total: totalCount
        }
      }`, { skip: state.skip, take: state.take }
    ).pipe(map(res => {
      return {
        data: res.employees.data.map(e => {
          return {
            ...e,
            discipline: EmployeeDiscipline[e.discipline]
          };
        }),
        total: res.employees.total
      }
    }));
  }

  private getEmployeesSearch(search: string, state: State): Observable<PageableResult<Employee>> {
    return this._api.graphql<{ employees: PageableResult<Employee> }>(`
      query($search: String!, $skip: Int, $take: Int) {
        employees: searchInEmployees(value: $search, skip: $skip, take: $take)
        ${this._utils.getSortExpression(state)}
          ${this._utils.getFilterExpression(state)} {
          data {
            id
            employeeId
            firstName
            lastName
            discipline
            rate
            isActive
          }
          total: totalCount
        }
      }`, { search, skip: state.skip, take: state.take }
    ).pipe(map(res => {
      return {
        data: res.employees.data.map(e => {
          return {
            ...e,
            discipline: EmployeeDiscipline[e.discipline]
          };
        }),
        total: res.employees.total
      }
    }));
  }

  public createEmployee(employee: EmployeeNew): Observable<void> {
    return this._api.graphql(`
      mutation($employee: EmployeeInput!) {
        employeeCreate(employee: $employee) { }
      }`, { employee }
    )
  }

  public updateEmployee(employee: EmployeeUpdate): Observable<void> {
    return this._api.graphql(`
      mutation($employee: EmployeeInput!) {
        employeeUpdate(employee: $employee)
      }`, { employee }
    )
  }

  public updateMultipleEmployees(employees: EmployeeUpdate[]): Observable<void> {
    return this._api.graphql(`
      mutation($employees: [EmployeeInput!]) {
        employees: employeeMultipleUpdate(employees: $employees)
      }`, { employees }
    )
  }

  public deleteEmployee(employeeId: number): Observable<void> {
    return this._api.graphql(`
    mutation ($employeeId: Int!) {
      employeeDelete(id: $employeeId)
    }`, { employeeId }
    )
  }

  public getEmployee(employeeId: number): Observable<Employee> {
    return this._api.graphql<{ employee: Employee }>(`
    query($employeeId: Int!) {
      employee(id: $employeeId){
        id
        employeeId
        firstName
        lastName
        discipline
        rate
        createdAt
        updatedAt
      }
    }`, { employeeId }
    ).pipe(map(res => res.employee))
  }

  public getDisciplineAsNumber(discipline: string): number {
    switch (discipline) {
      case 'PA': return 1;
      case 'PCA': return 2;
      case 'HHA': return 3;
      case 'PCA_OR_HHA': return 4;
    }
  }
}
