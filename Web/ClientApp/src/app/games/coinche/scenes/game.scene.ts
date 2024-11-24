import { Injectable } from '@angular/core';
import { Subscription } from 'rxjs';
import { ContractMadeEvent } from 'src/app/games/coinche/domain/events/contract.event';
import { ScreenCoordinate } from 'src/app/games/coinche/domain/PlayerPosition';
import { StartTurnTimerEvent, TurnTimerTickedEvent } from 'src/app/games/coinche/domain/events/turn-timer.event';
import { DealerSelectedEvent } from 'src/app/games/coinche/domain/events/dealer-selected.event';
import { AddCardEvent } from 'src/app/games/coinche/domain/events/add-card.event';
import { Game } from 'src/app/games/coinche/domain/game';
import { BubbleQueuePosition, PlayerSaidEvent } from '../domain/events/player-said.event';
import { FOREVER } from 'phaser';
import { CoincheCardColorsEnum } from 'src/app/typewriter/enums/CardEnum.enum';
import { ContractSealedEvent } from 'src/app/games/coinche/domain/events/contract-sealed.event';

/**
 * Core Coinche game loading and orchestrator.
 */
@Injectable()
export class GameScene extends Phaser.Scene {
    private gameDomain!: Game; // phaser already define a game variable.
    private subscriptions: Subscription;
    private cardsSpriteOption = {
        cardWidth: 334,
        cardHeight: 440
    };
    private cardsScaleFactor = 0.3;
    private dealerChipScaleFactor = 0.3;
    private contractFormElement!: Phaser.GameObjects.DOMElement;
    private dealerChip!: Phaser.GameObjects.Image;

    private turnTimerBox!: Phaser.GameObjects.Graphics;
    private turnTimerFill!: Phaser.GameObjects.Graphics;

    private turnTimerRectangle!: { x: number, y: number, width: number, height: number, direction: ScreenCoordinate };

    private currentContract!: ContractMadeEvent;

    private speechBubbles: { playerNumber: number, bubble: Phaser.GameObjects.Graphics, text: Phaser.GameObjects.Text }[];

    private contractBubbles: { playerNumber: number, bubble: Phaser.GameObjects.Graphics, text: Phaser.GameObjects.Text }[];

    constructor() {
        super({ key: 'game' });
        this.subscriptions = new Subscription();
        this.speechBubbles = [];
        this.contractBubbles = [];
    }

    init() {
    }

    preload() {
        this.load.spritesheet('cards', 'assets/img/cards.png', {
            frameWidth: this.cardsSpriteOption.cardWidth,
            frameHeight: this.cardsSpriteOption.cardHeight
        });

        this.load.image('cardBack', 'assets/img/back.png');

        this.load.image('dealerChip', 'assets/img/dealer.jpg');

        this.load.html('contractForm', 'assets/game-forms/coinche/contract.form.html');
    }

    create() {
        this.dealerChip = this.add.image(0, 0, 'dealerChip');
        this.dealerChip.setScale(this.dealerChipScaleFactor);
        this.dealerChip.visible = false;

        this.turnTimerBox = this.add.graphics();
        this.turnTimerFill = this.add.graphics();

        this.events.on('shutdown', () => this.onDestroy());

        this.subscriptions.add(this.gameDomain.onCurrentPlayerCardReceived.subscribe({
            next: (data) => this.addSprite(data)
        }));

        this.subscriptions.add(this.gameDomain.onOtherPlayerCardReceived.subscribe({
            next: (data) => this.addImage(data)
        }));

        this.subscriptions.add(this.gameDomain.onContractChanged.subscribe({
            next: (data) => {
                this.displayContractForm(data);
            }
        }));

        this.subscriptions.add(this.gameDomain.onDealerDefined.subscribe({
            next: (data) => this.displayDealerChip(data)
        }));

        this.subscriptions.add(this.gameDomain.onTurnTimeStarted.subscribe({
            next: (data) => {
                if (data) {
                    this.displayTurnTimer(data);
                }
            }
        }));

        this.subscriptions.add(this.gameDomain.onTurnTimerTicked.subscribe({
            next: (data) => this.updateTurnTimer(data)
        }));

        this.subscriptions.add(this.gameDomain.onTurnTimerCleared.subscribe({
            next: () => this.clearTurnTimer()
        }));

        this.subscriptions.add(this.gameDomain.onPlayerSays.subscribe({
            next: (event) => this.playerSays(event)
        }));

        this.subscriptions.add(this.gameDomain.onContractSealed.subscribe({
            next: (event) => this.showActiveContract(event)
        }));
    }

    override update() {
    }

    /**
     * Start loadings game's data (shuffled cards, ...). and listening to game events.
     * @param gameId Game's ID
     * @param playerId Current player's ID
     */
    public startListening(gameId: string, playerId: string) {
        this.gameDomain = new Game();

        this.gameDomain.setGame(gameId, playerId);
    }

