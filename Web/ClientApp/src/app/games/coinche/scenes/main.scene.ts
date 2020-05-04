import { Injectable } from '@angular/core';
import { GameScene } from './game.scene';
import { LobbyScene } from './lobby.scene';
import { GameStartedEvent } from '../domain/events/game-started.event';

/**
 * Entry point for the game. Load all scenes acording to context.
 */
@Injectable()
export class MainScene extends Phaser.Scene {

    constructor(private gameScene: GameScene, private lobbyScene: LobbyScene) {
        super({ key: 'Main' });
    }

    preload() {
    }
    create() {
        this.scene.add('game', this.gameScene, false);

        this.scene.add('lobby', this.lobbyScene, true);
        this.lobbyScene.onGameStarted.subscribe({
            next: data => this.startGame(data)
        });
    }
    update() {
    }

    private startGame(data: GameStartedEvent) {
        this.scene.stop('lobby');
        this.scene.start('game', { gameId: data.gameId, playerId: data.playerId});
    }
}
