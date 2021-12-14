import { Component, OnInit, Injectable } from '@angular/core';
import Phaser from 'phaser';
import { MainScene } from './scenes/main.scene';

/**
 * Component holding the game canvas.
 */
@Component({
    selector: 'app-coinche',
    templateUrl: './coinche.component.html',
    styleUrls: ['./coinche.component.scss']
})
export class CoincheComponent implements OnInit {

    game!: Phaser.Game;
    gameConfig: Phaser.Types.Core.GameConfig;

    constructor(mainScene: MainScene) {
        this.gameConfig = {
            height: 900,
            width: 1600,
            type: Phaser.AUTO,
            parent: 'gameContainer',
            dom: {
                createContainer: true
            },
            scene: mainScene
        };
    }

    ngOnInit(): void {
        this.game = new Phaser.Game(this.gameConfig);
    }

}
