import { Player } from './player';
import { v4 as uuidv4 } from 'uuid';
import { Observable, of, Subscription, Subscriber } from 'rxjs';
import { GameDto } from '../../../typewriter/classes/GameDto';
import { LobbyService } from '../../../services/lobby/lobby.service';
import { sha256 } from 'js-sha256';

/**
 * Lobby domain actions.
 */
export class Lobby {
    private player: Player;
    public onNewGameSubscriber: Subscriber<GameDto> = new Subscriber<GameDto>();
    private onNewGameStart: Subscription;

    constructor(private lobbyService: LobbyService) {
    }

    public onNewPlayerToLobby(): Observable<number> {
        return this.lobbyService.onNewPlayer;
    }

    /**
     * Add the host player (generate it's own ID).
     */
    public addPlayer() {
        const player = new Player(uuidv4());
        this.player = player;

        this.lobbyService.broadcastNewPlayer(player.id)
    }

    /**
     * Record the current player as one searching a game.
     */
    public searchGame() {
        this.lobbyService.broadcastSearchGame(this.player.id);
        this.onNewGameStart = this.lobbyService.onGameStarted.subscribe({
            next: (data) => {
                if (data) {
                    const hashedPlayerId = sha256(this.player.id);

                    if (data.players.indexOf(hashedPlayerId) > -1) {
                        this.onNewGameSubscriber.next(data.id);
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
}
