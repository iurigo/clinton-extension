// Angular
import { ValidatorFn } from '@angular/forms';
// Enums
import { FormItemType, FormItemAlignment } from './dynamic-form.enums';


export interface FileInfo {
  id?: number;
  name: string;
  ext: string;
  size: number;
  source?: File;
}

export interface FormItemOption {
  id: number | string;
  value: string;
  parentValueId?: number;
}

export interface FormItemAction {
  action: string;
  title?: string;
  iconCss?: string;
}

export interface FormItemActionEvent<T> {
  action: string;
  item: FormItem;
  e?: T;
}

export interface FormItemExtrasEvent<T> {
  item: FormItem;
  e?: T;
}

export interface FormItemExtras {
  type: FormItemType;
  field: string;
  title?: string;
  titlePosition?: 'left' | 'right';
  options?: FormItemOption[];
  trueLabel?: string;
  falseLabel?: string;
  width?: number;
  value?: any;
}
export interface FormItem {

  type: FormItemType;
  field?: string;
  title?: string;
  value?: any;
  isRequired?: boolean;
  isOptional?: boolean;
  isHidden?: boolean;
  isReadOnly?: boolean;
  disabled?: boolean; // Implemented for [Text] only for now.
  focus?: boolean;
  validators?: ValidatorFn[];
  actions?: FormItemAction[];
  extras?: FormItemExtras;
  style?: any;
  labelStyle?: any;
}

export interface FormItemSeparator extends FormItem {
  alignment?: FormItemAlignment;
}

export interface FormItemText extends FormItem {
  value?: string;
  minLength?: number;
  maxLength?: number;
  mask?: string;
  isBarcode?: boolean;
}

export interface FormItemMultilineText extends FormItem {
  value?: string;
  rows?: number;
}

export interface FormItemEditor extends FormItem {
  value?: string;
  options?: FormItemOption[];
}

export interface FormItemPassword extends FormItem {
  value?: string;
  minLength?: number;
  maxLength?: number;
  altView?: boolean;
}

export interface FormItemUrl extends FormItem {
  value?: string;
  altView?: boolean;
}

export interface FormItemNumber extends FormItem {
  value?: number;
  min?: number;
  max?: number;
  format?: string;
}

export interface FormItemSlider extends FormItem {
  value?: number;
  min: number;
  max: number;
}

export interface FormItemDateTime extends FormItem {
  value?: Date;
  format?: string;
}

export interface FormItemBoolean extends FormItem {
  value?: boolean;
  trueLabel?: string;
  falseLabel?: string;
}

export interface FormItemDropDown extends FormItem {
  value?: number | string;
  options?: FormItemOption[];
  linkedPropertyId?: number;
  parentPropertyId?: number;
  parentValueId?: number;
}

export interface FormItemRadio extends FormItem {
  value?: number | string;
  options?: FormItemOption[];
}

export interface FormItemMultiSelect extends FormItem {
  value?: number[] | string[];
  options?: FormItemOption[];
  hideOptions?: boolean;
}

export interface FormItemFile extends FormItem {
  value?: FileInfo[];
  accept: string;
  takePicture: boolean;
}

export interface FormItemColor extends FormItem {
  value?: string;
  colors?: Array<string>;
}