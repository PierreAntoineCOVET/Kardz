import { Injectable } from '@angular/core';
import { v4 as uuidv4 } from 'uuid';
import { Subscription } from 'rxjs';
import { ColorEnum } from 'src/app/typewriter/enums/ColorEnum.enum';
import { ContractEvent } from 'src/app/games/coinche/domain/events/contract.event';
import { ScreenCoordinate } from 'src/app/games/coinche/domain/PlayerPosition';
import { StartTurnTimerEvent, TurnTimerTickedEvent } from 'src/app/games/coinche/domain/events/turn-timer.event';
import { DealerSelectedEvent } from 'src/app/games/coinche/domain/events/dealer-selected.event';
import { AddCardEvent } from 'src/app/games/coinche/domain/events/add-card.event';
import { Game } from 'src/app/games/coinche/domain/game';

/**
 * Core Coinche game loading and orchestrator.
 */
@Injectable()
export class GameScene extends Phaser.Scene {
    private gameDomain: Game; // phaser already define a game variable.
    private subscriptions: Subscription;
    private cardsSpriteOption = {
        cardWidth: 334,
        cardHeight: 440
    };
    private cardsScaleFactor = 0.3;
    private dealerChipScaleFactor = 0.3;
    private contractFormElement: Phaser.GameObjects.DOMElement;
    private dealerChip: Phaser.GameObjects.Image;

    private turnTimerBox: Phaser.GameObjects.Graphics;
    private turnTimerFill: Phaser.GameObjects.Graphics;

    private turnTimerRectangle: { x: number, y: number, width: number, height: number, direction: ScreenCoordinate };

    private currentContract: ContractEvent;

    constructor() {
        super({ key: 'game' });
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

        this.subscriptions = this.gameDomain.onCurrentPlayerCardReceived.subscribe({
            next: (data) => this.addSprite(data)
        });

        this.subscriptions.add(this.gameDomain.onOtherPlayerCardReceived.subscribe({
            next: (data) => this.addImage(data)
        }));

        this.subscriptions.add(this.gameDomain.onPlayerReadyToBet.subscribe({
            next: (data) => {
                this.displayContractForm(data);
            }
        }));

        this.subscriptions.add(this.gameDomain.onDealerDefined.subscribe({
            next: (data) => this.displayDealerChip(data)
        }));

        this.subscriptions.add(this.gameDomain.onTurnTimeStarted.subscribe({
            next: (data) => {
                this.displayTurnTimer(data);
            }
        }));

        this.subscriptions.add(this.gameDomain.onTurnTimerTicked.subscribe({
            next: (data) => this.updateTurnTimer(data)
        }));
    }

    update() {
    }

    /**
     * Start loadings game's data (shuffled cards, ...). and listening to game events.
     * @param gameId Game's ID
     * @param playerId Current player's ID
     */
    public startListening(gameId: uuidv4, playerId: uuidv4) {
        this.gameDomain = new Game();

        this.gameDomain.setGame(gameId, playerId);
    }

    /**
     * Display the dealer chip at the event location.
     * @param event
     */
    private displayTurnTimer(event: StartTurnTimerEvent) {
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
     * Display the dealer chip at the event location.
     * @param event
     */
    private updateTurnTimer(event: TurnTimerTickedEvent) {
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
     * Display the dealer chip at the event location.
     * @param event
     */
    private displayDealerChip(event: DealerSelectedEvent) {
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
    private displayContractForm(event: ContractEvent) {
        if (event) {
            this.currentContract = new ContractEvent();

            this.contractFormElement = this.add.dom(800, 550).createFromCache('contractForm');

            const valueDropDown = this.contractFormElement.getChildByID("contractValue");
            for (let i = 80; i < event.selectedValue; i += 10) {
                const optionToRemove = valueDropDown.children.item(0);
                valueDropDown.removeChild(optionToRemove);
            }

            this.contractFormElement.addListener('click');
            this.contractFormElement.on(
                'click',
                (event) => this.contractFormClickEventHandler(event)
            );
            this.contractFormElement.setPerspective(800);
        }
        else {
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
                const valueDropDown = <any>this.contractFormElement.getChildByID('contractValue');
                const selectedValue: number =
                    valueDropDown.value === 'capot'
                        ? 170
                        : Number(valueDropDown.value);
                this.currentContract.selectedValue = selectedValue;
                this.gameDomain.sendContract(this.currentContract);
            }
        }
        else if (event.target.name === 'pass') {
            this.gameDomain.sendContract(undefined);
        }
        else if (event.target.name !== undefined && event.target.name !== 'value') {
            if (this.currentContract && this.currentContract.selectedColor !== undefined) {
                const previousColor = this.contractFormElement.getChildByID(ColorEnum[this.currentContract.selectedColor]);
                previousColor.classList.remove('selected');
            }

            const selectedColorButton = this.contractFormElement.getChildByID(event.target.value);
            selectedColorButton.classList.add('selected');

            let color: string = event.target.value;
            this.currentContract.selectedColor = ColorEnum[color];
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

    private createSpeechBubble(x, y, width, height, quote) {
        var bubbleWidth = width;
        var bubbleHeight = height;
        var bubblePadding = 10;
        var arrowHeight = bubbleHeight / 4;

        var bubble = this.add.graphics({ x: x, y: y });

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

        //  Calculate arrow coordinates
        var point1X = Math.floor(bubbleWidth / 7);
        var point1Y = bubbleHeight;
        var point2X = Math.floor((bubbleWidth / 7) * 2);
        var point2Y = bubbleHeight;
        var point3X = Math.floor(bubbleWidth / 7);
        var point3Y = Math.floor(bubbleHeight + arrowHeight);

        //  Bubble arrow shadow
        bubble.lineStyle(4, 0x222222, 0.5);
        bubble.lineBetween(point2X - 1, point2Y + 6, point3X + 2, point3Y);

        //  Bubble arrow fill
        bubble.fillTriangle(point1X, point1Y, point2X, point2Y, point3X, point3Y);
        bubble.lineStyle(2, 0x565656, 1);
        bubble.lineBetween(point2X, point2Y, point3X, point3Y);
        bubble.lineBetween(point1X, point1Y, point3X, point3Y);

        var content = this.add.text(0, 0, quote, { fontFamily: 'Arial', fontSize: 20, color: '#000000', align: 'center', wordWrap: { width: bubbleWidth - (bubblePadding * 2) } });

        var b = content.getBounds();

        content.setPosition(bubble.x + (bubbleWidth / 2) - (b.width / 2), bubble.y + (bubbleHeight / 2) - (b.height / 2));
    }
}
