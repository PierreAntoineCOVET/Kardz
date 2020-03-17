import { GamesEnum } from '../../../typewriter/enums/GamesEnum.enum';
import { Injectable } from '@angular/core';
import { Game } from '../domain/game';
import { v4 as uuidv4 } from 'uuid';
import { GameService } from '../../../services/game/game.service';
import { AddCardEvent } from '../domain/events/add-card.event';
import { Subscription } from 'rxjs';

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
    private scaleFactor = 0.3;

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
    }
    create() {
        this.events.on('shutdown', () => this.onDestroy());

        this.subscriptions = this.gameDomain.onNewSpriteSubscriber.subscribe({
            next: (data) => this.addSprite(data)
        });

        this.subscriptions.add(this.gameDomain.onNewImageSubscriber.subscribe({
            next: (data) => this.addImage(data)
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
     * Add an image to the given location.
     * @param event Image informations.
     */
    private addImage(event: AddCardEvent) {
        const image = this.add.image(
            event.x,
            event.y,
            event.elementName);
        image.setScale(this.scaleFactor);
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
        sprite.setScale(this.scaleFactor);
    }

    /**
     * Remove all subscriptions.
     */
    private onDestroy() {
        this.gameDomain.onDestroy();
        this.subscriptions.unsubscribe();
    }
}
