import { Injectable } from '@angular/core';
import { Lobby } from '../domain/lobby';
import { SignalRService } from '../../../services/signal-r/signal-r.service';
import { TranslateService } from '@ngx-translate/core';
import { Button } from '../../engine/button.component';
import { Observable, Subscriber, Subscription } from 'rxjs';
import { GameDto } from '../../../typewriter/classes/GameDto';
import { sha256 } from 'js-sha256';

/**
 * Core Coinche game loading and orchestrator.
 */
@Injectable()
export class LobbyScene extends Phaser.Scene {
    private lobby: Lobby;
    private numberOfPlayersText: Phaser.GameObjects.Text;
    private searchGameButton: Button;
    private onNewGameSubscriber: Subscriber<GameDto> = new Subscriber<GameDto>();
    private isSearchingGame: boolean = false;
    private onNewPlayerToLobby: Subscription;
    private onNewGameStart: Subscription;

    constructor(private signalRService: SignalRService, private translateService: TranslateService) {
        super({ key: 'lobby' });

        this.lobby = new Lobby();
    }

    preload() {
    }
    create() {
        // plug to new player enter in lobby event
        this.onNewPlayerToLobby = this.signalRService.onNewPlayer.subscribe({
            next: (data) => this.addNewPlayer(data)
        });

        // create search game button
        this.searchGameButton = new Button(this, 780, 570, this.translateService.instant('game.lobby.searchGame'))
            .setOrigin(1, 1);
        this.searchGameButton.click
            .subscribe({
                next: (button) => {
                    // disable button after first click
                    if (!this.isSearchingGame) {
                        this.isSearchingGame = true;
                        button.setText(this.translateService.instant('game.lobby.searchingGame'));
                        this.searchGame();
                    }
                }
            });
        this.add.existing(this.searchGameButton);

        this.numberOfPlayersText = this.add.text(780, 20, '')
            .setOrigin(1, 0);

        this.addPlayer();

    }
    update() {
    }

    /**
     * Remove all signalr listeners.
     */
    public stopSubscriptions() {
        this.onNewPlayerToLobby.unsubscribe();
        this.onNewGameStart.unsubscribe();
    }

    /**
     * Observable that will emit value if a game is found for the current player.
     * Cannot fire before player clicked on this.searchGameButton.
     */
    public get onGameStarted(): Observable<GameDto> {
        return new Observable(observer => this.onNewGameSubscriber = observer);
    }

    private addPlayer() {
        this.lobby.addPlayer()
            .subscribe({
                next: (playerId) => this.signalRService.broadcastNewPlayer(playerId)
            });
    }

    private addNewPlayer(numebrOfPlayers: number) {
        console.log('new player to lobby');
        this.numberOfPlayersText.setText(this.translateService.instant('game.lobby.numberOfPlayers') + ' : ' + numebrOfPlayers);
    }

    private searchGame() {
        this.signalRService.broadcastSearchGame(this.lobby.playerId);
        this.onNewGameStart = this.signalRService.onGameStarted.subscribe({
            next: (data) => {
                if (data) {
                    const hasedPlayerId = sha256(this.lobby.playerId);

                    if (data.players.indexOf(hasedPlayerId) > -1) {
                        this.onNewGameSubscriber.next(data.id);
                    }
                }
            }
        });
    }
}
