import { Injectable } from '@angular/core';
import { GameScene } from './game';
import { LobbyScene } from './lobby';
import { SignalRService } from '../../../services/signal-r/signal-r.service';
import { v4 as uuidv4 } from 'uuid';

/**
 * Entry point for the game. Load all scenes acording to context.
 */
@Injectable()
export class MainScene extends Phaser.Scene {
    constructor(private coicheScene: GameScene, private lobbyScene: LobbyScene, private signalRService: SignalRService) {
        super({ key: 'Main' });

        this.signalRService.startConnection({
            next: () => this.onSocketInitialized(),
            error: (reason) => this.onSocketInitializationFailed(reason),
            complete: () => { }
        });
        this.signalRService.addLobbyListener((data) => console.log(data));
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

    private onSocketInitialized() {
        this.signalRService.broadcastToLobby(uuidv4());
    }

    private onSocketInitializationFailed(error: any) {
        console.log("Socket initialisation failed :");
        console.log(error);
    }
}
