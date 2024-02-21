import { CardsEnum } from 'src/app/typewriter/enums/CardEnum.enum';

export interface AddCardEvent {
    x: number;
    y: number;
    elementName: string;
    card: CardsEnum;
    angle: number;
}
