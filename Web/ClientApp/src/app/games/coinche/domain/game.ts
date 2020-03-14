import { v4 as uuidv4 } from 'uuid';
import { GameService } from '../../../services/game/game.service';
import { Observable, Subscriber } from 'rxjs';
import { CardsEnum } from '../../../typewriter/enums/CardsEnum.enum';
import { Player } from './player';
import { PlayerPosition } from './PlayerPosition';
import { AddCardEvent } from './events/add-card.event';
import { cachedDataVersionTag } from 'v8';

export class Game {
    public gameId: uuidv4;
    public playerId: uuidv4;
    private onNewSpriteSubscriber: Subscriber<AddCardEvent> = new Subscriber<AddCardEvent>();
    private onNewImageSubscriber: Subscriber<AddCardEvent> = new Subscriber<AddCardEvent>();
    private cardWidth = 105;

    private players: Player[] = []
    //private currentPlayer: Player;

    constructor(private gameService: GameService) {
    }

    public setGame(gameId: uuidv4, playerId: uuidv4) {
        this.gameId = gameId;
        this.playerId = playerId;

        //this.currentPlayer = new Player(playerId, PlayerPosition.bottom);
        this.players.push(new Player(playerId, PlayerPosition.bottom));
        this.players.push(new Player(null, PlayerPosition.left));
        this.players.push(new Player(null, PlayerPosition.top));
        this.players.push(new Player(null, PlayerPosition.right));

        this.gameService.startConnection()
            .then(() => this.broadcastGameCardsForPlayer())
            .catch((reason) => this.onSocketInitializationFailed(reason));
    }

    private broadcastGameCardsForPlayer() {
        this.gameService.onPlayerCardsReceived(this.playerId).subscribe({
            next: (datas) => this.displayCards(datas)
        });

        return this.gameService.broadcastGameCardsForPlayer(this.gameId, this.playerId);
    }

    public get onNewSpriteAdded(): Observable<AddCardEvent> {
        return new Observable(observer => this.onNewSpriteSubscriber = observer);
    }

    public get onNewImageAdded(): Observable<AddCardEvent> {
        return new Observable(observer => this.onNewImageSubscriber = observer);
    }

    private displayCards(cards: CardsEnum[]) {
        for (const player of this.players) {
            if (player.id == this.playerId) {
                cards.sort((a, b) => a - b).forEach((spriteNumber, index) => {
                    const spritePosition = player.getSpritePosition(index, this.cardWidth);

                    const cardEvent = new AddCardEvent();
                    cardEvent.x = spritePosition.x;
                    cardEvent.y = spritePosition.y;
                    cardEvent.elementName = 'cards';
                    cardEvent.card = spriteNumber;

                    this.onNewSpriteSubscriber.next(cardEvent);
                });
            }
            else {
                for (let i = 0; i < 8; i++) {
                    const spritePosition = player.getSpritePosition(i, this.cardWidth);

                    const cardEvent = new AddCardEvent();
                    cardEvent.x = spritePosition.x;
                    cardEvent.y = spritePosition.y;
                    cardEvent.elementName = 'cardBack';
                    cardEvent.angle = spritePosition.angle;

                    this.onNewImageSubscriber.next(cardEvent);
                }
            }
        }
    }

    private onSocketInitializationFailed(error: any) {
        console.log("Socket initialisation failed :");
        console.log(error);
        //Todo display error on scene
    }
}
