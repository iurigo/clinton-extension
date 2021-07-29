import { AbstractControl, ValidationErrors } from '@angular/forms';

export class PasswordValidators {

  static passwordsMatch(control: AbstractControl): ValidationErrors | null {
    const a = control.get('password').value;
    const b = control.get('passwordConfirm').value;
    if (a === b) { return null }
    else { return { 'passwordsFailed': true } }
  }

}
