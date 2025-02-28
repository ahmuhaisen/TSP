import { Component, effect, EventEmitter, Input, input, Output, output, signal } from '@angular/core';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDrawerModule } from 'ng-zorro-antd/drawer';
import { NzEmptyComponent } from 'ng-zorro-antd/empty';
import { NzIconModule, NzIconPatchService } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzSpinModule } from 'ng-zorro-antd/spin';

@Component({
  selector: 'app-search-drawer',
  imports: [
    NzDrawerModule,
    NzIconModule,
    NzButtonModule,
    NzSpinModule,
    NzInputModule,
    NzSelectModule,
    NzEmptyComponent
  ],
  templateUrl: './search-drawer.component.html',
  styleUrl: './search-drawer.component.css'
})
export class SearchDrawerComponent {

  isSearchLoading = signal(false);
  // Parent component sends the initial visible value.
  @Input() visible: boolean = false;
  // This EventEmitter lets the child notify the parent of changes.
  @Output() visibleChange = new EventEmitter<boolean>();

  doSearch(){
    this.isSearchLoading.set(true);
  }

  close() {
    this.visible = false;
    this.visibleChange.emit(this.visible);
  }
}
