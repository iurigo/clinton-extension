// Angular
import { Injectable } from '@angular/core';
import { UserService } from 'src/app/services/user.service';
// Interfaces
import { Tab } from 'src/app/shared/interfaces/tab.interface';


@Injectable()
export class MainService {

  constructor(
    private _user: UserService
  ) { }

  public async getTabs(): Promise<Tab[]> {
    const tabs: Tab[] = [];

    tabs.push({
      id: 'employees',
      title: 'Employees',
      url: '/employees'
    });
    if (await this._user.isAdmin().toPromise()){
      tabs.push({
        id: 'system-settings',
        title: 'Settings',
        url: '/system-settings'
      });
    }

    return tabs;
  }
}
