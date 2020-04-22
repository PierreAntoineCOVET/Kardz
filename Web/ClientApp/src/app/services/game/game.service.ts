import { Injectable } from '@angular/core';
import { v4 as uuidv4 } from 'uuid';
import { Observable } from 'rxjs';
import { CardsEnum } from '../../typewriter/enums/CardsEnum.enum';
import { HubConnectionBuilder, HubConnection } from '@aspnet/signalr';
import { environment } from '../../../environments/environment';
import { IGameInitDto } from '../../typewriter/classes/GameInitDto';

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

    public broadcastGetGameInformations(gameId: uuidv4, playerId: uuidv4) {
        this.hubConnection.invoke('GetGameInformations', gameId, playerId);
    }

    public get onGameInformationsReceived(): Observable<IGameInitDto> {
        return new Observable(subscriber => {
            this.hubConnection.on('gameInformationsReceived', (data) => subscriber.next(data));
        });
    }
}
