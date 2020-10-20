import { GameService } from './game.service';
import { Subscription } from 'rxjs';

const ctx: Worker = self as any;
let gameService: GameService;
let subscription = new Subscription();

ctx.addEventListener('message', (message: any) => {
    if (!message.data.fName) {
        throw new Error('Message does not contains the target function\'s name.');
    }

    if (message.data.fName == 'destroy') {
        subscription.unsubscribe();
        close();
        return;
    }

    if (!message.data.playerId) {
        throw new Error('Message does not contains the player\'s id.');
    }

    if (!gameService) {
        gameService = new GameService(message.data.playerId);

        subscription.add(gameService.onGameInformationsReceived.subscribe({
            next: (response) => {
                ctx.postMessage(response);
            }
        }));

        subscription.add(gameService.onGameContractChanged.subscribe({
            next: (response) => {
                ctx.postMessage(response);
            }
        }));
    }

    SendMyMessage(message.data);
});

function SendMyMessage(messageData: any) {
    switch (messageData.fName) {
        case 'broadcastGetGameInformations':
            gameService.broadcastGetGameInformations(messageData.gameId, messageData.playerId);
            break;

        default:
            throw new Error(`Unsupported function ${messageData.fName}`);
    }   
}
