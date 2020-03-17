import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { GameDto } from '../../typewriter/classes/GameDto';

@Injectable({
    providedIn: 'root'
})
export class LobbyService {
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

    /**
     * Emit when a new player enter the lobby.
     */
    public get onNewPlayer(): Observable<number> {
        return new Observable(subscriber => {
            this.hubConnection.on('playersInLobby', (data) => subscriber.next(data));
        });
    }

    /**
     * Emit when a new game is started.
     */
    public get onGameStarted(): Observable<GameDto> {
        return new Observable(subscriber => {
            this.hubConnection.on('gameStarted', (data) => subscriber.next(data));
        });
    }

    /**
     * Send current player's data to the hub.
     * @param data
     */
    public broadcastNewPlayer(data: any) {
        this.hubConnection.invoke('AddNewPlayer', data);
    }

    /**
     * Record current player as ready for game.
     * @param data
     */
    public broadcastSearchGame(data: any) {
        this.hubConnection.invoke('SearchGame', data);
    }
}
