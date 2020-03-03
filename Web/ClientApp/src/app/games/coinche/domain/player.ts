import { GameScene } from '../scenes/game';
import { Observable } from 'rxjs';
import { CardsEnum } from '../../../typewriter/enums/CardsEnum.enum';
import { PlayerPosition } from './PlayerPosition';

/**
 * Player responsible to display it's cards.
 */
export class Player {
    private scaleFactor: number = 0.2;

    constructor(private scene: GameScene, private position: PlayerPosition, private active: boolean)
    { }

    /**
     * Display the players cards, front or back with correct screen position.
     * @param cards List of cards for the current player.
     */
    public displayCards(cards: CardsEnum[]): void {
        cards.sort((a, b) => a - b).forEach((spriteNumber, index) => {
            // Phaser.GameObjects.Sprite or Phaser.GameObjects.Image implements mostly the same interfaces
            // they have no 1 type with all needed methods => any.
            let sceneElement: any;

            if (this.active) {
                sceneElement = this.scene.add.sprite(
                    0,
                    0,
                    'cards',
                    spriteNumber);
            }
            else {
                sceneElement = this.scene.add.image(0, 0, 'cardBack');

                if (this.position == PlayerPosition.right) {
                    sceneElement.angle = 90;
                }
                else if (this.position == PlayerPosition.left) {
                    sceneElement.angle = -90;
                }
            }

            sceneElement.setScale(this.scaleFactor);
            sceneElement.setOrigin(0);
            this.setSpritePosition(this.position, index, sceneElement);

        });
    }

    /**
     * Set sceneElement position according to player position and card number.
     * @param position PLayer position on the screen.
     * @param index Card to display.
     * @param sceneElement Element to position.
     */
    private setSpritePosition(position: PlayerPosition, index: number, sceneElement: any): void {
        let spritePosition: { x: number, y: number };

        if (position == PlayerPosition.top) {
            spritePosition = {
                x: 100 + (index * sceneElement.displayWidth),
                y: 0
            };
        }
        else if (position == PlayerPosition.bottom) {
            spritePosition = {
                x: 100 + (index * sceneElement.displayWidth),
                y: 500
            };
        }
        else if (position == PlayerPosition.right) {
            spritePosition = {
                x: 780,
                y: 50 + (index * sceneElement.displayWidth)
            };
        }
        else if (position == PlayerPosition.left) {
            spritePosition = {
                x: 0,
                y: 110 + (index * sceneElement.displayWidth)
            };
        }

        sceneElement.setPosition(spritePosition.x, spritePosition.y);
    }
}

