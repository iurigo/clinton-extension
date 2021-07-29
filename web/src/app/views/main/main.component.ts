import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Tab } from 'src/app/shared/interfaces/tab.interface';
import { MainService } from './main.service';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss'],
  providers: [MainService]
})
export class MainComponent implements OnInit {

  public tabs: Tab[] = [];

  constructor(
    private _service: MainService,
    private _router: Router
  ) { }

  public async ngOnInit() {
    // Get the list of available tabs
    this.tabs = await this._service.getTabs();

    // (Optionally) redirect to the first available tab
    if (this._router.url === '/') {
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
