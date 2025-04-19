import { inject, Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../../environments/environment';
import { SecureLocalStorageService } from './secure-local-storage.service';
import { IGenericNotification } from '../types/notification.types';

@Injectable({
  providedIn: 'root',
})
export class NotificationHubService {
  private hubConnection!: signalR.HubConnection;
  localStorageService = inject(SecureLocalStorageService);

  public startConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.apiURL}hubs/notifications`, {
        accessTokenFactory: () => this.localStorageService.getItem('token') || ''
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('✅ SignalR Connected'))
      .catch(err => console.error('❌ SignalR Error: ', err));
  }

  public onNotification(callback: (data: IGenericNotification) => void): void {
    this.hubConnection.on('ReceiveNotification', callback);
  }

  public stopConnection(): void {
    if (this.hubConnection) {
      this.hubConnection.stop().then(() => console.log('🔌 SignalR Disconnected'));
    }
  }
}
