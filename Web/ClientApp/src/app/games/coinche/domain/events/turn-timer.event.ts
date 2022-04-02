import { ScreenCoordinate } from 'src/app/games/coinche/domain/PlayerPosition';

/**
 * Start a player turn's timer.
 */
export interface StartTurnTimerEvent {
    /**
     * X coordinate of the timer.
     */
    x: number;

    /**
     * Y coordinate of the timer.
     */
    y: number;

    /**
     * Loading bar width.
     */
    width: number;

    /**
     * Loading bar height.
     */
    height: number;

    /**
     * Direction for the fill.
     */
    direction: ScreenCoordinate;

    /**
     * Number of the player having the timer.
     */
    playerNumber: number;
}

/**
 * Player's turn time tick.
 */
export interface TurnTimerTickedEvent {
    /**
     * Timer's percent on complition.
     */
    percentage: number;
}
