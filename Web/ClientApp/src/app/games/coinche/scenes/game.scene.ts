import { GamesEnum } from '../../../typewriter/enums/GamesEnum.enum';
import { Injectable } from '@angular/core';
import { Game } from '../domain/game';
import { v4 as uuidv4 } from 'uuid';
import { GameService } from '../../../services/game/game.service';
import { AddCardEvent } from '../domain/events/add-card.event';
import { Subscription } from 'rxjs';
import { ChooseContractEvent } from '../domain/events/choose-contract.event';
import { DealerSelectedEvent } from '../domain/events/dealer-selected.event';

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
        this.events.on('shutdown', () => this.onDestroy());

        this.subscriptions = this.gameDomain.onNewSpriteSubscriber.subscribe({
            next: (data) => this.addSprite(data)
        });

        this.subscriptions.add(this.gameDomain.onNewImageSubscriber.subscribe({
            next: (data) => this.addImage(data)
        }));

        this.subscriptions.add(this.gameDomain.onPlayerChooseContractSubscriber.subscribe({
            next: (data) => this.displayContractForm(data)
        }));

        this.subscriptions.add(this.gameDomain.onDealerSetSubscriber.subscribe({
            next: (data) => this.displayDealerChip(data)
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
    private displayDealerChip(event: DealerSelectedEvent) {
        if (event) {
            if (!this.dealerChip) {
                this.dealerChip = this.add.image(event.x, event.y, 'dealerChip');
                this.dealerChip.setScale(this.dealerChipScaleFactor);
            }
            else {
                this.dealerChip.setPosition(event.x, event.y);
            }
        }
    }

    /**
     * Display the html form to choose the contract.
     * @param event Last player choice.
     */
    private displayContractForm(event: ChooseContractEvent) {
        if (event) {
            this.contractFormElement = this.add.dom(800, 400).createFromCache('contractForm');
            this.contractFormElement.setPerspective(800);
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
}
