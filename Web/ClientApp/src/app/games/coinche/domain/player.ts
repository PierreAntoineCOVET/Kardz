import { ScreenCoordinate } from '../domain/PlayerPosition';
import { StartTurnTimerEvent } from './events/turn-timer.event';
import { BubbleQueuePosition, PlayerSaidEvent } from './events/player-said.event';
import { IGameContractDto } from 'src/app/typewriter/classes/GameContractDto';
import { CoincheCardColorsEnum } from 'src/app/typewriter/enums/CardEnum.enum';
import { ContractSealedEvent } from 'src/app/games/coinche/domain/events/contract-sealed.event';

/**
 * Player VM responsible to compute it's UI (position and width).
 */
export class Player {
    public number!: number;
    public teamNumber!: number;
    public isDealer!: boolean;
    public isPlaying!: boolean;

    private readonly turnTimerBigSide = 300;
    private readonly turnTimerSmallSide = 30;

    private readonly contractSpeechBubbleWidth = 200;
    private readonly contractSpeechBubbleHeight = 100;

    constructor(public id: string | null, public position: ScreenCoordinate)
    {
    }

    /**
     * Set sceneElement position according to player position and card number.
     * @param index Card to display.
     * @param elementWidth Width of the elements (to aligne them).
     */
    public getSpritePosition(index: number, elementWidth: number): { x: number, y: number, angle: number } {
        const spritePosition = { x: 0, y: 0, angle: 0 };

        if (this.position == ScreenCoordinate.top) {
            spritePosition.x = 435 + (index * elementWidth);
            spritePosition.y = 86;
        }
        else if (this.position == ScreenCoordinate.bottom) {
            spritePosition.x = 435 + (index * elementWidth);
            spritePosition.y = 814;
        }
        else if (this.position == ScreenCoordinate.right) {
            spritePosition.x = 1514;
            spritePosition.y = 80 + (index * elementWidth);
            spritePosition.angle = 90;
        }
        else if (this.position == ScreenCoordinate.left) {
            spritePosition.x = 86;
            spritePosition.y = 80 + (index * elementWidth);
            spritePosition.angle = -90;
        }

        return spritePosition;
    }

    /**
     * Display the deal chip for the player who delt cards.
     */
    public getDealerChipPosition(): { x: number, y: number } {
        const spritePosition = { x: 0, y: 0 };

        if (this.position == ScreenCoordinate.top) {
            spritePosition.x = 430;
            spritePosition.y = 210;
        }
        else if (this.position == ScreenCoordinate.bottom) {
            spritePosition.x = 1150;
            spritePosition.y = 685;
        }
        else if (this.position == ScreenCoordinate.right) {
            spritePosition.x = 1390;
            spritePosition.y = 150;
        }
        else if (this.position == ScreenCoordinate.left) {
            spritePosition.x = 210;
            spritePosition.y = 730;
        }

        return spritePosition;
    }

    /**
     * Display the deal chip for the player who delt cards.
     */
    public getTurnTimerPosition(): StartTurnTimerEvent {
        const timerRectangle = {} as StartTurnTimerEvent;

        if (this.position == ScreenCoordinate.top) {
            timerRectangle.x = 930;
            timerRectangle.y = 210;
            timerRectangle.width = this.turnTimerBigSide;
            timerRectangle.height = this.turnTimerSmallSide;
            timerRectangle.direction = ScreenCoordinate.left;

        }
        else if (this.position == ScreenCoordinate.bottom) {
            timerRectangle.x = 650;
            timerRectangle.y = 685;
            timerRectangle.width = this.turnTimerBigSide;
            timerRectangle.height = this.turnTimerSmallSide;
            timerRectangle.direction = ScreenCoordinate.right;
        }
        else if (this.position == ScreenCoordinate.right) {
            timerRectangle.x = 1390;
            timerRectangle.y = 545;
            timerRectangle.width = this.turnTimerSmallSide;
            timerRectangle.height = this.turnTimerBigSide;
            timerRectangle.direction = ScreenCoordinate.top;
        }
        else if (this.position == ScreenCoordinate.left) {
            timerRectangle.x = 210;
            timerRectangle.y = 245;
            timerRectangle.width = this.turnTimerSmallSide;
            timerRectangle.height = this.turnTimerBigSide;
            timerRectangle.direction = ScreenCoordinate.bottom;
        }

        return timerRectangle;
    }

