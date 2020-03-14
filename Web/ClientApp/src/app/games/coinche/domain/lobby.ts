import { v4 as uuidv4 } from 'uuid';
import { Observable, of, Subscription, Subscriber } from 'rxjs';
import { LobbyService } from '../../../services/lobby/lobby.service';
import { sha256 } from 'js-sha256';
import { GameStartedEvent } from './events/game-started.event';

/**
 * Lobby domain actions.
 */
export class Lobby {
    //private player: Player;
    private playerId: uuidv4;
    public onNewGameSubscriber: Subscriber<GameStartedEvent> = new Subscriber<GameStartedEvent>();
    private onNewGameStart: Subscription;

    constructor(private lobbyService: LobbyService) {
        this.lobbyService.startConnection()
            .then(() => this.addPlayer())
            .catch((reason) => this.onSocketInitializationFailed(reason));
    }

    public onNewPlayerToLobby(): Observable<number> {
        return this.lobbyService.onNewPlayer;
    }

    /**
     * Add the host player (generate it's own ID).
     */
    private addPlayer() {
        this.playerId = uuidv4();

        this.lobbyService.broadcastNewPlayer(this.playerId)
    }

    /**
     * Record the current player as one searching a game.
     */
    public searchGame() {
        this.lobbyService.broadcastSearchGame(this.playerId);
        this.onNewGameStart = this.lobbyService.onGameStarted.subscribe({
            next: (data) => {
                if (data) {
                    const hashedPlayerId = sha256(this.playerId);

                    if (data.players.indexOf(hashedPlayerId) > -1) {
                        const gameStartedEvent = new GameStartedEvent();
                        gameStartedEvent.gameId = data.id;
                        gameStartedEvent.playerId = this.playerId;
                        this.onNewGameSubscriber.next(gameStartedEvent);
                    }
                }
            }
        });
    }

    /**
     * Remove all signalr listeners.
     */
    public stopSubscriptions() {
        this.onNewGameStart.unsubscribe();
    }

    private onSocketInitializationFailed(error: any) {
        console.log("Socket initialisation failed :");
        console.log(error);
        //Todo display error on scene
    }
}
