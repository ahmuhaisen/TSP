import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'highlight',
  standalone: true
})
export class HighlightPipe implements PipeTransform {
  constructor() { }

  transform(value: string, args: string): any {
    if (args && value) {
      let pattern = args.replace(/[\-\[\]\/\{\}\(\)\*\+\?\.\\\^\$\|]/g, '\\$&');
      
      const regex = new RegExp(pattern, 'gi');
      return value.replace(regex, (match) => `<span class='highlight'>${match}</span>`);
    } else {
      return value;
    }
  }

}
