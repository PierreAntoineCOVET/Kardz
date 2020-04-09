import { v4 as uuidv4 } from 'uuid';
import { GameService } from '../../../services/game/game.service';
import { Observable, Subscriber, Subscription, ReplaySubject } from 'rxjs';
import { CardsEnum } from '../../../typewriter/enums/CardsEnum.enum';
import { Player } from './player';
import { PlayerPosition } from './PlayerPosition';
import { AddCardEvent } from './events/add-card.event';
import { cachedDataVersionTag } from 'v8';

export class Game {
    public gameId: uuidv4;
    public playerId: uuidv4;

    /**
     * Emit when a new sprite need to be added (current player has a new card).
     */
    public onNewSpriteSubscriber: ReplaySubject<AddCardEvent> = new ReplaySubject<AddCardEvent>();

    /**
     * Emit when a new image need to be added (other players received a new card).
     */
    public onNewImageSubscriber: ReplaySubject<AddCardEvent> = new ReplaySubject<AddCardEvent>();

    private subscriptions: Subscription;
    private cardWidth = 105;

    private players: Player[] = []

    constructor(private gameService: GameService) {
    }

    /**
     * Add players and request for shuffled cards.
     * @param gameId Game ID.
     * @param playerId Player ID.
     */
    public setGame(gameId: uuidv4, playerId: uuidv4) {
        this.gameId = gameId;
        this.playerId = playerId;

        this.players.push(new Player(playerId, PlayerPosition.bottom));
        this.players.push(new Player(null, PlayerPosition.left));
        this.players.push(new Player(null, PlayerPosition.top));
        this.players.push(new Player(null, PlayerPosition.right));

        this.gameService.startConnection(this.playerId)
            .then(() => this.broadcastGameCardsForPlayer())
            .catch((reason) => this.onSocketInitializationFailed(reason));
    }

    /**
     * Request game cards for the player.
     */
    private broadcastGameCardsForPlayer() {
        this.subscriptions = this.gameService.onPlayerCardsReceived.subscribe({
            next: (datas) => this.displayCards(datas)
        });

        return this.gameService.broadcastGameCardsForPlayer(this.gameId, this.playerId);
    }

    /**
     * Compute card (sprite or image) location based on player position.
     * @param cards List of cards for the current player, other players all have 'back' image.
     */
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

    /**
     * Destroy all subscriptions.
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
