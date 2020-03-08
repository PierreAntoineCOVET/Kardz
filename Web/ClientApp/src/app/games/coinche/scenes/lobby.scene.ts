import { Injectable } from '@angular/core';
import { Player } from '../domain/player';
import { Lobby } from '../domain/lobby';
import { SignalRService } from '../../../services/signal-r/signal-r.service';
import { TranslateService } from '@ngx-translate/core';
import { Button } from '../../engine/button.component';

/**
 * Core Coinche game loading and orchestrator.
 */
@Injectable()
export class LobbyScene extends Phaser.Scene {
    private lobby: Lobby;
    private numberOfPlayersText: Phaser.GameObjects.Text;
    private startGameButton: Phaser.GameObjects.Text;
    private searchGameButton: Button;

    constructor(private signalRService: SignalRService, private translateService: TranslateService) {
        super({ key: 'lobby' });

        this.lobby = new Lobby();
    }

    preload() {
    }
    create() {
        this.signalRService.onNewPlayer.subscribe({
            next: (data) => this.addNewPlayer(data)
        });

        this.searchGameButton = new Button(this, 540, 500, this.translateService.instant('game.lobby.searchGame'));
        this.searchGameButton.click
            .subscribe({
                next: () => this.searchGame()
            });
        this.add.existing(this.searchGameButton);

        this.numberOfPlayersText = this.add.text(780, 20, '')
            .setOrigin(1, 0);

        this.addPlayer();

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

    private searchGame() {
        this.signalRService.broadcastSearchGame(this.lobby.playerId)
    }
}
