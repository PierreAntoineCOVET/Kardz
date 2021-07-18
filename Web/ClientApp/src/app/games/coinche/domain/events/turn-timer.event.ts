import { ScreenCoordinate } from 'src/app/games/coinche/domain/PlayerPosition';

/**
 * Start a player turn's timer.
 */
export class StartTurnTimerEvent {
    /**
     * X coordinate of the timer.
     */
    public x: number;

    /**
     * Y coordinate of the timer.
     */
    public y: number;

    /**
     * Loading bar width.
     */
    public width: number;

    /**
     * Loading bar height.
     */
    public height: number;

    /**
     * Direction for the fill.
     */
    public direction: ScreenCoordinate;
}

/**
 * Player's turn time tick.
 */
export class TurnTimerTickedEvent {
    /**
     * Timer's percent on complition.
     */
    public percentage: number;
}
