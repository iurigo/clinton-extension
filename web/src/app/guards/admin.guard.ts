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
export class AdminGuard implements CanActivate {

  constructor(private _user: UserService, private _router: Router) { }

  public canActivate(): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return new Promise(async (res) => {
      const isAdmin = await this._user.isAdmin().toPromise();
      if (isAdmin) {
        res(true);
      } else {
        this._router.navigate(['login']);
        res(false);
      };
    });
  }
}
