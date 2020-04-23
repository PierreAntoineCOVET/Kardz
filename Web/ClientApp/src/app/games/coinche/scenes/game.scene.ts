import { GamesEnum } from '../../../typewriter/enums/GamesEnum.enum';
import { Injectable } from '@angular/core';
import { Game } from '../domain/game';
import { v4 as uuidv4 } from 'uuid';
import { GameService } from '../../../services/game/game.service';
import { AddCardEvent } from '../domain/events/add-card.event';
import { Subscription } from 'rxjs';
import { ChooseContractEvent } from '../domain/events/choose-contract.event';
import { DealerSelectedEvent } from '../domain/events/dealer-selected.event';
import { StartTurnTimerEvent, TurnTimerTickedEvent } from '../domain/events/turn-timer.event';
import { ScreenCoordinate } from '../domain/PlayerPosition';

/**
 * Core Coinche game loading and orchestrator.
 */
@Injectable()
export class GameScene extends Phaser.Scene {
    private gameDomain: Game;
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

    constructor(gameService: GameService) {
        super({ key: 'game' });

        this.gameDomain = new Game(gameService);
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

        this.subscriptions = this.gameDomain.onNewSpriteSubscriber.subscribe({
            next: (data) => this.addSprite(data)
        });

        this.subscriptions.add(this.gameDomain.onNewImageSubscriber.subscribe({
            next: (data) => this.addImage(data)
        }));

        this.subscriptions.add(this.gameDomain.onPlayerChooseContractSubscriber.subscribe({
            next: (data) => {
                this.displayContractForm(data);
                this.startTurnTimer();
            }
        }));

        this.subscriptions.add(this.gameDomain.onDealerSetSubscriber.subscribe({
            next: (data) => this.displayDealerChip(data)
        }));

        this.subscriptions.add(this.gameDomain.onTurnTimeStartedSubscriber.subscribe({
            next: (data) => this.displayTurnTimer(data)
        }));

        this.subscriptions.add(this.gameDomain.onTurnTimerTickedSubscriber.subscribe({
            next: (data) => this.updateTurnTimer(data)
        }));
    }

    update() {
    }

    /**
     * Start loadings game's data (shuffle cards, ...).
     * @param gameId Game's ID
     * @param playerId Current player's ID
     */
    public setGame(gameId: uuidv4, playerId: uuidv4) {
        this.gameDomain.setGame(gameId, playerId);
    }

    /**
     * Display the dealer chip at the event location.
     * @param event
     */
    private displayTurnTimer(event: StartTurnTimerEvent) {
        if (event) {
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
    }

    /**
     * Display the dealer chip at the event location.
     * @param event
     */
    private updateTurnTimer(event: TurnTimerTickedEvent) {
        if (event) {
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
    private displayContractForm(event: ChooseContractEvent) {
        if (event) {
            this.contractFormElement = this.add.dom(800, 550).createFromCache('contractForm');
            this.contractFormElement.setPerspective(800);
        }
    }

    private startTurnTimer() {
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
}
