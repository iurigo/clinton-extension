import { Role, Status } from './users.enum';

export interface UserBase {
  fullName: string;
  username: string;
}

export interface UserRaw extends UserBase {
  id: number;
  role: string;
  status: string;
}

export interface UserNew extends UserBase {
  role: string;
  status: string;
  password: string;
}

export interface UserUpdate extends UserBase {
  id: number;
  role: string;
  status: string;
}

export interface User extends UserBase {
  id: number;
  isAdmin: boolean;
  isActive: boolean;
}

export interface UserDialog extends UserBase {
  id: number;
  isAdmin: boolean;
  isActive: boolean;
  password: string;
}