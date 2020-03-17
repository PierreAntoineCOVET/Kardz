import { Injectable } from '@angular/core';
import { Lobby } from '../domain/lobby';
import { LobbyService } from '../../../services/lobby/lobby.service';
import { TranslateService } from '@ngx-translate/core';
import { Button } from '../../engine/button.component';
import { Observable, Subscription } from 'rxjs';
import { GameStartedEvent } from '../domain/events/game-started.event';

/**
 * Core Coinche game loading and orchestrator.
 */
@Injectable()
export class LobbyScene extends Phaser.Scene {
    private lobby: Lobby;
    private numberOfPlayersText: Phaser.GameObjects.Text;
    private searchGameButton: Button;
    private isSearchingGame: boolean = false;
    private subscriptions: Subscription;
    private numberOfPlayersBaseText: string;

    constructor(lobbyService: LobbyService, private translateService: TranslateService) {
        super({ key: 'lobby' });

        this.lobby = new Lobby(lobbyService);
    }

    preload() {
    }
    create() {
        this.events.on('shutdown', () => this.onDestroy());

        this.numberOfPlayersBaseText = this.translateService.instant('game.lobby.numberOfPlayers') + ' : ';

        this.subscriptions = this.lobby.onNewPlayerToLobby().subscribe({
            next: (numebrOfPlayers) =>
                this.numberOfPlayersText.setText(this.numberOfPlayersBaseText + numebrOfPlayers)
        });

        // create search game button
        this.searchGameButton = new Button(this, 1540, 850, this.translateService.instant('game.lobby.searchGame'))
            .setOrigin(1, 1);
        this.subscriptions.add(this.searchGameButton.click
            .subscribe({
                next: (button) => {
                    // disable button after first click
                    if (!this.isSearchingGame) {
                        this.isSearchingGame = true;
                        button.setText(this.translateService.instant('game.lobby.searchingGame'));
                        this.lobby.searchGame();
                    }
                }
            }));
        this.add.existing(this.searchGameButton);

        this.numberOfPlayersText = this.add.text(1540, 50, '')
            .setOrigin(1, 0);

    }
    update() {
    }

    /**
     * Remove all signalr listeners.
     */
    private onDestroy() {
        this.lobby.onDestroy();
        this.subscriptions.unsubscribe();
    }

    /**
     * Observable that will emit value if a game is found for the current player.
     * Cannot fire before player clicked on this.searchGameButton.
     */
    public get onGameStarted(): Observable<GameStartedEvent> {
        return this.lobby.onNewGameSubscriber;
    }
}
