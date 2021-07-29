// Angular
import { Injectable } from '@angular/core';
// Moment
import * as moment from 'moment';
// KendoUI
import { State, CompositeFilterDescriptor, FilterDescriptor } from '@progress/kendo-data-query';
import { WindowRef } from '@progress/kendo-angular-dialog';
// Enums
import { FilterOperator } from '../shared/enums/filter-operator.enum';


@Injectable({
  providedIn: 'root'
})
export class UtilitiesService {

  public ScrollbarWidth: number = null;


  public async initialize(): Promise<void> {
    // Calculate scrollbar's default width
    let scrollbox = document.createElement('div');
    scrollbox.style.overflowY = 'scroll';
    document.body.appendChild(scrollbox);

    this.ScrollbarWidth = Math.max(scrollbox.offsetWidth - scrollbox.clientWidth - 1, 0);
    document.body.removeChild(scrollbox);
  }
  public keepWindowVisible(windowRef: WindowRef): void {
    // prevent moving beyond top
    if (windowRef.window.instance.top < 0) {
      windowRef.window.instance.setOffset('top', 0);
    };
    // prevent moving below bottom
    if (windowRef.window.instance.top > window.innerHeight - 44) {
      windowRef.window.instance.setOffset('top', window.innerHeight - 44);
    };
    // prevent moving beyond left (set offset 44px)
    if (windowRef.window.instance.left <= 44 - (windowRef.window.instance.width)) {
      windowRef.window.instance.setOffset('left', 44 - (windowRef.window.instance.width));
    };
    // prevent moving beyond right (set offset 44px)
    if (windowRef.window.instance.left >= window.innerWidth - 44) {
      windowRef.window.instance.setOffset('left', window.innerWidth - 44);
    }
  }

  public getSortExpression(state: State): string {
    if (state.sort.filter(s => s.dir != null).length > 0) {
      return `@sort(complex: [${state.sort.filter(s => s.dir != null).map(s => `{ field: "${s.field}", direction: ${s.dir.toUpperCase()} }`).join(', ')}])`;
    }
    return '';
  }

  public getFilterExpression(state: State): string {
    if (state.filter != null && state.filter.filters.length > 0) {
      const columnFilters = state.filter.filters.map((cf: CompositeFilterDescriptor) => {
        return `{ logic: ${cf.logic.toUpperCase()}, filters: [${cf.filters.map((f: FilterDescriptor) => {
          const quotes = typeof f.value === 'string' ? '"' : '';
          return `{ field: "${f.field}", operator: ${(f.operator)}, value: ${quotes}${f.value}${quotes} }`;
        }).join(', ')}] }`
      });

      return `@filter(complex: { logic: ${state.filter.logic.toUpperCase()}, filters: [${columnFilters}] })`;
    }
    return '';
  }

  public getGridState(state: State, options: { dateFields: string[] } = { dateFields: [] }): State {
    return {
      skip: state.skip,
      take: state.take,
      filter: this.getCustomFilterState(state, options),
      sort: state.sort
    };
  }

