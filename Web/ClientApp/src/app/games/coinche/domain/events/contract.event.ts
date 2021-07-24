import { ColorEnum } from 'src/app/typewriter/enums/ColorEnum.enum';

/**
 * Contract of the current player for the game.
 */
export class ContractEvent {
    /**
     * Selected color for trumps.
     */
    public selectedColor: ColorEnum;

    /**
     * Selected trumps value (170 = capot).
     */
    public selectedValue: number;

    /**
     * Contract coinched / counter coinched
     */
    public coinched: boolean;
}
