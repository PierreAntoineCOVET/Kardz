import { GameScene } from '../scenes/game';
import { Observable } from 'rxjs';
import { CardsEnum } from '../../../typewriter/enums/CardsEnum.enum';
import { PlayerPosition } from './PlayerPosition';

/**
 * Player responsible to display it's cards.
 */
export class Player {
    private cardDisplayWidth: number;
    private cardDisplayHeight: number;
    private scaleFactor: number = 5;

    constructor(private scene: GameScene, spriteBaseWidth: number, spriteBaseHeight: number, private position: PlayerPosition, private active: boolean)
    {
        this.cardDisplayWidth = spriteBaseWidth / this.scaleFactor;
        this.cardDisplayHeight = spriteBaseHeight / this.scaleFactor;
    }

    public displayCards(cards: CardsEnum[]): void {
        cards.sort((a, b) => a - b).forEach((spriteNumber, index) => {
            const spritePosition = this.getSpritePosition(this.position, index);

            if (this.active) {
                const sprite = this.scene.add.sprite(
                    spritePosition.x,
                    spritePosition.y,
                    'cards',
                    spriteNumber);
                sprite.displayWidth = this.cardDisplayWidth;
                sprite.displayHeight = this.cardDisplayHeight;
            }
            else {
                const image = this.scene.add.image(spritePosition.x, spritePosition.y, 'cardBack');
                image.displayWidth = this.cardDisplayWidth;
                image.displayHeight = this.cardDisplayHeight;
            }

        });
    }

    private getSpritePosition(position: PlayerPosition, index: number): { x: number, y: number } {
        if (position == PlayerPosition.top) {
            return {
                x: this.cardDisplayHeight / 2 + 100 + (index * this.cardDisplayWidth),
                y: this.cardDisplayHeight / 2
            };
        }
        else if (position == PlayerPosition.bottom) {
            return {
                x: this.cardDisplayHeight / 2 + 100 + (index * this.cardDisplayWidth),
                y: this.cardDisplayHeight / 2 + 500
            };
        }
        else if (position == PlayerPosition.right) {
            return {
                x: this.cardDisplayHeight / 2 + 700,
                y: this.cardDisplayHeight / 2 + (index * this.cardDisplayWidth)
            };
        }
        else if (position == PlayerPosition.leftt) {
            return {
                x: this.cardDisplayHeight / 2,
                y: this.cardDisplayHeight / 2 + (index * this.cardDisplayWidth)
            };
        }
    }
}