    /**
     * Get the position, size and content of the speech bubble
     * annoncing the last player contract choice.
     */
    public getActiveContractBubble(contractInfo: IGameContractDto): ContractSealedEvent {
        let text = this.getContractValueText(contractInfo);
        let position = this.getContractValuePosition();

        return {
            height: this.contractSpeechBubbleHeight,
            width: this.contractSpeechBubbleWidth,
            playerNumber: this.number,
            text: text,
            x: position.x,
            y: position.y
        } as ContractSealedEvent;
    }

    /**
     * Get the position, size and content of the speech bubble
     * annoncing the last player contract choice.
     */
    public getContractSpeechBubble(contractInfo: IGameContractDto): PlayerSaidEvent {
        let text = this.getContractLastBetText(contractInfo);
        let position = this.getBubblePosition();

        return {
            height: this.contractSpeechBubbleHeight,
            width: this.contractSpeechBubbleWidth,
            playerNumber: this.number,
            text: text,
            x: position.x,
            y: position.y,
            bubbleQueuePosition: position.queue
        } as PlayerSaidEvent;
    }

    private getContractValuePosition(): { x: number, y: number, queue: BubbleQueuePosition } {
        const position = {} as { x: number, y: number, queue: BubbleQueuePosition };

        if (this.position == ScreenCoordinate.top) {
            position.x = 1015;
            position.y = 190;
            position.queue = BubbleQueuePosition.topRight;
        }
        else if (this.position == ScreenCoordinate.bottom) {
            position.x = 380;
            position.y = 620;
            position.queue = BubbleQueuePosition.bottomLeft;
        }
        else if (this.position == ScreenCoordinate.right) {
            position.x = 1230;
            position.y = 620;
            position.queue = BubbleQueuePosition.bottomRight;
        }
        else if (this.position == ScreenCoordinate.left) {
            position.x = 170;
            position.y = 80;
            position.queue = BubbleQueuePosition.bottomLeft;
        }

        return position;
    }

    private getBubblePosition(): { x: number, y: number, queue: BubbleQueuePosition } {
        const position = {} as { x: number, y: number, queue: BubbleQueuePosition };

        if (this.position == ScreenCoordinate.top) {
            position.x = 715;
            position.y = 205;
            position.queue = BubbleQueuePosition.topRight;
        }
        else if (this.position == ScreenCoordinate.bottom) {
            position.x = 680;
            position.y = 610;
            position.queue = BubbleQueuePosition.bottomLeft;
        }
        else if (this.position == ScreenCoordinate.right) {
            position.x = 1230;
            position.y = 430;
            position.queue = BubbleQueuePosition.bottomRight;
        }
        else if (this.position == ScreenCoordinate.left) {
            position.x = 180;
            position.y = 380;
            position.queue = BubbleQueuePosition.bottomLeft;
        }

        return position;
    }

    private getContractValueText(contractInfo: IGameContractDto): string {
        if (contractInfo.value == null || contractInfo.color == null) {
            throw new Error("Invalid contract state !");
        }

        return `${contractInfo.value} ${CoincheCardColorsEnum[contractInfo.color]}`;
    }

    private getContractLastBetText(contractInfo: IGameContractDto): string {
        if (contractInfo.lastValue != null && contractInfo.lastColor != null) {
            return `${contractInfo.lastValue} ${CoincheCardColorsEnum[contractInfo.lastColor]}`;
        }
        else {
            if (contractInfo.isContractCoinched) {
                return 'Coinche !'
            }
            else if (contractInfo.isContractCounterCoinched) {
                return 'Contre Coinche !'
            }
            else {
                return 'Passed'
            }
        }
    }
}

