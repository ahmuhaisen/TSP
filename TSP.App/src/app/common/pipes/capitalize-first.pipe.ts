import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'capitalizefirst',
  standalone: true
})
export class CapitalizeFirstPipe implements PipeTransform {

  transform(value: string): string {
    if (value === null) return value;
    return value.charAt(0).toUpperCase() + value.slice(1).toLowerCase();
  }
}
