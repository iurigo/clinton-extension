// Angular
import { FormControl, ValidatorFn } from '@angular/forms';


export function isEqualToValidator(field: string, isPrimary: boolean): ValidatorFn {
  return (control1: FormControl): { [key: string]: any } | null => {
    if (!!control1.parent) {
      const control2 = control1.parent.get(field);

      if ((control1 != null && control2 != null) && control1.value !== control2.value) {
        if (isPrimary) {
          control2.setErrors({ notEqual: true });
          return null;
        }
        return { notEqual: true };
      }
      control2.setErrors(null);
    }
    return null;
  };
}