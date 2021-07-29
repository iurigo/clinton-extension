// Angular
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
// Services
import { ApiService } from 'src/app/services/api.service';

@Injectable()
export class FooterService {

  constructor(private _api: ApiService) { }

  public getVersion(): Observable<string> {
    return this._api.graphql<{ version: string }>(`
      query {
        version
      }`
    ).pipe(map(res => res.version));
  }
}
