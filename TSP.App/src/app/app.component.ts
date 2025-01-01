import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzMessageService } from 'ng-zorro-antd/message';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    NzButtonModule,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'The Societies Portal';

  messageService = inject(NzMessageService);

  createMessage(){
    this.messageService.info('The Societies Portal!');
  }
}
