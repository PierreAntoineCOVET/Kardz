import { v4 as uuidv4 } from 'uuid';
import { Observable, of, Subscription, Subscriber, Subject } from 'rxjs';
import { LobbyService } from '../../../services/lobby/lobby.service';
import { sha256 } from 'js-sha256';
import { GameStartedEvent } from './events/game-started.event';

/**
 * Lobby domain actions.
 */
export class Lobby {
    //private player: Player;
    private playerId: uuidv4;
    public onNewGameSubscriber: Subject<GameStartedEvent> = new Subject<GameStartedEvent>();

    constructor(private lobbyService: LobbyService) {
        this.lobbyService.startConnection()
            .then(() => this.addPlayer())
            .catch((reason) => this.onSocketInitializationFailed(reason));
    }

    /**
     * Emit when a new player enter the lobby.
     */
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
        this.lobbyService.onGameStarted.subscribe({
            next: (data) => {
                if (data) {
                    const hashedPlayerId = sha256(this.playerId);

                    if (data.players.indexOf(hashedPlayerId) > -1) {
                        const gameStartedEvent = new GameStartedEvent();
                        gameStartedEvent.gameId = data.id;
                        gameStartedEvent.playerId = this.playerId;
                        this.onNewGameSubscriber.next(gameStartedEvent);
                        this.onNewGameSubscriber.complete();
                    }
                }
            }
        });
    }

    /**
     * Remove all subscriptions.
     */
    public onDestroy() {
        //this.onNewGameStart.unsubscribe();
    }

    /**
     * Called if connection initialisation failled.
     * @param error
     */
    private onSocketInitializationFailed(error: any) {
        console.log("Socket initialisation failed :");
        console.log(error);
    }
}
