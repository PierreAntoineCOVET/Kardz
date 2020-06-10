import { Injectable } from '@angular/core';
import { v4 as uuidv4 } from 'uuid';
import { BehaviorSubject } from 'rxjs';
import { HubConnectionBuilder, HubConnection } from '@microsoft/signalr';
import { environment } from '../../../environments/environment';
import { IGameInitDto } from '../../typewriter/classes/GameInitDto';
import { ICoincheContractDto } from '../../typewriter/classes/CoincheContractDto';

@Injectable({
  providedIn: 'root'
})
export class GameService {
    private hubConnection: HubConnection;

    public onGameInformationsReceived: BehaviorSubject<IGameInitDto> = new BehaviorSubject<IGameInitDto>(undefined);

    public onGameContractChanged: BehaviorSubject<ICoincheContractDto> = new BehaviorSubject<ICoincheContractDto>(undefined);

    constructor() { }

    /**
     * Start SingalR connection and call connectionInitialized on succes or error.
     */
    public startConnection(playerId: uuidv4): Promise<void> {
        this.hubConnection = new HubConnectionBuilder()
            .withUrl(environment.singalRBaseUrl + '/game', { accessTokenFactory: () => playerId })
            //.withAutomaticReconnect()
            .build();

        this.hubConnection.on('gameInformationsReceived', (data) => {
            console.log('GameService event : ');
            console.log(data);
            this.onGameInformationsReceived.next(data);
        });
        this.hubConnection.on('gameContractChanged', (data) => this.onGameContractChanged.next(data));

        return this.hubConnection.start();
    }

    public broadcastGetGameInformations(gameId: uuidv4, playerId: uuidv4) {
        this.hubConnection.invoke('GetGameInformations', gameId, playerId);
    }

    public broadcastSetGameContract(gameId: uuidv4, playerId: uuidv4, selectedColor: number, selectedValue: number) {
        this.hubConnection.invoke('SetGameContract', selectedColor, selectedValue, gameId, playerId);
    }

    public broadcastPassGameContract(gameId: uuidv4, playerId: uuidv4,) {
        this.hubConnection.invoke('PassGameContract', gameId, playerId);
    }
}
