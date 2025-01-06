import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'truncate',
  standalone: true
})
export class TruncatePipe implements PipeTransform {

  transform(value: string, length: number): string {
    if (!value) return '';
    if(value.length <= length) return value;
    return value.substring(0, length).trim() + '...';
  }
  
}
