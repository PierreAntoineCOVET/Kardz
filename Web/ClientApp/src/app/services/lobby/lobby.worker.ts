import { LobbyService } from './lobby.service';
import { Subscription } from 'rxjs';

const ctx: Worker = self as any;
let lobbyService: LobbyService;
let subscription = new Subscription();

ctx.addEventListener('message', (message: any) => {
    if (!message.data.fName) {
        throw new Error('Message does not contains the target function\'s name.');
    }

    if (message.data.fName == 'destroy') {
        subscription.unsubscribe();
        //close();
        return;
    }

    if (!message.data.playerId) {
        throw new Error('Message does not contains the player\'s id.');
    }

    if (!lobbyService) {
        lobbyService = new LobbyService(message.data.playerId);

        subscription.add(lobbyService.onGameStarted.subscribe((response) => {
            ctx.postMessage(response);
        }));

        subscription.add(lobbyService.onNewPlayer.subscribe((response) => {
            ctx.postMessage(response);
        }));
    }

    SendMyMessage(message.data);
});

function SendMyMessage(messageData: any) {
    switch (messageData.fName) {
        case 'broadcastNewPlayer':
            lobbyService.broadcastNewPlayer(messageData.playerId);
            break;

        case 'broadcastSearchGame':
            lobbyService.broadcastSearchGame(messageData.playerId);
            break;

        default:
            throw new Error(`Unsupported function ${messageData.fName}`);
    }
}
