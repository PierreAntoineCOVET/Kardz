import { Player } from '../domain/player';
import { CardsService } from '../../../services/cards/cards.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { CardsEnum } from '../../../typewriter/enums/CardsEnum.enum';
import { GamesEnum } from '../../../typewriter/enums/GamesEnum.enum';
import { Injectable } from '@angular/core';

/**
 * Core Coinche game loading and orchestrator.
 */
@Injectable({
    providedIn: 'root'
})
export class GameScene extends Phaser.Scene {
    private cards: Phaser.GameObjects.Sprite;
    private player: Player
    private cardsSpriteOption = {
        cardWidth: 334,
        cardHeight: 440
    };

    constructor(private cardService: CardsService) {
        super({ key: 'Game' });
        this.player = new Player(this, this.cardsSpriteOption.cardWidth, this.cardsSpriteOption.cardHeight);
    }

    preload() {
        this.load.spritesheet('cards', 'assets/img/cards.png', {
            frameWidth: this.cardsSpriteOption.cardWidth,
            frameHeight: this.cardsSpriteOption.cardHeight
        });
    }
    create() {
        this.player.displayCards(this.shuffleForPlayer());
    }
    update() {
    }

    shuffleForPlayer(): Observable<CardsEnum[]> {
        return this.cardService.getShuffledCards(GamesEnum.Coinche)
            .pipe(
                map(cards => cards.splice(0, 7))
            );
    }
}
