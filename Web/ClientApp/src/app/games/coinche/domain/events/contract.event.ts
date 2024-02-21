import { CoincheCardColorsEnum } from "src/app/typewriter/enums/CardEnum.enum";

/**
 * Raised when the game contract has changed.
 */
export interface ContractMadeEvent {
    /**
     * Selected color for trumps.
     */
    selectedColor: CoincheCardColorsEnum;

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

    /**
     * True if the contract is closed.
     */
    isClosed: boolean;
}