    /**
     * Display a buble as a player speech.
     * @param playerSaidEvent What to say and where to display it.
     */
    private playerSays(playerSaidEvent: PlayerSaidEvent | undefined) {
        if (playerSaidEvent) {
            const bubbleData = this.createSpeechBubble(playerSaidEvent);
            this.speechBubbles.push(bubbleData);
        }
        else {
            this.clearAllSpeachBubble();
        }
    }

    /**
     * Display a buble as a player speech.
     * @param contractSealedEvent What to say and where to display it.
     */
    private showActiveContract(contractSealedEvent: ContractSealedEvent | undefined) {
        if (contractSealedEvent) {
            const playerSaidEvent = {
                x: contractSealedEvent.x,
                y: contractSealedEvent.y,
                width: contractSealedEvent.width,
                height: contractSealedEvent.height,
                text: contractSealedEvent.text,
                playerNumber: contractSealedEvent.playerNumber,
                bubbleQueuePosition: null
            } as PlayerSaidEvent;
            const bubbleData = this.createSpeechBubble(playerSaidEvent);
            this.contractBubbles.push(bubbleData)
        }
        else {
            this.clearAllContractBubble();
        }
    }

    /**
     * Display a green rectangle representing the turn timer;
     * @param event
     */
    private displayTurnTimer(event: StartTurnTimerEvent) {
        this.cleanSpeachBubble(event.playerNumber);
        this.turnTimerRectangle = {
            x: event.x,
            y: event.y,
            width: event.width,
            height: event.height,
            direction: event.direction
        };

        if (event.direction == ScreenCoordinate.left) {
            this.turnTimerRectangle.width = -this.turnTimerRectangle.width;
        }

        if (event.direction == ScreenCoordinate.top) {
            this.turnTimerRectangle.height = -this.turnTimerRectangle.height;
        }

        this.turnTimerBox.fillStyle(0x00FF00);
        this.turnTimerBox.fillRect(
            this.turnTimerRectangle.x,
            this.turnTimerRectangle.y,
            this.turnTimerRectangle.width,
            this.turnTimerRectangle.height);
    }

    /**
     * Fill the turn timer in red for the given percentage.
     * @param event
     */
    private updateTurnTimer(event: TurnTimerTickedEvent) {
        if (!this.turnTimerRectangle) {
            return;
        }

        this.turnTimerFill.clear();
        this.turnTimerFill.fillStyle(0xFF0000);

        if (this.turnTimerRectangle.direction == ScreenCoordinate.right
            || this.turnTimerRectangle.direction == ScreenCoordinate.left) {
            this.turnTimerFill.fillRect(
                this.turnTimerRectangle.x,
                this.turnTimerRectangle.y,
                this.turnTimerRectangle.width * event.percentage / 100,
                this.turnTimerRectangle.height);
        }

        if (this.turnTimerRectangle.direction == ScreenCoordinate.top
            || this.turnTimerRectangle.direction == ScreenCoordinate.bottom) {
            this.turnTimerFill.fillRect(
                this.turnTimerRectangle.x,
                this.turnTimerRectangle.y,
                this.turnTimerRectangle.width,
                this.turnTimerRectangle.height * event.percentage / 100);
        }
    }

    /**
     * Clear the current timer rectangles.
     */
    private clearTurnTimer() {
        this.turnTimerBox.clear();
        this.turnTimerFill.clear();
    }

    /**
     * Display the dealer chip at the event location.
     * @param event
     */
    private displayDealerChip(event: DealerSelectedEvent | undefined) {
        if (event) {
            if (!this.dealerChip.visible) {
                this.dealerChip.visible = true;
            }

            this.dealerChip.setPosition(event.x, event.y);
        }
        else {
            this.dealerChip.visible = false;
        }
    }

    /**
     * Display the html form to choose the contract.
     * @param event Last player choice.
     */
    private displayContractForm(event: ContractMadeEvent | undefined) {
        if (event) {
            this.cleanSpeachBubble(event.currentPlayerNumber);

            this.currentContract = {} as ContractMadeEvent;

            this.contractFormElement = this.add.dom(800, 550).createFromCache('contractForm');

            if (event.selectedValue) {
                const valueDropDown = this.contractFormElement.getChildByID("contractValue");

                if (!valueDropDown) {
                    throw new Error("valueDropDown is null");
                }

                for (let i = 80; i < event.selectedValue; i += 10) {
                    const optionToRemove = valueDropDown.children.item(0);
                    if (optionToRemove) {
                        valueDropDown.removeChild(optionToRemove);
                    }
                }
            }
            else {
                const coincheButton = this.contractFormElement.getChildByID("coinche");
                if (!coincheButton) {
                    throw new Error("Child ID coinche is null");
                }
                coincheButton.setAttribute("disabled", "disabled");
            }

            this.contractFormElement.addListener('click');
            this.contractFormElement.on(
                'click',
                (event: any) => this.contractFormClickEventHandler(event)
            );
            this.contractFormElement.setPerspective(800);
        }
        else {
            this.clearTurnTimer();
            if (this.contractFormElement) {
                this.contractFormElement.destroy();
            }
        }
    }

