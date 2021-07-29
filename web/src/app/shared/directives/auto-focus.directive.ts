// Angular
import { Directive, AfterContentInit, ElementRef, Input } from "@angular/core";


@Directive({
  selector: '[appAutoFocus]'
})
export class AutoFocusDirective implements AfterContentInit {

  @Input() public appAutoFocus: boolean;

  constructor(private _element: ElementRef) { }

  public ngAfterContentInit() {
    if (!!this.appAutoFocus) {
      setTimeout(() => {
        if (!!this._element.nativeElement.children[0] && !!this._element.nativeElement.children[0].children[0] && !!this._element.nativeElement.children[0].children[0].focus) {
          this._element.nativeElement.children[0].children[0].focus();
        } else if (!!this._element.nativeElement.firstChild && !!this._element.nativeElement.firstChild.focus) {
          this._element.nativeElement.firstChild.focus();
        } else if (!!this._element.nativeElement.focus) {
          this._element.nativeElement.focus();
        } else {
          console.warn('Focus element was not found.');
        }
      }, 200);
    }
  }
}