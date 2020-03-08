import { Injectable } from '@angular/core';
import { GameScene } from './game.scene';
import { LobbyScene } from './lobby.scene';
import { SignalRService } from '../../../services/signal-r/signal-r.service';
import { GameDto } from '../../../typewriter/classes/GameDto';

/**
 * Entry point for the game. Load all scenes acording to context.
 */
@Injectable()
export class MainScene extends Phaser.Scene {

    constructor(private coicheScene: GameScene, private lobbyScene: LobbyScene, private signalRService: SignalRService) {
        super({ key: 'Main' });

        this.signalRService.startConnection()
            .then(() => this.onSocketInitialized())
            .catch((reason) => this.onSocketInitializationFailed(reason));
    }

    preload() {
    }
    create() {
        this.scene.add('game', this.coicheScene, false);

        this.lobbyScene = <LobbyScene>this.scene.add('lobby', this.lobbyScene, false);
        this.lobbyScene.onGameStarted.subscribe({
            next: data => this.startGame(data)
        });
    }
    update() {
    }

    private startGame(data: GameDto) {
        this.lobbyScene.stopSubscriptions();
        this.scene.stop('lobby');
        this.scene.start('game');
    }

    private onSocketInitialized() {
        this.scene.start('lobby');
    }

    private onSocketInitializationFailed(error: any) {
        console.log("Socket initialisation failed :");
        console.log(error);
        //Todo display error on scene
    }
}
