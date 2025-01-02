import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CommonMethods {

  setEditorFocus(e: any) {
    setTimeout(() => {
      if (e == null || e == undefined) return;

      if (e.component != undefined || e.component != null) e.component.focus();
      else e.focus();
    }, 100);
  }

}
