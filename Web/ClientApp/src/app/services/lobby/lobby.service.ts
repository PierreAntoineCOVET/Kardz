import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { environment } from '../../../environments/environment';
export class LobbyService {
    private hubConnection: HubConnection;

    /** Emit when a new player enter the lobby, return the number of players in the lobby. */
    public onNewPlayer: Subject<number> = new Subject<number>();

    /** Emmit when the current player's game is starting, return the game's Id. */
    public onGameStarted: Subject<string> = new Subject<string>();

    constructor(playerId: string) {
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
    public async broadcastNewPlayer(data: string) {
        await this.startConnection();

        this.hubConnection.invoke('AddNewPlayer', data);
    }

    /**
     * Record current player as ready for game.
     * @param data player's id.
     */
    public async broadcastSearchGame(data: string) {
        await this.startConnection();

        this.hubConnection.invoke('SearchGame', data);
    }
}
