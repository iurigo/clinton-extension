
export interface Employee {
  id?: number;
  employeeId: number;
  firstName: string;
  lastName: string;
  discipline: string;
  rate?: number;
  isActive?: boolean;
  updatedAt: Date;
}

export interface EmployeeNew {
  employeeId: number;
  firstName: string;
  lastName: string;
  discipline: string;
  rate: number;
  isActive: boolean;
}

export interface EmployeeUpdate {
  id: number;
  employeeId: number;
  firstName: string;
  lastName: string;
  discipline: string;
  rate: number;
  isActive: boolean;
}

export interface EmployeeDialog {
  id?: number;
  employeeId: string;
  firstName: string;
  lastName: string;
  discipline: string;
  rate: number;
  isActive: boolean;
}