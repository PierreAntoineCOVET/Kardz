import { ColorEnum } from 'src/app/typewriter/enums/ColorEnum.enum';

/**
 * Raised when the current player need to vote on the game's contract.
 */
export interface ContractMadeEvent {
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

    /**
     * Number of the player currently playing.
     */
    currentPlayerNumber: number;
}
