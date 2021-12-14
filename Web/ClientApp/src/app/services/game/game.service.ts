import { HubConnectionBuilder, HubConnection, HubConnectionState } from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { IGameContractDto } from 'src/app/typewriter/classes/GameContractDto';
import { IGameInitDto } from 'src/app/typewriter/classes/GameInitDto';
import { environment } from 'src/environments/environment';

export class GameService {
    private hubConnection: HubConnection;

    /**
     * Use of a subject because data can be received from the server without being a response to a request (push).
     */
    public onGameInformationsReceived: Subject<IGameInitDto> = new Subject<IGameInitDto>();

    public onGameContractChanged: Subject<IGameContractDto> = new Subject<IGameContractDto>();

    constructor(playerId: string) {
        this.hubConnection = new HubConnectionBuilder()
            .withUrl(environment.singalRBaseUrl + '/game', { accessTokenFactory: () => playerId })
            .withAutomaticReconnect()
            .build();

        this.hubConnection.on('gameInformationsReceived', (data) => {
            this.onGameInformationsReceived.next(data);
        });

        this.hubConnection.on('gameContractChanged', (data) => this.onGameContractChanged.next(data));
    }

    /**
     * Start SingalR connection and call connectionInitialized on succes or error.
     */
    private startConnection(): Promise<void> {
        if (this.hubConnection.state == HubConnectionState.Disconnected) {
            return this.hubConnection.start().catch((reason) => {
                console.error("Socket initialisation failed : ");
                console.error(reason);
            });
        }
        else if (this.hubConnection.state == HubConnectionState.Connected) {
            return Promise.resolve();
        }
        else {
            throw new Error(`Error while getting worker signal connection : ${this.hubConnection.state}`);
        }
    }

    public async broadcastGetGameInformations(gameId: string, playerId: string) {
        await this.startConnection();

        this.hubConnection.send('GetGameInformations', gameId, playerId);
    }

    public async broadcastSetGameContract(gameId: string, playerId: string, selectedColor: number, selectedValue: number, coinched: boolean) {
        await this.startConnection();

        this.hubConnection.send('SetGameContract', selectedColor, selectedValue, gameId, playerId, coinched);
    }
}
