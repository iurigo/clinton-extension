// Angular
import { Injectable } from '@angular/core';
import { CanActivate, UrlTree, Router } from '@angular/router';
// Services
import { UserService } from '../services/user.service';
// RxJS
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MainGuard implements CanActivate {

  constructor(private _user: UserService, private _router: Router) { }

  public canActivate(): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return new Promise(async res => {
      await this._user.initialize();
      if (this._user.isAuthorized$.value) {
        await this._user.refreshAccessToken();
        res(true);
      } else {
        this._router.navigate(['/login']);
        res(false);
      }
    });
  }
}
