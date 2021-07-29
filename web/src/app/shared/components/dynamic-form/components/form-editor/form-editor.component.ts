// Angular
import { Component, EventEmitter, forwardRef, Input, OnChanges, Output, ViewChild } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
// Classes
import { FormControlValueAccessor } from '../../classes/form-control-value-accessor';
// Interfaces
import { FormEditorOption } from './form-editor.interfaces';
import { FormItemActionEvent, FormItemEditor, FormItemExtrasEvent } from '../../dynamic-form.interfaces';
import { EditorComponent } from '@progress/kendo-angular-editor';


@Component({
  selector: 'app-form-editor',
  templateUrl: './form-editor.component.html',
  styleUrls: ['./form-editor.component.scss', '../../styles/dynamic-form-item.scss'],
  providers: [
    { provide: NG_VALUE_ACCESSOR, multi: true, useExisting: forwardRef(() => FormEditorComponent) }
  ]
})
export class FormEditorComponent extends FormControlValueAccessor implements OnChanges {

  @Input() public item: FormItemEditor = null;
  @Output() public onAction = new EventEmitter<FormItemActionEvent<any>>();
  @Output() public onExtras = new EventEmitter<FormItemExtrasEvent<any>>();

  @ViewChild('editor', { static: true }) public editor: EditorComponent;

  public options: FormEditorOption[] = [];

  constructor() { super(); }

  public ngOnChanges(): void {
    this.options = this.item.options.map(o => {
      const option: FormEditorOption = {
        id: <string>o.id,
        value: o.value,
        click: (e: FormEditorOption) => {
          this.editor.exec('insertText', { text: e.id });
        }
      };
      return option;
    });
  }
}
