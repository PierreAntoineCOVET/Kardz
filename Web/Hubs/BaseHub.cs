using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace Web.Hubs
{
    public abstract class BaseHub : Hub
    {
        private IMemoryCache MemoryCache;

        public BaseHub(IMemoryCache memoryCache)
        {
            MemoryCache = memoryCache;
        }

        public IEnumerable<string> GetPlayerConnectionId(IEnumerable<Guid> playerIds)
        {
            var connectionIds = new List<string>();

            foreach (var playerId in playerIds)
            {
                var playerConnection = MemoryCache.Get<string>(playerId.ToString());

                if (playerConnection == null)
                {
                    throw new HubException($"Connection to client {playerId} lost");
                }
                else
                {
                    connectionIds.Add(playerConnection);
                }
            }

            return connectionIds;
        }

        public override Task OnConnectedAsync()
        {
            var accesToken = Context.GetHttpContext().Request.Query["access_token"];

            if(!Guid.TryParse(accesToken, out Guid playerId))
            {
                throw new AuthenticationException($"Invalid player id '{accesToken}'");
            }

            var connectionId = Context.ConnectionId;

            var storeConnectionId = MemoryCache.Get<string>(accesToken);

            if(string.IsNullOrWhiteSpace(storeConnectionId) || storeConnectionId != connectionId)
            {
                MemoryCache.Set(accesToken, connectionId, new MemoryCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(5) });
            }
            
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var accesToken = Context.GetHttpContext().Request.Query["access_token"];

            MemoryCache.Remove(accesToken);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
