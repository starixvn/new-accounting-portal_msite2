import { Pipe, PipeTransform } from '@angular/core';
import * as _ from 'lodash';

// Status code constants
const OPERATION_TIMEOUT_STATUS = 3;
const SETTLEMENT_FAILURE_STATUS = 99;

@Pipe({ name: 'nameStatus' })
export class NameStatus implements PipeTransform {
  transform(value: number): string {
    switch (value) {
      case OPERATION_TIMEOUT_STATUS:
        return '[3] Failed: operation timed out.';
      case SETTLEMENT_FAILURE_STATUS:
        return '[99] Failed: cannot settle all tickets in one action.';
      default:
        return `${value}`;
    }
  }
}
