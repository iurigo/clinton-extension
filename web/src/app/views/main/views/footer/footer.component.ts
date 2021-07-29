import { Component, OnInit } from '@angular/core';
// Service
import { FooterService } from './footer.service';
// Moment
import * as moment from 'moment';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss'],
  providers: [FooterService]
})
export class FooterComponent implements OnInit {

  public version: string;
  public year: string;

  constructor(private _service: FooterService) { }

  public async ngOnInit(): Promise<void> {
    this.version = await this._service.getVersion().toPromise();
    this.year = moment().format('YYYY');
  }
}
