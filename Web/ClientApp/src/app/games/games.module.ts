import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CoincheComponent } from './coinche/coinche.component';
import { HttpClientModule } from '@angular/common/http';
import { GameScene } from './coinche/scenes/game.scene';
import { MainScene } from './coinche/scenes/main.scene';
import { LobbyScene } from './coinche/scenes/lobby.scene';

/**
 * Module for all the games logic.
 */
@NgModule({
    declarations: [CoincheComponent],
    imports: [
        CommonModule,
        HttpClientModule,
    ],
    exports: [
        CoincheComponent
    ],
    providers: [
        GameScene,
        MainScene,
        LobbyScene
    ]
})
export class GamesModule { }
