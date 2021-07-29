// Angular
import { Component, AfterViewInit, Input, Output, EventEmitter } from '@angular/core';
// KendoUI
import { FilterService } from '@progress/kendo-angular-grid';
import { CompositeFilterDescriptor, FilterDescriptor, filterBy, distinct } from '@progress/kendo-data-query';


@Component({
  selector: 'app-multicheck-filter',
  templateUrl: './multicheck-filter.component.html',
  styleUrls: ['./multicheck-filter.component.scss']
})
export class MulticheckFilterComponent implements AfterViewInit {
  @Input() public isPrimitive: boolean = false;
  @Input() public currentFilter: CompositeFilterDescriptor;
  @Input() public data = [];
  @Input() public textField = 'value';
  @Input() public valueField = 'id';
  @Input() public filterService: FilterService;
  @Input() public field: string;
  @Output() public valueChange = new EventEmitter<number[]>();

  public currentData: any;
  public showFilter = true;
  private value: any[] = [];

  public textAccessor = (dataItem: any) => this.isPrimitive || dataItem == null ? dataItem : dataItem[this.textField];
  public valueAccessor = (dataItem: any) => this.isPrimitive || dataItem == null ? dataItem : dataItem[this.valueField];

  public ngAfterViewInit() {
    this.currentData = this.data;
    this.value = this.currentFilter.filters.map((f: FilterDescriptor) => f.value);

    this.showFilter = typeof this.textAccessor(this.currentData[0]) === 'string';
  }

  public isItemSelected(item) {
    return this.value.some(x => x === this.valueAccessor(item));
  }

  public onSelectionChange(item, li) {
    if (this.value.some(x => x === item)) {
      this.value = this.value.filter(x => x !== item);
    } else {
      this.value.push(item);
    }

    this.filterService.filter({
      filters: this.value.map(value => ({
        field: this.field,
        operator: 'eq',
        value
      })),
      logic: 'or'
    });

    this.onFocus(li);
  }

  public onInput(e: any) {
    this.currentData = distinct([
      ...this.currentData.filter(dataItem => this.value.some(val => val === this.valueAccessor(dataItem))),
      ...filterBy(this.data, {
        operator: 'contains',
        field: this.textField,
        value: e.target.value
      })],
      this.textField
    );
  }

  public onFocus(li: any): void {
    const ul = li.parentNode;
    const below = ul.scrollTop + ul.offsetHeight < li.offsetTop + li.offsetHeight;
    const above = li.offsetTop < ul.scrollTop;

    // Scroll to focused checkbox
    if (below || above) {
      ul.scrollTop = li.offsetTop;
    }
  }

}