    /**
     * Handle all contract's related actions.
     * @param event click event data.
     */
    private contractFormClickEventHandler(event: any) {
        if (event.target.name === 'bet') {
            if (this.currentContract.selectedColor !== undefined) {
                const valueDropDown = this.contractFormElement.getChildByID('contractValue') as  any;
                if (!valueDropDown) {
                    throw new Error("Child ID contractValue is null");
                }
                const selectedValue: number =
                    valueDropDown.value === 'capot'
                        ? 170
                        : Number(valueDropDown.value);
                this.currentContract.selectedValue = selectedValue;
                this.gameDomain.sendContract(this.currentContract);
            }
            else {
                const betColorWarning = this.contractFormElement.getChildByID('betColorWarning');
                if (!betColorWarning) {
                    throw new Error("Child ID betColorWarning is null");
                }
                betColorWarning.classList.remove('hidden');
            }
        }
        else if (event.target.name === 'pass') {
            this.gameDomain.sendContract(undefined);
        }
        else if (event.target.name === 'coinche') {
            this.currentContract.coinched = true;
            this.gameDomain.sendContract(this.currentContract);
        }
        else if (event.target.name !== undefined && event.target.name !== 'value') {
            if (this.currentContract && this.currentContract.selectedColor !== undefined) {
                const previousColor = this.contractFormElement.getChildByID(CoincheCardColorsEnum[this.currentContract.selectedColor]);
                if (!previousColor) {
                    throw new Error("Child ID for previous color is null");
                }
                previousColor.classList.remove('selected');
            }

            const selectedColorButton = this.contractFormElement.getChildByID(event.target.value);
            if (!selectedColorButton) {
                throw new Error("Child ID for current color is null");
            }
            selectedColorButton.classList.add('selected');

            let color: string = event.target.value;
            this.currentContract.selectedColor = CoincheCardColorsEnum[color as keyof typeof CoincheCardColorsEnum];
        }
    }

    /**
     * Add an image to the given location.
     * @param event Image informations.
     */
    private addImage(event: AddCardEvent) {
        const image = this.add.image(
            event.x,
            event.y,
            event.elementName);
        image.setScale(this.cardsScaleFactor);
        image.setAngle(event.angle);
    }

    /**
     * Add a sprite to the given location.
     * @param event Sprite information.
     */
    private addSprite(event: AddCardEvent) {
        const sprite = this.add.sprite(
            event.x,
            event.y,
            event.elementName,
            event.card);
        sprite.setScale(this.cardsScaleFactor);
    }

    /**
     * Remove all subscriptions.
     */
    private onDestroy() {
        this.gameDomain.onDestroy();
        this.subscriptions.unsubscribe();
    }

    private createSpeechBubble(playerSaidEvent: PlayerSaidEvent): { playerNumber: number, bubble: Phaser.GameObjects.Graphics, text: Phaser.GameObjects.Text } {
        var bubbleWidth = playerSaidEvent.width;
        var bubbleHeight = playerSaidEvent.height;
        var bubblePadding = 10;
        var arrowHeight = bubbleHeight / 4;

        var bubble = this.add.graphics({ x: playerSaidEvent.x, y: playerSaidEvent.y });
        var content = this.add.text(0, 0, playerSaidEvent.text, { fontFamily: 'Arial', fontSize: '2em', color: '#000000', align: 'center', wordWrap: { width: bubbleWidth - (bubblePadding * 2) } });

        this.cleanSpeachBubble(playerSaidEvent.playerNumber);

        //  Bubble shadow
        bubble.fillStyle(0x222222, 0.5);
        bubble.fillRoundedRect(6, 6, bubbleWidth, bubbleHeight, 16);

        //  Bubble color
        bubble.fillStyle(0xffffff, 1);

        //  Bubble outline line style
        bubble.lineStyle(4, 0x565656, 1);

        //  Bubble shape and outline
        bubble.strokeRoundedRect(0, 0, bubbleWidth, bubbleHeight, 16);
        bubble.fillRoundedRect(0, 0, bubbleWidth, bubbleHeight, 16);

        if (playerSaidEvent.bubbleQueuePosition != null) {
            //  Calculate arrow coordinates
            const queuePosition = this.getQueuePosition(bubbleWidth, bubbleHeight, arrowHeight, playerSaidEvent.bubbleQueuePosition);

            //  Bubble arrow shadow
            bubble.lineStyle(4, 0x222222, 0.5);
            if (playerSaidEvent.bubbleQueuePosition == BubbleQueuePosition.topRight) {
                bubble.lineBetween(queuePosition.point2X - 1, queuePosition.point2Y - 6, queuePosition.point3X + 2, queuePosition.point3Y);
            }
            else {
                bubble.lineBetween(queuePosition.point2X - 1, queuePosition.point2Y + 6, queuePosition.point3X + 2, queuePosition.point3Y);
            }

            //  Bubble arrow fill
            bubble.fillTriangle(
                queuePosition.point1X, queuePosition.point1Y,
                queuePosition.point2X, queuePosition.point2Y,
                queuePosition.point3X, queuePosition.point3Y);
            bubble.lineStyle(2, 0x565656, 1);
            bubble.lineBetween(queuePosition.point2X, queuePosition.point2Y, queuePosition.point3X, queuePosition.point3Y);
            bubble.lineBetween(queuePosition.point1X, queuePosition.point1Y, queuePosition.point3X, queuePosition.point3Y);
        }

        var b = content.getBounds();

        content.setPosition(bubble.x + (bubbleWidth / 2) - (b.width / 2), bubble.y + (bubbleHeight / 2) - (b.height / 2));

        return { playerNumber: playerSaidEvent.playerNumber, bubble: bubble, text: content };
    }

