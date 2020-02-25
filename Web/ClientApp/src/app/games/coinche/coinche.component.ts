import { Component, OnInit } from '@angular/core';
import Phaser from 'phaser';

@Component({
    selector: 'app-coinche',
    templateUrl: './coinche.component.html',
    styleUrls: ['./coinche.component.scss']
})
export class CoincheComponent implements OnInit {

    game: Phaser.Game;
    gameConfig: Phaser.Types.Core.GameConfig;

    constructor() {
        this.gameConfig = {
            height: 600,
            width: 800,
            type: Phaser.AUTO,
            parent: 'gameContainer',
            scene: MainScene
        };
        this.game = new Phaser.Game(this.gameConfig);
    }

    ngOnInit(): void {
    }

}

class MainScene extends Phaser.Scene {
    private image: Phaser.GameObjects.Image;

    constructor() {
        super({ key: 'Game' });
    }

    create() {
        this.image = this.add.image(400, 300, 'logo');
    }
    preload() {
        this.load.image('logo', 'assets/img/phaser2.png');
    }
    update() {
        const key = this.input.keyboard.createCursorKeys();

        if (key.up.isDown) {
            this.image.y -= 5;
        }
        else if (key.down.isDown) {
            this.image.y += 5;
        }

        if (key.right.isDown) {
            this.image.x += 5;
        }
        else if (key.left.isDown) {
            this.image.x -= 5;
        }

        if (key.space.isDown) {
            this.image.x = 400;
            this.image.y = 300;
        }
    }
}
