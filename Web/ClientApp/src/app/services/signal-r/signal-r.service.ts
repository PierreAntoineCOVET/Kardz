import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { environment } from '../../../environments/environment';
import { Observable, Observer } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class SignalRService {
    private hubConnection: HubConnection;

    constructor() { }

    /**
     * Start SingalR connection and call connectionInitialized on succes or error.
     */
    public startConnection(): Promise<void> {
        this.hubConnection = new HubConnectionBuilder()
            .withUrl(environment.singalRBaseUrl + '/lobby')
            .build();

        return this.hubConnection.start();
    }

    public addNewPLayerListener(callBack: (...args: any[]) => void) {
        this.hubConnection.on('newPlayer', callBack);
    }

    public broadcastNewPlayer(data: any) {
        this.hubConnection.invoke('NewPlayer', data);
    }
}
