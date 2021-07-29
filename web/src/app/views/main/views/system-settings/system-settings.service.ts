// Angular
import { Injectable } from '@angular/core';
// Interfaces
import { Tab } from 'src/app/shared/interfaces/tab.interface';


@Injectable()
export class SystemSettingsService {

  constructor(
  ) { }

  public getTabs(): Tab[] {
    const tabs: Tab[] = [];

    tabs.push({
      id: 'users',
      title: 'Users',
      url: '/system-settings/users'
    });

    return tabs;
  }
}
