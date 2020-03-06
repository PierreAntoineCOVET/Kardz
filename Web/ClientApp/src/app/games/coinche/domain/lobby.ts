import { Player } from './player';
import { v4 as uuidv4 } from 'uuid';
import { Observable, of } from 'rxjs';

/**
 * Lobby domain actions.
 */
export class Lobby {
    private players: Player[] = [];

    /**
     * Get the number of register players.
     */
    public get getNumberOfPlayers(): string {
        return this.players.length.toString();
    }

    /**
     * Add the host player (generate it's own ID).
     */
    public addPlayer() : Observable<any> {
        const player = new Player(uuidv4());
        this.players.push(player);

        return of(player.id);
    }
}
