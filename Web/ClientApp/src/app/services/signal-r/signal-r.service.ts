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
     * @param connectionInitialized Observer to call when connection is initialised.
     */
    public startConnection(connectionInitialized: Observer<void>) {
        this.hubConnection = new HubConnectionBuilder()
            .withUrl(environment.singalRBaseUrl + '/lobby')
            .build();

        this.hubConnection.start()
            .then(() => connectionInitialized.next())
            .catch((reason) => connectionInitialized.error(reason));
    }

    public addLobbyListener(callBack: (...args: any[]) => void) {
        this.hubConnection.on('lobby', callBack);
    }

    public broadcastToLobby(data: any) {
        this.hubConnection.invoke('NewPlayer', data);
    }
}
