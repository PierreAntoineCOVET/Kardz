import { Player } from '../domain/player';

/**
 * Core Coinche game loading and orchestrator.
 */
export class CoicheScene extends Phaser.Scene {
    private cards: Phaser.GameObjects.Sprite;
    private player: Player
    private cardsSpriteOption = {
        cardWidth: 334,
        cardHeight: 440
    };

    constructor() {
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

    shuffleForPlayer(): number[] {
        let possibleSprites = [
            0,
            6,
            7,
            8,
            9,
            10,
            11,
            12,

            13 + 0,
            13 + 6,
            13 + 7,
            13 + 8,
            13 + 9,
            13 + 10,
            13 + 11,
            13 + 12,

            (13 * 2) + 0,
            (13 * 2) + 6,
            (13 * 2) + 7,
            (13 * 2) + 8,
            (13 * 2) + 9,
            (13 * 2) + 10,
            (13 * 2) + 11,
            (13 * 2) + 12,

            (13 * 3) + 0,
            (13 * 3) + 6,
            (13 * 3) + 7,
            (13 * 3) + 8,
            (13 * 3) + 9,
            (13 * 3) + 10,
            (13 * 3) + 11,
            (13 * 3) + 12,
        ];

        let selectedCards: number[] = [];

        for (let i = 0; i < 7; i++) {
            const cardNumber = Math.floor(Math.random() * possibleSprites.length);
            selectedCards.push(possibleSprites.splice(cardNumber, 1)[0]);
        }

        return selectedCards;
    }
}
