import { ColorEnum } from 'src/app/typewriter/enums/ColorEnum.enum';

/**
 * Contract of the current player for the game.
 */
export interface ContractEvent {
    /**
     * Selected color for trumps.
     */
    selectedColor: ColorEnum;

    /**
     * Selected trumps value (170 = capot).
     */
    selectedValue: number;

    /**
     * Contract coinched / counter coinched
     */
    coinched: boolean;
}
