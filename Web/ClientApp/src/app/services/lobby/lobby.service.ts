import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { environment } from '../../../environments/environment';
import { v4 as uuidv4 } from 'uuid';

export class LobbyService {
    private hubConnection: HubConnection;

    /** Emit when a new player enter the lobby, return the number of players in the lobby. */
    public onNewPlayer: Subject<number> = new Subject<number>();

    /** Emmit when the current player's game is starting, return the game's Id. */
    public onGameStarted: Subject<uuidv4> = new Subject<uuidv4>();

    constructor(playerId: uuidv4) {
        this.hubConnection = new HubConnectionBuilder()
            .withUrl(environment.singalRBaseUrl + '/lobby', { accessTokenFactory: () => playerId })
            .withAutomaticReconnect()
            .build();

        this.hubConnection.on('playersInLobby', (data) => this.onNewPlayer.next(data));

        this.hubConnection.on('gameStarted', (data) => this.onGameStarted.next(data));
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

    /**
     * Send current player's data to the hub.
     * @param data player's id.
     */
    public async broadcastNewPlayer(data: uuidv4) {
        await this.startConnection();

        this.hubConnection.invoke('AddNewPlayer', data);
    }

    /**
     * Record current player as ready for game.
     * @param data player's id.
     */
    public async broadcastSearchGame(data: uuidv4) {
        await this.startConnection();

        this.hubConnection.invoke('SearchGame', data);
    }
}
