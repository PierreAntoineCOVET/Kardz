import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CoincheComponent } from './coinche/coinche.component';
import { HttpClientModule } from '@angular/common/http';
import { GameScene } from './coinche/scenes/game';
import { MainScene } from './coinche/scenes/main';
import { LobbyScene } from './coinche/scenes/lobby';

/**
 * Module for all the games logic.
 */
@NgModule({
    declarations: [CoincheComponent],
    imports: [
        CommonModule,
        HttpClientModule
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
