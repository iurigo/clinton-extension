// Angular
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { saveAs } from "@progress/kendo-file-saver";
import { NotifyService } from './notify.service';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private _http: HttpClient, private _notify: NotifyService) { }

  public graphql<T>(query: string, variables: any = null): Observable<T> {
    return this._http.post<{ data: T, errors?: { message: string }[] }>('/graphql', { query, variables: variables || {} })
      .pipe(
        map(response => {
          // Catch GraphQl errors
          if (response.errors && response.errors.length > 0) {
            throw ({ message: response.errors[0].message });
          }
          return response.data;
        }),
        catchError(this.handleError.bind(this))
      );
  }

  public get<T>(url: string): Observable<T> {
    return this._http.get<T>(url).pipe(catchError(this.handleError.bind(this)));
  }

  public downloadFile(url: string, filename: string, body?: any, method: string = 'GET'): Observable<void> {
    return this._http.request(method, url, {
      body: body,
      observe: "response",
      responseType: "arraybuffer"
    })
      .pipe(map(res => {
        const file = new Blob([res.body], { type: "application" });
        saveAs(file, filename);
      }));
  }

  public uploadFile<T>(url: string, file: File, suppressAlert: boolean = false): Observable<T> {
    const form = new FormData();
    form.append('file', file);
    return this._http.post(url, form).pipe(catchError(this.handleError.bind(this, suppressAlert)));
  }


  // [ INTERNAL ]
  private handleError(suppressAlert: boolean, error: HttpErrorResponse) {
    let message: string = 'System Error.';
    if (error.error != null && error.error.errors != null && (<any[]>error.error.errors).length > 0) {
      message = error.error.errors[0].message;
    } else if (error.error != null && typeof error.error.message === 'string') {
      message = error.error.message;
    } else if (error.error instanceof ArrayBuffer) {
      const rawMessage: { message: string } = JSON.parse(new TextDecoder().decode(error.error));
      if (rawMessage?.message != null) { message = rawMessage.message; }
    } else if (typeof error.message === 'string') {
      message = error.message;
    }

    // Do not show gateway error notification (the end point is not available)
    const excludeErrorCodes: number[] = [405, 418, 502, 504];
    if (!excludeErrorCodes.some(code => code === error.status) && !suppressAlert) {
      this._notify.error(message);
    }

    return throwError(message);
  }

}
