import { v4 as uuidv4 } from 'uuid';
import { Subscription, ReplaySubject, Subject } from 'rxjs';
import { ICoincheContractDto } from 'src/app/typewriter/classes/CoincheContractDto';
import { CardsEnum } from 'src/app/typewriter/enums/CardsEnum.enum';
import { IGameInitDto } from 'src/app/typewriter/classes/GameInitDto';
import { ContractEvent } from 'src/app/games/coinche/domain/events/contract.event';
import { DealerSelectedEvent } from 'src/app/games/coinche/domain/events/dealer-selected.event';
import { AddCardEvent } from 'src/app/games/coinche/domain/events/add-card.event';
import { ScreenCoordinate } from 'src/app/games/coinche/domain/PlayerPosition';
import { StartTurnTimerEvent, TurnTimerTickedEvent } from 'src/app/games/coinche/domain/events/turn-timer.event';
import { Player } from 'src/app/games/coinche/domain/player';
//import { GameService } from 'src/app/services/game/game.service';

export class Game {
    public gameId: uuidv4;
    public playerId: uuidv4;

    /**
     * Emit when a new sprite need to be added (current player has a new card).
     */
    public onCurrentPlayerCardReceived: ReplaySubject<AddCardEvent> = new ReplaySubject<AddCardEvent>();

    /**
     * Emit when a new image need to be added (other players received a new card).
     */
    public onOtherPlayerCardReceived: ReplaySubject<AddCardEvent> = new ReplaySubject<AddCardEvent>();

    /**
     * Emit when the real player is ready to vote for its contract.
     */
    public onPlayerReadyToBet: Subject<ContractEvent> = new Subject<ContractEvent>();

    /**
     * Emit when the game card's dealer is defined.
     */
    public onDealerDefined: Subject<DealerSelectedEvent> = new Subject<DealerSelectedEvent>();

    /**
     * Emit when a player's turn start.
     */
    public onTurnTimeStarted: Subject<StartTurnTimerEvent> = new Subject<StartTurnTimerEvent>();

    /**
     * Emit each seconds of a player's turn.
     */
    public onTurnTimerTicked: Subject<TurnTimerTickedEvent> = new Subject<TurnTimerTickedEvent>();

    private subscriptions: Subscription;
    private cardWidth = 105;

    private players: Player[] = []

    /** Number of seconds for the player's turn. */
    private readonly turnTimerTimespan = 30;

    private turnTimer: NodeJS.Timeout;
    private currentLoopIteration: number;

    private readonly gameWorkerService: Worker;

    constructor() {
        this.gameWorkerService = new Worker('../../../services/game/game.worker', { name: 'game', type: 'module' });

        this.gameWorkerService.onerror = (evt) => {
            console.error('Worker error :');
            console.error(evt);
        };

        this.gameWorkerService.onmessage = (message) => {
            if (!message || !message.data) {
                return;
            }

            if (this.isGameInitDto(message.data)) {
                this.initGame(message.data);
            }
            else if (this.isICoincheContractDto(message.data)) {
                this.gameContractChanged(message.data);
            }
        };
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

        this.gameWorkerService.postMessage({
            playerId: this.playerId,
            gameId: this.gameId,
            fName: 'broadcastGetGameInformations'
        });
    }

    private isGameInitDto(obj: any): obj is IGameInitDto {
        return (obj as IGameInitDto).dealer !== undefined;
    }

    private isICoincheContractDto(obj: any): obj is ICoincheContractDto {
        return (obj as ICoincheContractDto).hasLastPLayerPassed !== undefined;
    }

    private gameContractChanged(contractInfo: ICoincheContractDto) {
        this.setCurrentPlayerPlaying(contractInfo.currentPlayerNumber);

        const currentPlayer = this.players.find(p => p.id == this.playerId);
        if (currentPlayer.isPlaying) {
            var contractEvent = new ContractEvent();
            contractEvent.selectedValue = contractInfo.value + 10;
            contractEvent.selectedColor = contractInfo.color;

            this.onPlayerReadyToBet.next(contractEvent);
        }
        else {
            this.onPlayerReadyToBet.next(undefined);
        }
    }

    /**
     * Init game :
     *   - load cards sprites
     *   - set players numbers
     *   - set current dealer
     *   - display contract form
     * @param gameDatas
     */
    private initGame(gameDatas: IGameInitDto) {
        this.displayCards(gameDatas.playerCards);
        this.setPlayersNumber(gameDatas.playerNumber);
        this.setDealer(gameDatas.dealer);
        this.setCurrentPlayerPlaying(gameDatas.playerPlaying);

        const currentPlayer = this.players.find(p => p.id == this.playerId);
        if (currentPlayer.isPlaying) {
            var contractEvent = new ContractEvent();
            contractEvent.selectedValue = 80;

            this.onPlayerReadyToBet.next(contractEvent);
        }

        this.startTurnTimer(gameDatas.playerPlaying);
    }

    private startTurnTimer(currentPlayerNumber: number) {
        console.log('timer start');
        let currentPlayer = this.players.find(p => p.number == currentPlayerNumber);
        let startEvent = currentPlayer.getTurnTimerPosition();
        this.onTurnTimeStarted.next(startEvent);

        this.currentLoopIteration = 0;

        this.turnTimer = setInterval(
            () => {
                //if (this.currentLoopIteration === 0) {
                //    this.onTurnTimeStarted.next(startEvent);
                //}
                const completion = Math.round(this.currentLoopIteration * 100 / this.turnTimerTimespan);
                this.currentLoopIteration++;

                const event = new TurnTimerTickedEvent();
                event.percentage = completion;

                this.onTurnTimerTicked.next(event);

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
        this.players.forEach(p => p.isPlaying = false);
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

        this.onDealerDefined.next(dealerChipEvent);
    }

    /**
     * Set players number.
     * Current player take the number returned in GameInfo and the others are
     * set counter-clockwise (ex: bottom=1, right = 2, top = 3, left = 0).
     * @param playerNumber number of the current real player.
     */
    private setPlayersNumber(playerNumber: number) {
        this.players.find(p => p.position == ScreenCoordinate.bottom).number = playerNumber;
        this.players.find(p => p.position == ScreenCoordinate.right).number = (playerNumber + 1) % 4;
        this.players.find(p => p.position == ScreenCoordinate.top).number = (playerNumber + 2) % 4;
        this.players.find(p => p.position == ScreenCoordinate.left).number = (playerNumber + 3) % 4;
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

                    this.onCurrentPlayerCardReceived.next(cardEvent);
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

                    this.onOtherPlayerCardReceived.next(cardEvent);
                }
            }
        }
    }

    /**
     * Destroy all subscriptions.
     */
    public onDestroy() {
        this.subscriptions.unsubscribe();

        this.gameWorkerService.postMessage({
            fName: 'destroy'
        });

        this.onCurrentPlayerCardReceived.complete();
        this.onOtherPlayerCardReceived.complete();
        this.onPlayerReadyToBet.complete();
        this.onDealerDefined.complete();
        this.onTurnTimeStarted.complete();
        this.onTurnTimerTicked.complete();
    }

    /**
     * Send the ContractEvent to the server.
     * @param contract Player's contract.
     */
    public sendContract(contract: ContractEvent) {
        //if (contract) {
        //    this.gameService.broadcastSetGameContract(this.gameId, this.playerId, contract.selectedColor, contract.selectedValue);
        //}
        //else {
        //    this.gameService.broadcastPassGameContract(this.gameId, this.playerId);
        //}
    }
}
