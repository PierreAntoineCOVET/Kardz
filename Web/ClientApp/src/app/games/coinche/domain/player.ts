import { GameScene } from '../scenes/game';
import { Observable } from 'rxjs';
import { CardsEnum } from '../../../typewriter/enums/CardsEnum.enum';

/**
 * Player responsible to display it's cards.
 */
export class Player {
    private cardDisplayWidth: number;
    private cardDisplayHeight: number;

    constructor(private scene: GameScene, spriteBaseWidth: number, spriteBaseHeight: number)
    {
        this.cardDisplayWidth = spriteBaseWidth / 3;
        this.cardDisplayHeight = spriteBaseHeight / 3;
    }

    displayCards(cards: Observable<CardsEnum[]>): void {
        cards.subscribe({
            next: (cards) => {
                cards.sort((a, b) => a - b).forEach((spritePosition, index) => {
                    const xOffset = this.cardDisplayWidth / 2;
                    const yOffset = this.cardDisplayHeight / 2;

                    const sprite = this.scene.add.sprite(0 + xOffset + (index * this.cardDisplayWidth), 0 + yOffset, 'cards', spritePosition);

                    sprite.displayWidth = this.cardDisplayWidth;
                    sprite.displayHeight = this.cardDisplayHeight;
                });
            }
        });
    }
}
