/**
 * Start a player turn's timer.
 */
export interface ContractSealedEvent {
    /**
     * X coordinate of the displayed bubble.
     */
    x: number;

    /**
     * Y coordinate of the displayed bubble.
     */
    y: number;

    /**
     * Width of the displayed bubble.
     */
    width: number;

    /**
     * Height of the displayed bubble.
     */
    height: number;

    /**
     * What needs to be said.
     */
    text: string;

    /**
     * Who said it.
     */
    playerNumber: number;
}
