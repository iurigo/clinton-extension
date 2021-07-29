import { Component } from '@angular/core';
import { DialogRef } from '@progress/kendo-angular-dialog';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { PasswordValidators } from 'src/app/shared/form-validators/password-validators';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent {

  public dialog: DialogRef;
  public formTitle: string;
  public togglePasswordInput1: string = 'password';
  public togglePasswordInput2: string = 'password';

  public form = new FormGroup({
    password: new FormControl('', [Validators.required, Validators.minLength(4)]),
    passwordConfirm: new FormControl('', [Validators.required, Validators.minLength(4)])
  }, { validators: PasswordValidators.passwordsMatch });

  public onClose(): void {
    this.dialog.close();
  }

  public onFormSubmit(): void {
    this.dialog.close(this.form.value.password);
  }

  public oTogglePasswordInput1() {
    this.togglePasswordInput1 === 'password' ? this.togglePasswordInput1 = 'text' : this.togglePasswordInput1 = 'password';
  }

  public oTogglePasswordInput2() {
    this.togglePasswordInput2 === 'password' ? this.togglePasswordInput2 = 'text' : this.togglePasswordInput2 = 'password';
  }

}


