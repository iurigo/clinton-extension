import { AbstractControl, ValidationErrors } from '@angular/forms';

export class FormulaValidators {

  static formulaTotalPercentage(control: AbstractControl): ValidationErrors | null {
    return control.value === 100 ? null : { invalidTotal: true };
  }

}
