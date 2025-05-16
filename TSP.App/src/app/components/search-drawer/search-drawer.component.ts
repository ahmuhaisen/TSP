import { Component, effect, EventEmitter, Input, input, Output, output, signal } from '@angular/core';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDrawerModule } from 'ng-zorro-antd/drawer';
import { NzEmptyComponent } from 'ng-zorro-antd/empty';
import { NzIconModule, NzIconPatchService } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { FormsModule } from '@angular/forms';
@Component({
  selector: 'app-search-drawer',
  imports: [
    NzDrawerModule,
    NzIconModule,
    NzButtonModule,
    NzSpinModule,
    NzInputModule,
    NzSelectModule,
    NzEmptyComponent,
    FormsModule
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

  selectedSearchType: string = "";

  doSearch() {
    console.log(this.selectedSearchType);

    switch(this.selectedSearchType)
    {
      case "1":break;
      case "2":break;
      case "3":break;
    }


    this.isSearchLoading.set(false);
  }

  close() {
    this.visible = false;
    this.visibleChange.emit(this.visible);
  }
}



// public class EventBasicDTO
// {
//     public Guid Id { get; set; }
//     public required string Name { get; set; }
// }
// public class StudentBasicDTO
// {
//     public Guid Id { get; set; }
//     public required string FullName { get; set; }
//     public string? LogoId { get; set; }
// }

// public class SocietyListDTO
// {
//     public Guid Id { get; set; }
//     public required string Name { get; set; }
//     public string? Description { get; set; }
//     public required string LogoId { get; set; }
//     public DateOnly CreationDate { get; set; }
//     public string? ThemeColor { get; set; }
// }
