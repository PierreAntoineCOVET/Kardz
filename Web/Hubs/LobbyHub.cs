using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Hubs
{
    public class LobbyHub : Hub
    {
        public Task NewPlayer(Guid guid)
        {
            return Clients.All.SendAsync("newPlayer", guid);
        }
    }
}
