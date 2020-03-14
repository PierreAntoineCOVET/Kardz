import { v4 as uuidv4 } from 'uuid';

export class GameStartedEvent {
    public gameId: uuidv4;
    public playerId: uuidv4;
}
