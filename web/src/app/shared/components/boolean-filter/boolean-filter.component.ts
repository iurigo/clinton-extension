// Angular
import { Component, AfterViewInit, Input, Output, EventEmitter } from '@angular/core';
// KendoUI
import { FilterService } from '@progress/kendo-angular-grid';
import { CompositeFilterDescriptor, FilterDescriptor } from '@progress/kendo-data-query';


@Component({
  selector: 'app-boolean-filter',
  templateUrl: './boolean-filter.component.html',
  styleUrls: ['./boolean-filter.component.scss']
})
export class BooleanFilterComponent implements AfterViewInit {
  @Input() public currentFilter: CompositeFilterDescriptor;
  @Input() public falseLabel = 'Is False';
  @Input() public trueLabel = 'Is True';
  @Input() public filterService: FilterService;
  @Input() public field: string;
  @Output() public valueChange = new EventEmitter<boolean>();

  public value: boolean = null;

  public ngAfterViewInit() {
    const filter = <FilterDescriptor>this.currentFilter.filters.find((f: FilterDescriptor) => f.field === this.field);
    if (filter != null) {
      this.value = filter.value;
    }
  }

  public setValue(value): void {
    this.value = value;

    this.filterService.filter({
      filters: [{
        field: this.field,
        operator: 'eq',
        value: this.value
      }],
      logic: 'and'
    });
  }
}
