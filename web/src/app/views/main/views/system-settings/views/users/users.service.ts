// Angular
import { Injectable } from '@angular/core';
// Interfaces
import { User, UserNew, UserRaw, UserUpdate } from './users.interface';
// Services
import { ApiService } from 'src/app/services/api.service';
// RxJS
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Role, Status } from './users.enum';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  constructor(private _api: ApiService) { }

  public getAllUsers(): Observable<User[]> {
    return this._api.graphql<{ users: UserRaw[] }>(`
      query {
        users{
          id
          username
          fullName
          role
          status
        }
      }
    `).pipe(map(res => res.users.map(user => {
      return {
        id: user.id,
        fullName: user.fullName,
        username: user.username,
        isAdmin: Role[user.role] === Role.ADMIN ? true : false,
        isActive: Status[user.status] === Status.ACTIVE ? true : false
      };
    })));
  }

  public createUser(user: UserNew): Observable<void> {
    return this._api.graphql(`
      mutation($user: UserCreateInput!) {
        userCreate(user: $user) { }
      }`, { user }
    )
  }
  public updateUser(user: UserUpdate): Observable<void> {
    return this._api.graphql(`
      mutation($user: UserUpdateInput!) {
        userUpdate(user: $user)
      }`, { user }
    )
  }

  public resetPassword(userId: number, password: string): Observable<void> {
    return this._api.graphql(`
      mutation($userId: Int!, $password: String!) {
        userSetPassword(id: $userId, password: $password)
      }`, { userId, password }
    )
  }

  public deleteUser(userId: number): Observable<void> {
    return this._api.graphql(`
    mutation ($userId: Int!) {
      userDelete(id: $userId)
    }`, { userId }
    )
  }

  public getUserInfo(userId: number): Observable<User> {
    return this._api.graphql<{ user: UserRaw }>(`
    query($userId: Int!) {
      user(id: $userId){
        id
        username
        fullName
        role
        status
      }
    }`, { userId }
    ).pipe(map(res => {
      return {
        id: res.user.id,
        fullName: res.user.fullName,
        username: res.user.username,
        isAdmin: Role[res.user.role] === Role.ADMIN ? true : false,
        isActive: Status[res.user.status] === Status.ACTIVE ? true : false
      };
    }))
  }

}
