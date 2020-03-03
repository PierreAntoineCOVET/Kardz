import { Player } from '../domain/player';
import { CardsService } from '../../../services/cards/cards.service';
import { GamesEnum } from '../../../typewriter/enums/GamesEnum.enum';
import { Injectable } from '@angular/core';
import { PlayerPosition } from '../domain/PlayerPosition';

/**
 * Core Coinche game loading and orchestrator.
 */
@Injectable()
export class GameScene extends Phaser.Scene {
    private cards: Phaser.GameObjects.Sprite;
    private players: Player[] = []
    private cardsSpriteOption = {
        cardWidth: 334,
        cardHeight: 440
    };

    constructor(private cardService: CardsService) {
        super({ key: 'Game' });

        this.players.push(new Player(this, PlayerPosition.bottom, true));
        this.players.push(new Player(this, PlayerPosition.left, false));
        this.players.push(new Player(this, PlayerPosition.top, false));
        this.players.push(new Player(this, PlayerPosition.right, false));
    }

    preload() {
        this.load.spritesheet('cards', 'assets/img/cards.png', {
            frameWidth: this.cardsSpriteOption.cardWidth,
            frameHeight: this.cardsSpriteOption.cardHeight
        });
        this.load.image('cardBack', 'assets/img/back.png');
    }
    create() {
        this.cardService.getShuffledCards(GamesEnum.Coinche).subscribe({
            next: (cards) => {
                this.players.forEach((player, index) => {
                    const playerCards = cards.splice(0, 8);
                    player.displayCards(playerCards)
                });
            }
        });
    }
    update() {
    }
}
