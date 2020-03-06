import { Player } from './player';
import { v4 as uuidv4 } from 'uuid';
import { Observable, of } from 'rxjs';

export class Lobby {
    private players: Player[] = [];

    public get getNumberOfPlayers(): string {
        return this.players.length.toString();
    }

    public addCurrentPlayer() : Observable<any> {
        const player = new Player(uuidv4());
        this.players.push(player);

        return of(player.id);
    }

    public addNewPlayer(playerId: uuidv4) {
        for (let player of this.players) {
            if (player.id === playerId)
                return;
        }

        const player = new Player(playerId);
        this.players.push(player);
    }
}
