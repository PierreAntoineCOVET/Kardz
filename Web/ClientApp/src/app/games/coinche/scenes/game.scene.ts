import { GamesEnum } from '../../../typewriter/enums/GamesEnum.enum';
import { Injectable } from '@angular/core';
import { Game } from '../domain/game';
import { v4 as uuidv4 } from 'uuid';
import { GameService } from '../../../services/game/game.service';
import { AddCardEvent } from '../domain/events/add-card.event';

/**
 * Core Coinche game loading and orchestrator.
 */
@Injectable()
export class GameScene extends Phaser.Scene {
    //private cards: Phaser.GameObjects.Sprite;
    private gameDomain: Game;
    private cardsSpriteOption = {
        cardWidth: 334,
        cardHeight: 440
    };
    private scaleFactor = 0.3;

    constructor(gameService: GameService) {
        super({ key: 'game' });

        this.gameDomain = new Game(gameService);

        //this.gameDomain.onNewSpriteAdded.subscribe({
        //    next: (data) => this.addSprite(data)
        //});

        //this.gameDomain.onNewImageAdded.subscribe({
        //    next: (data) => this.addImage(data)
        //});
    }

    preload() {
        this.load.spritesheet('cards', 'assets/img/cards.png', {
            frameWidth: this.cardsSpriteOption.cardWidth,
            frameHeight: this.cardsSpriteOption.cardHeight
        });
        this.load.image('cardBack', 'assets/img/back.png');
    }
    create() {
        this.gameDomain.onNewSpriteAdded.subscribe({
            next: (data) => this.addSprite(data)
        });

        this.gameDomain.onNewImageAdded.subscribe({
            next: (data) => this.addImage(data)
        });
    }
    update() {
    }

    public setGame(gameId: uuidv4, playerId: uuidv4) {
        this.gameDomain.setGame(gameId, playerId);
    }

    private addImage(event: AddCardEvent) {
        const image = this.add.image(
            event.x,
            event.y,
            event.elementName);
        image.setScale(this.scaleFactor);
        image.setAngle(event.angle);
    }

    private addSprite(event: AddCardEvent) {
        const sprite = this.add.sprite(
            event.x,
            event.y,
            event.elementName,
            event.card);
        sprite.setScale(this.scaleFactor);
    }
}
