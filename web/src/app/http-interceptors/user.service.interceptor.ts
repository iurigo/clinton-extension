// Angular
import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
// Services
import { UserService } from '../services/user.service';
// RxJS
import { Observable } from 'rxjs';

@Injectable()
export class UserServiceInterceptor implements HttpInterceptor {
  constructor(private _user: UserService) { }

  public intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Get access token
    const token = this._user.accessToken$.value;

    if (token != null) {
      //  Set Default Headers
      const authReq = req.clone({
        headers: req.headers.set('Authorization', `bearer ${token}`)
      });
      // Send updated request
      return next.handle(authReq);
    } else {
      // Leave request as it is
      return next.handle(req);
    }
  }
}
