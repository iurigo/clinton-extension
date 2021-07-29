export interface DialogResult<T> {
  positive: boolean;
  model: T | any;
}