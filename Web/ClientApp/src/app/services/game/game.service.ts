import { Injectable } from '@angular/core';
import { v4 as uuidv4 } from 'uuid';
import { Observable } from 'rxjs';
import { CardsEnum } from '../../typewriter/enums/CardsEnum.enum';
import { HubConnectionBuilder, HubConnection } from '@aspnet/signalr';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class GameService {
    private hubConnection: HubConnection;

    constructor() { }

    /**
     * Start SingalR connection and call connectionInitialized on succes or error.
     */
    public startConnection(playerId: uuidv4): Promise<void> {
        this.hubConnection = new HubConnectionBuilder()
            .withUrl(environment.singalRBaseUrl + '/game', { accessTokenFactory: () => playerId })
            .build();

        return this.hubConnection.start();
    }

    public broadcastGameCardsForPlayer(gameId: uuidv4, playerId: uuidv4) {
        this.hubConnection.invoke('GetCardsForPlayer', gameId, playerId);
    }

    public get onPlayerCardsReceived(): Observable<CardsEnum[]> {
        return new Observable(subscriber => {
            this.hubConnection.on('playerCardsReceived', (data) => subscriber.next(data));
        });
    }
}
