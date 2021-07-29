// Angular
import { Component, EventEmitter, Input, OnDestroy, Output, OnChanges, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { FormGroup, FormControl, Validators, ValidatorFn } from '@angular/forms';
import { Subscription, of, combineLatest } from 'rxjs';
import { UtilitiesService } from 'src/app/services/utilities.service';
// Services
// Enums
import { FormItemType } from './dynamic-form.enums';
// Interfaces
import {
  FormItem,
  FormItemPassword,
  FormItemText,
  FormItemUrl,
  FormItemSlider,
  FormItemActionEvent,
  FormItemExtrasEvent
} from './dynamic-form.interfaces';


@Component({
  selector: 'app-dynamic-form',
  templateUrl: './dynamic-form.component.html',
  styleUrls: ['./dynamic-form.component.scss']
})
export class DynamicFormComponent implements OnDestroy, OnChanges, AfterViewInit {

  @Input() public items: FormItem[] = [];
  @Input() public style: string = null;

  @Output() public onValueChange = new EventEmitter<FormItem>();
  @Output() public onEnterKeyPress = new EventEmitter<FormItem>();
  @Output() public onAction = new EventEmitter<FormItemActionEvent<any>>();
  @Output() public onExtras = new EventEmitter<FormItemExtrasEvent<any>>();

  public form = new FormGroup({});
  public get itemType() { return FormItemType; }

  private subscriptions$ = new Subscription();


  constructor(
    public _util: UtilitiesService,
    public _cd: ChangeDetectorRef
  ) { }

  public ngOnDestroy() {
    this.subscriptions$.unsubscribe();
  }

  public ngOnChanges() {
    // Reset the form
    this.subscriptions$.unsubscribe();
    this.subscriptions$ = new Subscription();

    this.form.clearValidators();
    Object.keys(this.form.controls).forEach(key => {
      this.form.removeControl(key);
    });

    for (var item of this.items) {
      // Skip helper items
      if (item.type === FormItemType.SEPARATOR) { continue; }

      // Build form item the validators
      const validators: ValidatorFn[] = [];
      if (item.isRequired === true) { validators.push(Validators.required); }
      if ((<FormItemText | FormItemPassword>item).minLength) { validators.push(Validators.minLength((<FormItemText | FormItemPassword>item).minLength)); }
      if ((<FormItemText | FormItemPassword>item).maxLength) { validators.push(Validators.maxLength((<FormItemText | FormItemPassword>item).maxLength)); }
      if (Array.isArray(item.validators)) { validators.push(...item.validators); }

      // Special cases
      switch (item.type) {
        case FormItemType.URL:
          (<FormItemUrl>item).altView = (item.value == null || item.value === '');
          break;

        case FormItemType.SLIDER:
          // Hide item if there is no point of showing the control
          if ((<FormItemSlider>item).min === (<FormItemSlider>item).max) { item.isHidden = true; }
          break;
      }

      // Create form item control
      this.form.setControl(item.field, new FormControl({ value: item.value, disabled: false }, validators));

      // Listen for value changes
      this.subscriptions$.add(combineLatest([of(item), this.form.get(item.field).valueChanges]).subscribe(e => {
        this.onValueChange.emit(e[0]);
      }));

    }
  }

  public ngAfterViewInit(): void {
    this._cd.detectChanges();
  }
}
