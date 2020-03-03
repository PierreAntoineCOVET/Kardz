import { Injectable } from '@angular/core';
import { GameScene } from './game';
import { LobbyScene } from './lobby';

/**
 * Entry point for the game. Load all scenes acording to context.
 */
@Injectable()
export class MainScene extends Phaser.Scene {
    constructor(private coicheScene: GameScene, private lobbyScene: LobbyScene) {
        super({ key: 'Main' });
    }

    preload() {
    }
    create() {
        this.scene.add('game', this.coicheScene, true);
        this.scene.add('lobby', this.lobbyScene, false);
    }
    update() {
        //this.scene.start('game');
    }
}
