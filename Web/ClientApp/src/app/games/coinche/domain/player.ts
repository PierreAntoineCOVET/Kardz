import { Observable } from 'rxjs';
import { CardsEnum } from '../../../typewriter/enums/CardsEnum.enum';
import { PlayerPosition } from '../domain/PlayerPosition';
import { v4 as uuidv4 } from 'uuid';

/**
 * Player responsible to display it's cards.
 */
export class Player {
    public number: number;
    public isDealer: boolean;
    public isPlaying: boolean;

    constructor(public id: uuidv4, public position: PlayerPosition)
    {
    }

    /**
     * Set sceneElement position according to player position and card number.
     * @param index Card to display.
     * @param elementWidth Width of the elements (to aligne them).
     */
    public getSpritePosition(index: number, elementWidth: number): { x: number, y: number, angle: number } {
        const spritePosition = { x: 0, y: 0, angle: 0 };

        if (this.position == PlayerPosition.top) {
            spritePosition.x = 435 + (index * elementWidth);
            spritePosition.y = 86;
        }
        else if (this.position == PlayerPosition.bottom) {
            spritePosition.x = 435 + (index * elementWidth);
            spritePosition.y = 814;
        }
        else if (this.position == PlayerPosition.right) {
            spritePosition.x = 1514;
            spritePosition.y = 80 + (index * elementWidth);
            spritePosition.angle = 90;
        }
        else if (this.position == PlayerPosition.left) {
            spritePosition.x = 86;
            spritePosition.y = 80 + (index * elementWidth);
            spritePosition.angle = -90;
        }

        return spritePosition;
    }

    public getDealerChipPosition(): { x: number, y: number } {
        const spritePosition = { x: 0, y: 0 };

        if (this.position == PlayerPosition.top) {
            spritePosition.x = 430;
            spritePosition.y = 210;
        }
        else if (this.position == PlayerPosition.bottom) {
            spritePosition.x = 1150;
            spritePosition.y = 685;
        }
        else if (this.position == PlayerPosition.right) {
            spritePosition.x = 1390;
            spritePosition.y = 150;
        }
        else if (this.position == PlayerPosition.left) {
            spritePosition.x = 210;
            spritePosition.y = 730;
        }

        return spritePosition;
    }
}

