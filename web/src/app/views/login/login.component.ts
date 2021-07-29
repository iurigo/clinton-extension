// Angular
import { Component, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
// Services
import { UserService } from 'src/app/services/user.service';
// Components
import { DynamicFormComponent } from 'src/app/shared/components/dynamic-form/dynamic-form.component';
// Interfaces
import { FormItem } from 'src/app/shared/components/dynamic-form/dynamic-form.interfaces';
// Enums
import { FormItemType } from 'src/app/shared/components/dynamic-form/dynamic-form.enums';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {

  @ViewChild('form', { static: true }) public form: DynamicFormComponent;

  public showForgotPassword: boolean = false;
  public isBusy: boolean = false;
  public formData: FormItem[];

  constructor(
    private _user: UserService,
    private _router: Router,
  ) { }

  public async ngOnInit() {
    // Initial login form items
    this.formData = [
      { type: FormItemType.TEXT, field: 'username', title: 'Username', isRequired: true, focus: true },
      { type: FormItemType.PASSWORD, field: 'password', title: 'Password', isRequired: true }
    ];
  }

  public async onLoginClick(): Promise<void> {
    try {
      this.isBusy = true;
      const credentials: { username: string, password: string } = this.form.form.value;
      const signedIn = await this._user.login(credentials.username, credentials.password);
      if (signedIn) {
        await this._user.refreshUserInfo();
        await this._router.navigate(['']);
      }
    } finally {
      this.isBusy = false;
    }
  }
}
