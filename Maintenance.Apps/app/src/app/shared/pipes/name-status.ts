import { Pipe, PipeTransform } from '@angular/core';
import * as _ from 'lodash';

@Pipe({ name: 'nameStatus' })
export class NameStatus implements PipeTransform {
  transform(value: number): string {
    switch (value) {
      case 3:
        return '[3] Failed: operation timed out.';
      case 99:
        return '[99] Failed: cannot settle all tickets in one action.';
      default:
        return `${value}`;
    }
  }
}
