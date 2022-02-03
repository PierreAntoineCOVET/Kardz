import { v4 as uuidv4 } from 'uuid';
import { Subject } from 'rxjs';
import { GameStartedEvent } from 'src/app/games/coinche/domain/events/game-started.event';
import { environment } from 'src/environments/environment';

/**
 * Lobby domain actions.
 */
export class Lobby {
    private playerId: string;
    private readonly PLAYER_ID_STORAGE_KEY = 'PlayerIdKey';
    private readonly lobbyWorkerService: Worker;

    /** Emmit when the current player's game is starting, return the game's Id. */
    public onNewGameStarting: Subject<GameStartedEvent> = new Subject<GameStartedEvent>();

    /** Emit when a new player enter the lobby, return the number of players in the lobby. */
    public onNewPlayerToLobby: Subject<number> = new Subject<number>();

    constructor() {
        this.playerId = this.getOrCreatePlayerId();

        this.lobbyWorkerService = new Worker(new URL('../../../services/lobby/lobby.worker', import.meta.url), { name: 'lobby', type: 'module' });

        this.lobbyWorkerService.onerror = (evt) => {
            console.error('Worker error :');
            console.error(evt);
        };

        this.lobbyWorkerService.onmessage = (message) => {
            if (!message || !message.data) {
                return;
            }

            if (this.isValidUuidV4(message.data)) {
                const gameStartedEvent = {
                    gameId: message.data,
                    playerId: this.playerId
                } as GameStartedEvent;

                this.onNewGameStarting.next(gameStartedEvent);
            }
            else if (typeof message.data === "number") {
                this.onNewPlayerToLobby.next(message.data);
            }
        };

        this.lobbyWorkerService.postMessage({
            playerId: this.playerId,
            fName: 'broadcastNewPlayer'
        });
    }

    private isValidUuidV4(obj: any): boolean {
        return typeof obj === 'string' && obj.length === 36;
    }

    /**
     * Get the player id from local storage or create one if there is none.
     */
    private getOrCreatePlayerId(): string {
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
     * Record the current player as one searching a game.
     */
    public searchGame() {
        this.lobbyWorkerService.postMessage({
            playerId: this.playerId,
            fName: 'broadcastSearchGame'
        });
    }

    /**
     * Remove all subscriptions.
     */
    public onDestroy() {
        this.lobbyWorkerService.postMessage({
            fName: 'destroy'
        });

        this.onNewGameStarting.complete();
        this.onNewPlayerToLobby.complete();
    }
}
