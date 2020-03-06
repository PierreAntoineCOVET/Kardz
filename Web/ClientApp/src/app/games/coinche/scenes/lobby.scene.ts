import { Injectable } from '@angular/core';
import { Player } from '../domain/player';
import { Lobby } from '../domain/lobby';
import { SignalRService } from '../../../services/signal-r/signal-r.service';
import { TranslateService } from '@ngx-translate/core';

/**
 * Core Coinche game loading and orchestrator.
 */
@Injectable()
export class LobbyScene extends Phaser.Scene {
    private lobby: Lobby;
    private numberOfPlayersText: Phaser.GameObjects.Text;

    constructor(private signalRService: SignalRService, private translateService: TranslateService) {
        super({ key: 'lobby' });

        this.lobby = new Lobby();

    }

    preload() {
    }
    create() {
        this.signalRService.addNewPLayerListener((data) => this.addNewPlayer(data));

        this.addPlayer();
        this.numberOfPlayersText = this.add.text(200, 200, this.lobby.getNumberOfPlayers);
    }
    update() {
    }

    private addPlayer() {
        this.lobby.addPlayer()
            .subscribe({
                next: (playerId) => this.signalRService.broadcastNewPlayer(playerId)
            });
    }

    private addNewPlayer(numebrOfPlayers: number) {
        this.numberOfPlayersText.setText(this.translateService.instant('game.lobby.numberOfPlayers') + ' : ' + numebrOfPlayers);
    }
}
