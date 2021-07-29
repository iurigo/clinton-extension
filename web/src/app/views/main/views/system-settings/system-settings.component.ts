import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Tab } from 'src/app/shared/interfaces/tab.interface';
import { SystemSettingsService } from './system-settings.service';

@Component({
  selector: 'app-system-settings',
  templateUrl: './system-settings.component.html',
  styleUrls: ['./system-settings.component.scss'],
  providers: [SystemSettingsService]
})
export class SystemSettingsComponent implements OnInit {

  public tabs: Tab[] = [];

  constructor(
    private _service: SystemSettingsService,
    private _router: Router
  ) { }

  public async ngOnInit() {
    // Get the list of available tabs
    this.tabs = this._service.getTabs();

    // (Optionally) redirect to the first available tab
    if (this._router.url === '/system-settings') {
      this._router.navigate([this.tabs[0].url]);
    }
  }

  public isSelected(url: string): boolean {
    return this._router.url.startsWith(url);
  }

  public onTabSelect(e: { index: number }): void {
    if (!this._router.url.startsWith(this.tabs[e.index].url)) {
      this._router.navigate([this.tabs[e.index].url]);
    }
  }

}
