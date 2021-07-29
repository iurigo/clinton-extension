// Angular
import { HttpHeaders, HttpParams } from '@angular/common/http';

export interface ApiOptions {
  body?: any;
  headers?: HttpHeaders | { [header: string]: string | string[]; };
  observe?: 'response' | 'body';
  params?: HttpParams | { [param: string]: string | string[]; };
  reportProgress?: boolean;
  responseType?: 'json' | 'arraybuffer' | 'blob' | 'text';
  withCredentials?: boolean;
  notify?: boolean;
}
