// Angular
import { Injectable } from '@angular/core';
// Interfaces
import { AccessToken } from '../interfaces/access-token.interface';
// RxJS
import { Observable, BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
// Services
import { ApiService } from './api.service';

const REFRESH_TOKEN = 'REFRESH_TOKEN';

@Injectable({
  providedIn: 'root'
})

export class UserService {
  public isAuthorized$ = new BehaviorSubject<boolean>(false);
  public accessToken$ = new BehaviorSubject<string>(null);
  public id$ = new BehaviorSubject<number>(null);
  public isAdmin$ = new BehaviorSubject<boolean>(null);
  public isActive$ = new BehaviorSubject<boolean>(null);
  public fullName$ = new BehaviorSubject<string>(null);

  // Private
  private refreshToken: string = null;
  private refreshTokenTimer: any = null;

  constructor(private _api: ApiService) { }

  public async initialize(): Promise<void> {
    this.isAuthorized$.next(sessionStorage.getItem(REFRESH_TOKEN) != null);
  }

  public async refreshAccessToken(): Promise<void> {
    try {
      // Get access-token
      const accessToken = await this.getRefreshAccessToken(sessionStorage.getItem(REFRESH_TOKEN)).toPromise();
      // Set access-token
      this.setAccessToken(accessToken);
      this.isAuthorized$.next(true);
      // Return success
    } catch { }
    this.isAuthorized$.next(false);
  }

  public async login(username: string, password: string): Promise<boolean> {
    try {
      // Get access-token
      const accessToken = await this.getAccessToken(username, password).toPromise();
      // Set access-token
      this.setAccessToken(accessToken);
      this.isAuthorized$.next(true);
      // Return success
      return true;
    } catch { };

    this.isAuthorized$.next(false);
    return false;
  }

  public logout(): void {
    sessionStorage.clear();
    this.accessToken$.next(null);
    this.isAuthorized$.next(false);
    this.clearUserInfo();
  }

  public async setAccessToken(accessToken: AccessToken): Promise<void> {
    sessionStorage.setItem(REFRESH_TOKEN, accessToken.refreshToken);
    this.accessToken$.next(accessToken.token);
    this.isAuthorized$.next(true);
    this.isAdmin$.next(await this.isAdmin().toPromise());
    this.fullName$.next(await this.getFullName().toPromise());
    this.id$.next(await this.getUserId().toPromise());
    // Enable access-token auto refresh
    this.startTokenAuthRefresh(55);
  }

  private async getUserInfo(): Promise<void> {
    this.id$.next(await this.getUserId().toPromise());
    this.isAdmin$.next(await this.isAdmin().toPromise());
    this.fullName$.next(await this.getFullName().toPromise());
    this.isActive$.next(await this.isActive().toPromise());
  }

  public startTokenAuthRefresh(intervalInMinutes: number): void {
    setInterval(async () => {
      const token = await this.getRefreshAccessToken(sessionStorage.getItem(REFRESH_TOKEN)).toPromise();
      this.setAccessToken(token);
    }, 1000 * 60 * intervalInMinutes); // (ms * s * m) once every 55 minutes
  }

  public clearUserInfo(): void {
    this.id$.next(null);
    this.fullName$.next(null);
    this.isAdmin$.next(null);
    this.isActive$.next(null);
    clearInterval(this.refreshTokenTimer);
  }

  public async refreshUserInfo(): Promise<void> {
    // Get user info
    await this.getUserInfo();

    // Make sure that user is active
    if (!this.isActive$.value) {
      this.logout();
    }
  }

  // GraphQL
  private getAccessToken(username: string, password: string): Observable<AccessToken> {
    return this._api.graphql<{ accessToken: AccessToken }>(`
    query($username: String!, $password: String!) {
      accessToken(username: $username, password: $password) {
          refreshToken
          token
        }
      }`,
      { username, password }
    ).pipe(map(e => e.accessToken));
  }

  private getRefreshAccessToken(refreshToken: string): Observable<AccessToken> {
    return this._api.graphql<{ refreshToken: AccessToken }>(`
    query($refreshToken: String!) {
      refreshToken(token: $refreshToken) {
          refreshToken
          token
        }
      }`,
      { refreshToken }
    ).pipe(map(e => e.refreshToken));
  }

  private getFullName(): Observable<string> {
    return this._api.graphql<{ fullName: string }>(`
      query {
          fullName
        }
    `).pipe(map(res => res.fullName));
  }

  public isAdmin(): Observable<boolean> {
    return this._api.graphql<{ isAdmin: boolean }>(`
      query {
          isAdmin
        }
    `).pipe(map(res => res.isAdmin));
  }

  private getUserId(): Observable<number> {
    return this._api.graphql<{ userId: number }>(`
      query {
          userId
        }
    `).pipe(map(res => res.userId));
  }

  private isActive(): Observable<boolean> {
    return this._api.graphql<{ isActive: boolean }>(`
      query {
          isActive
        }
    `).pipe(map(res => res.isActive));
  }

}
