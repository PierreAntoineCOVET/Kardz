import { Observable, of } from 'rxjs';

export class Button extends Phaser.GameObjects.Text {
    constructor(scene: Phaser.Scene, x: number, y: number, text: string, options?: Phaser.Types.GameObjects.Text.TextStyle) {
        text = '< ' + text + ' >';
        super(scene, x, y, text, options);

        this.setInteractive({ useHandCursor: true })
            .on('pointerover', () => this.enterButtonHoverState())
            .on('pointerout', () => this.enterButtonRestState());
    }

    private enterButtonHoverState() {
        this.setStyle({ fill: '#0f0' });
    }

    private enterButtonRestState() {
        this.setStyle({ fill: '#fff' });
    }

    public get click(): Observable<any> {
        return new Observable(observer => {
            this.on('pointerup', () => observer.next(this));
        });
    }
}
