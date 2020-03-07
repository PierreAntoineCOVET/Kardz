import { Player } from './player';
import { v4 as uuidv4 } from 'uuid';
import { Observable, of } from 'rxjs';

/**
 * Lobby domain actions.
 */
export class Lobby {
    private player: Player;

    /**
     * Add the host player (generate it's own ID).
     */
    public addPlayer(): Observable<uuidv4> {
        const player = new Player(uuidv4());
        this.player = player;

        return of(player.id);
    }

    /**
     * Get current player's id.
     */
    public get playerId(): uuidv4 {
        return this.player.id;
    }
}