  public formatBytes(bytes: number, decimals: number = 2): string {
    if (bytes == 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return `${parseFloat((bytes / Math.pow(k, i)).toFixed(decimals))} ${sizes[i]}`;
  }

  public async readFileAsString(file: Blob): Promise<string> {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = () => resolve(reader.result.toString());
      reader.onerror = error => reject(error);
    })
  }

  public async getImageDimensions(base64string: string): Promise<{ height: number, width: number }> {
    return new Promise((resolve, reject) => {
      const image = new Image();
      image.onload = () => resolve({ height: image.height, width: image.width });
      image.onerror = error => reject(error);
      image.src = base64string;
    });
  }

  public async resizeImage(base64string: string, maxWidth: number, maxHeight: number): Promise<string> {
    return new Promise((resolve, reject) => {
      const image = new Image();
      image.onload = () => {
        let ratio: number;
        if (image.width < maxWidth) {
          ratio = maxWidth / image.width;
        } else {
          ratio = maxHeight / image.height;
        }
        const newWidth = image.width * ratio;
        const newHeight = image.height * ratio;
        const canvas: HTMLCanvasElement = document.createElement('canvas');
        canvas.height = newHeight;
        canvas.width = newWidth;
        const ctx = canvas.getContext('2d');
        ctx.drawImage(image, 0, 0, image.width, image.height, 0, 0, newWidth, newHeight);
        resolve(canvas.toDataURL());
      };
      image.onerror = error => reject(error);
      image.src = base64string;
    });
  }

  public isIOS(): boolean {
    const platforms: string[] = [
      'iPad Simulator',
      'iPhone Simulator',
      'iPod Simulator',
      'iPad',
      'iPhone',
      'iPod'
    ];

    return platforms.includes(navigator.platform) || (navigator.userAgent.includes("Mac") && "ontouchend" in document);
  }

  public timeSpanToString(value: string): string {
    if (value == null || value === '') { return null; }
    const duration = moment.duration(value);
    const hours = duration.hours();
    const minutes = duration.minutes();
    const seconds = duration.seconds();

    let result = '';
    if (hours > 0) { result += `${hours} ${(hours === 1 ? 'hour' : 'hours')}`; }
    if (minutes > 0 || hours > 0) { result += `${(result !== '' ? ', ' : '')}${minutes} ${(minutes === 1 ? 'minute' : 'minutes')}`; }
    result += `${(result !== '' ? ', ' : '')}${seconds} ${(seconds === 1 ? 'second' : 'seconds')}`;

    return result;
  }

  public minutesToString(value: number): string {
    if (value == null) { return null; }
    const hours = Math.floor(value / 60);
    const minutes = value % 60;
    
    let result = '';
    if (hours > 0) { result += `${hours} ${(hours === 1 ? 'hour' : 'hours')}`; }
    result += `${(result !== '' ? ', ' : '')}${minutes} ${(minutes === 1 ? 'minute' : 'minutes')}`;

    return result;
  }


  // [ Helper functions ]
  private getCustomFilterState(state: State, options: { dateFields: string[] }): CompositeFilterDescriptor {
    if (state.filter == null) { return null; }

    const rootFilter: CompositeFilterDescriptor = {
      logic: state.filter.logic,
      filters: state.filter.filters.map((gFilter: CompositeFilterDescriptor) => {
        return {
          logic: gFilter.logic,
          filters: gFilter.filters.map((filter: FilterDescriptor) => {
            return {
              field: filter.field,
              operator: this.getFilterOperatorName(<string>filter.operator),
              value: this.getFilterValue(<string>filter.field, filter.value, options),
              ignoreCase: filter.ignoreCase
            };
          })
        };
      })
    }

    return rootFilter;
  }

  private getFilterOperatorName(kendoOperator: string): string {
    switch (kendoOperator) {
      case 'eq': return FilterOperator[FilterOperator.EQUAL];
      case 'neq': return FilterOperator[FilterOperator.NOT_EQUAL];
      case 'isnull': return FilterOperator[FilterOperator.IS_NULL];
      case 'isnotnull': return FilterOperator[FilterOperator.IS_NOT_NULL];
      case 'lt': return FilterOperator[FilterOperator.LESS_THAN];
      case 'lte': return FilterOperator[FilterOperator.LESS_THAN_OR_EQUAL];
      case 'gt': return FilterOperator[FilterOperator.GREATER_THAN];
      case 'gte': return FilterOperator[FilterOperator.GREATER_THAN_OR_EQUAL];
      case 'startswith': return FilterOperator[FilterOperator.STARTS_WITH];
      case 'endswith': return FilterOperator[FilterOperator.ENDS_WITH];
      case 'contains': return FilterOperator[FilterOperator.CONTAINS];
      case 'isempty': return FilterOperator[FilterOperator.IS_EMPTY];
      case 'isnotempty': return FilterOperator[FilterOperator.IS_NOT_EMPTY];
      default: throw new Error(`Unknown filter operator: ${kendoOperator}`);
    }
  }

  private getFilterValue(field: string, value: any, options: { dateFields: string[] }): any {
    // Null value
    if (value == null) { return null; }

    // Date value
    if (options.dateFields.some(f => f === field)) {
      return moment(value).toISOString();
    }

    // Default value
    return value;
  }
}