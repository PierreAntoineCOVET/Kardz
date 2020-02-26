import { Component, OnInit } from '@angular/core';
import Phaser from 'phaser';
import { CoicheScene } from './scenes/game';

@Component({
    selector: 'app-coinche',
    templateUrl: './coinche.component.html',
    styleUrls: ['./coinche.component.scss']
})
/**
 * Component holding the game canvas.
 */
export class CoincheComponent implements OnInit {

    game: Phaser.Game;
    gameConfig: Phaser.Types.Core.GameConfig;

    constructor() {
        this.gameConfig = {
            height: 600,
            width: 800,
            type: Phaser.AUTO,
            parent: 'gameContainer',
            scene: MainScene
        };
        this.game = new Phaser.Game(this.gameConfig);
    }

    ngOnInit(): void {
    }

}

/**
 * Entry point for the game. Load all scenes acording to context.
 */
class MainScene extends Phaser.Scene {
    private image: Phaser.GameObjects.Image;

    constructor() {
        super({ key: 'Main' });
    }

    preload() {
    }
    create() {
        this.scene.add('game', new CoicheScene(), true);
    }
    update() {
        this.scene.start('game');
    }
}
