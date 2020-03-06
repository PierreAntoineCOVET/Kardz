import { Injectable } from '@angular/core';
import { Player } from '../domain/player';
import { Lobby } from '../domain/lobby';
import { SignalRService } from '../../../services/signal-r/signal-r.service';

/**
 * Core Coinche game loading and orchestrator.
 */
@Injectable()
export class LobbyScene extends Phaser.Scene {
    private lobby: Lobby;
    private numberOfPlayersText: Phaser.GameObjects.Text;

    constructor(private signalRService: SignalRService) {
        super({ key: 'lobby' });

        this.lobby = new Lobby();

    }

    preload() {
    }
    create() {
        this.addCurrentPlayer();
        this.signalRService.addNewPLayerListener((data) => this.addNewPlayer(data));

        this.numberOfPlayersText = this.add.text(200, 200, this.lobby.getNumberOfPlayers);
    }
    update() {
    }

    private addCurrentPlayer() {
        this.lobby.addCurrentPlayer()
            .subscribe({
                next: (playerId) => this.signalRService.broadcastNewPlayer(playerId)
            });
    }

    private addNewPlayer(playerId: any) {
        this.lobby.addNewPlayer(playerId);
        this.numberOfPlayersText.setText(this.lobby.getNumberOfPlayers);
    }
}
