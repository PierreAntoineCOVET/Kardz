/**
 * Start a player turn's timer.
 */
export interface PlayerSaidEvent {
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

    /**
     * Define the bubble queue (arrow) position relative to the bubble.
     */
    bubbleQueuePosition: BubbleQueuePosition | null;
}

/**
 * Define the bubble queue (arrow) position relative to the bubble.
 */
export enum BubbleQueuePosition {
    bottomLeft = 0,
    topRight = 1,
    bottomRight = 2
}

