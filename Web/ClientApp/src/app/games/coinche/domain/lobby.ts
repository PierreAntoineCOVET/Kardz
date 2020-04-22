import { v4 as uuidv4 } from 'uuid';
import { Observable, of, Subscription, Subscriber, Subject } from 'rxjs';
import { LobbyService } from '../../../services/lobby/lobby.service';
import { sha256 } from 'js-sha256';
import { GameStartedEvent } from './events/game-started.event';
import { environment } from '../../../../environments/environment';

/**
 * Lobby domain actions.
 */
export class Lobby {
    //private player: Player;
    private playerId: uuidv4;
    private readonly PLAYER_ID_STORAGE_KEY = 'PlayerIdKey';
    private subscriptions: Subscription;
    public onNewGameSubscriber: Subject<GameStartedEvent> = new Subject<GameStartedEvent>();

    constructor(private lobbyService: LobbyService) {
        this.playerId = this.getOrCreatePlayerId();

        this.lobbyService.startConnection(this.playerId)
            .then(() => this.addPlayer())
            .catch((reason) => this.onSocketInitializationFailed(reason));
    }

    /**
     * Get the player id from local storage or create one if there is none.
     */
    private getOrCreatePlayerId(): uuidv4 {
        if (environment.production) {
            let currentPlayerId = localStorage.getItem(this.PLAYER_ID_STORAGE_KEY);

            if (!currentPlayerId) {
                currentPlayerId = uuidv4();
                localStorage.setItem(this.PLAYER_ID_STORAGE_KEY, currentPlayerId);
            }

            return currentPlayerId;
        }
        else {
            return uuidv4();
        }
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
        this.lobbyService.broadcastNewPlayer(this.playerId)
    }

    /**
     * Record the current player as one searching a game.
     */
    public searchGame() {
        this.lobbyService.broadcastSearchGame(this.playerId);
        this.subscriptions = this.lobbyService.onGameStarted.subscribe({
            next: (data) => {
                if (data) {
                    const gameStartedEvent = new GameStartedEvent();
                    gameStartedEvent.gameId = data;
                    gameStartedEvent.playerId = this.playerId;
                    this.onNewGameSubscriber.next(gameStartedEvent);
                    this.onNewGameSubscriber.complete();

                    //const hashedPlayerId = sha256(this.playerId);
                }
            }
        });
    }

    /**
     * Remove all subscriptions.
     */
    public onDestroy() {
        this.subscriptions.unsubscribe();
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
