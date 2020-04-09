import { CardsEnum } from '../../../../typewriter/enums/CardsEnum.enum';

export class AddCardEvent {
    public x: number;
    public y: number;
    public elementName: string;
    public card: CardsEnum;
    public angle: number;
}
