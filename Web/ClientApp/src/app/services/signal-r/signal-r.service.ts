import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { environment } from '../../../environments/environment';
import { Observable, Observer } from 'rxjs';
import { GameDto } from '../../typewriter/classes/GameDto';

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

    public get onNewPlayer(): Observable<any> {
        return new Observable(subscriber => {
            this.hubConnection.on('playersInLobby', (data) => subscriber.next(data));
        });
    }

    public get onGameStarted(): Observable<GameDto> {
        return new Observable(subscriber => {
            this.hubConnection.on('gameStarted', (data) => subscriber.next(data));
        });
    }

    public broadcastNewPlayer(data: any) {
        this.hubConnection.invoke('AddNewPlayer', data);
    }

    public broadcastSearchGame(data: any) {
        this.hubConnection.invoke('SearchGame', data);
    }
}
