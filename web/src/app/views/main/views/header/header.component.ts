import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from 'src/app/services/user.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnDestroy {

  public fullName: string;
  public isLoading: boolean;

  private subscription$ = new Subscription;

  constructor(private _router: Router, private _user: UserService) { }

  public ngOnInit(): void {
    this.subscription$.add(this._user.fullName$.subscribe(fullName => this.fullName = fullName));
  }

  public ngOnDestroy(): void {
    this.subscription$.unsubscribe();
  }

  public async onClickLogout(): Promise<void> {
    this.isLoading = true;
    this._user.logout();
    await this._router.navigate(['/login']);
    this.isLoading = false;
  }

}