    /**
     * Clear all displayed player's speach bubble.
     */
    private clearAllSpeachBubble() {
        for (const player of this.gameDomain.players) {
            this.cleanSpeachBubble(player.number);
        }
    }

    /**
     * Delete every graphic elements related to a player speach bubble.
     * @param playerNumber
     */
    private cleanSpeachBubble(playerNumber: number) {
        const existingBubbleIndex = this.speechBubbles.findIndex(b => b.playerNumber == playerNumber)

        if (existingBubbleIndex > -1) {
            const existingBubble = this.speechBubbles[existingBubbleIndex];
            existingBubble.bubble.destroy();
            existingBubble.text.destroy();
            this.speechBubbles.splice(this.speechBubbles.indexOf(existingBubble), 1);
        }
    }

    /**
     * Clear all displayed player's speach bubble.
     */
    private clearAllContractBubble() {
        for (const player of this.gameDomain.players) {
            this.cleanContractBubble(player.number);
        }
    }

    /**
     * Delete every graphic elements related to a player speach bubble.
     * @param playerNumber
     */
    private cleanContractBubble(playerNumber: number) {
        const existingBubbleIndex = this.contractBubbles.findIndex(b => b.playerNumber == playerNumber)

        if (existingBubbleIndex > -1) {
            const existingBubble = this.contractBubbles[existingBubbleIndex];
            existingBubble.bubble.destroy();
            existingBubble.text.destroy();
            this.contractBubbles.splice(this.contractBubbles.indexOf(existingBubble), 1);
        }
    }

    /**
     * Compute the bubble queue position.
     * @param bubbleWidth
     * @param bubbleHeight
     * @param arrowHeight
     * @param bubbleQueuePosition
     */
    private getQueuePosition(bubbleWidth: number, bubbleHeight: number, arrowHeight: number, bubbleQueuePosition: BubbleQueuePosition):
        {
            point1X: number, point1Y: number,
            point2X: number, point2Y: number,
            point3X: number, point3Y: number,
        } {

        if (bubbleQueuePosition == BubbleQueuePosition.bottomLeft) {
            return {
                point1X: Math.floor(bubbleWidth / 7),
                point1Y: bubbleHeight,
                point2X: Math.floor((bubbleWidth / 7) * 2),
                point2Y: bubbleHeight,
                point3X: Math.floor(bubbleWidth / 7),
                point3Y: bubbleHeight + arrowHeight,
            };
        }
        else if (bubbleQueuePosition == BubbleQueuePosition.bottomRight) {
            return {
                point1X: Math.floor((bubbleWidth / 7) * 5),
                point1Y: bubbleHeight,
                point2X: Math.floor((bubbleWidth / 7) * 6),
                point2Y: bubbleHeight,
                point3X: Math.floor((bubbleWidth / 7) * 6),
                point3Y: bubbleHeight + arrowHeight,
            };
        }
        else {
            return {
                point1X: Math.floor((bubbleWidth / 7) * 5),
                point1Y: 0,
                point2X: Math.floor((bubbleWidth / 7) * 6),
                point2Y: 0,
                point3X: Math.floor((bubbleWidth / 7) * 6),
                point3Y: -arrowHeight,
            };
        }
    }

    /**
     * Display sealed contract info
     */
    private afterContractSealed() {

    }
}
