import { Pipe, PipeTransform } from '@angular/core';
import * as _ from 'lodash';

@Pipe({ name: 'replaceLineBreaks' })
export class ReplaceLineBreaks implements PipeTransform {
  transform(value: string): string {
    return _.isEmpty(value) ? '' : value.replace(/\\n/g, '<br/>');
  }
}
