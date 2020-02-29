import { Injectable } from '@angular/core';
import { GameScene } from './game';

/**
 * Entry point for the game. Load all scenes acording to context.
 */
@Injectable({
    providedIn: 'root'
})
export class MainScene extends Phaser.Scene {
    constructor(private coicheScene: GameScene) {
        super({ key: 'Main' });
    }

    preload() {
    }
    create() {
        this.scene.add('game', this.coicheScene, true);
    }
    update() {
        this.scene.start('game');
    }
}
