import { v4 as uuidv4 } from 'uuid';
import { GameService } from '../../../services/game/game.service';
import { Observable, Subscriber, Subscription, ReplaySubject, Subject, BehaviorSubject } from 'rxjs';
import { CardsEnum } from '../../../typewriter/enums/CardsEnum.enum';
import { Player } from './player';
import { ScreenCoordinate } from './PlayerPosition';
import { AddCardEvent } from './events/add-card.event';
import { cachedDataVersionTag } from 'v8';
import { IGameInitDto } from '../../../typewriter/classes/GameInitDto';
import { ChooseContractEvent } from './events/choose-contract.event';
import { DealerSelectedEvent } from './events/dealer-selected.event';
import { StartTurnTimerEvent, TurnTimerTickedEvent } from './events/turn-timer.event';

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

    /**
     * Emit when the real player is ready to vote for its contract.
     */
    public onPlayerChooseContractSubscriber: BehaviorSubject<ChooseContractEvent> = new BehaviorSubject<ChooseContractEvent>(undefined);

    /**
     * Emit when the game card's dealer is defined.
     */
    public onDealerSetSubscriber: BehaviorSubject<DealerSelectedEvent> = new BehaviorSubject<DealerSelectedEvent>(undefined);

    /**
     * Emit when a player's turn start.
     */
    public onTurnTimeStartedSubscriber: BehaviorSubject<StartTurnTimerEvent> = new BehaviorSubject<StartTurnTimerEvent>(undefined);

    /**
     * Emit each seconds of a player's turn.
     */
    public onTurnTimerTickedSubscriber: BehaviorSubject<TurnTimerTickedEvent> = new BehaviorSubject<TurnTimerTickedEvent>(undefined);

    private subscriptions: Subscription;
    private cardWidth = 105;

    private players: Player[] = []

    private readonly turnMaxTimeSecond = 30;

    private turnTimer: NodeJS.Timeout;
    private currentLoopIteration: number;

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

        this.players.push(new Player(playerId, ScreenCoordinate.bottom));
        this.players.push(new Player(null, ScreenCoordinate.left));
        this.players.push(new Player(null, ScreenCoordinate.top));
        this.players.push(new Player(null, ScreenCoordinate.right));

        this.gameService.startConnection(this.playerId)
            .then(() => this.broadcastGameCardsForPlayer())
            .catch((reason) => this.onSocketInitializationFailed(reason));
    }

    /**
     * Request game cards for the player.
     */
    private broadcastGameCardsForPlayer() {
        this.subscriptions = this.gameService.onGameInformationsReceived.subscribe({
            next: (datas) => {
                this.initGame(datas);
            }
        });

        return this.gameService.broadcastGetGameInformations(this.gameId, this.playerId);
    }

    /**
     * Init all game datas.
     * @param gameDatas
     */
    private initGame(gameDatas: IGameInitDto) {
        this.displayCards(gameDatas.playerCards);
        this.setPlayersNumber(gameDatas.playerNumber);
        this.setDealer(gameDatas.dealer);
        this.setCurrentPlayerPlaying(gameDatas.playerPlaying);

        let currentPlayer = this.players.find(p => p.id == this.playerId);
        if (currentPlayer.isPlaying) {
            var contractEvent = new ChooseContractEvent();
            this.onPlayerChooseContractSubscriber.next(contractEvent);
        }

        this.startTurnTimer(gameDatas.playerPlaying);
    }

    private startTurnTimer(currentPlayerNumber: number) {
        let currentPlayer = this.players.find(p => p.number == currentPlayerNumber);
        let startEvent = currentPlayer.getTurnTimerPosition();
        this.onTurnTimeStartedSubscriber.next(startEvent);

        this.currentLoopIteration = 0;

        this.turnTimer = setInterval(
            () => {
                const completion = Math.round(this.currentLoopIteration * 100 / this.turnMaxTimeSecond);
                this.currentLoopIteration++;

                const event = new TurnTimerTickedEvent();
                event.percentage = completion;

                this.onTurnTimerTickedSubscriber.next(event);

                if (completion == 100) {
                    // force a random play
                    clearInterval(this.turnTimer);
                }
            },
            1000);
    }

    /**
     * Indicate which player has to play for the current turn.
     * @param currentPlayerPlaying number of the player who has to play.
     */
    private setCurrentPlayerPlaying(currentPlayerPlaying: number) {
        this.players.find(p => p.number == currentPlayerPlaying).isPlaying = true;
    }

    /**
     * Set the property isDealer to true for the current dealer.
     * @param dealerNumber
     */
    private setDealer(dealerNumber: number) {
        const dealer = this.players.find(p => p.number == dealerNumber);
        const position = dealer.getDealerChipPosition();

        let dealerChipEvent = new DealerSelectedEvent();
        dealerChipEvent.x = position.x;
        dealerChipEvent.y = position.y;

        this.onDealerSetSubscriber.next(dealerChipEvent);
    }

    /**
     * Set players number.
     * Current player take the number returned in GameInfo and the others are
     * set clockwise (ex: bottom=1, left = 2, top = 3, right = 0).
     * @param playerNumber number of the current real player.
     */
    private setPlayersNumber(playerNumber: number) {
        this.players.find(p => p.position == ScreenCoordinate.bottom).number = playerNumber;
        this.players.find(p => p.position == ScreenCoordinate.left).number = (playerNumber + 1) % 4;
        this.players.find(p => p.position == ScreenCoordinate.top).number = (playerNumber + 2) % 4;
        this.players.find(p => p.position == ScreenCoordinate.right).number = (playerNumber + 3) % 4;
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
