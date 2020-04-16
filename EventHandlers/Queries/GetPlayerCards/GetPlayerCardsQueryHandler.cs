using MediatR;
using Repositories.ReadEntities;
using Repositories.ReadRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventHandlers.Queries.GetPlayerCards
{
    public class GetPlayerCardsQueryHandler : IRequestHandler<GetPlayerCardsQuery>
    {
        private readonly IGenericRepository<CoincheGame> GameRepository;

        public GetPlayerCardsQueryHandler(IGenericRepository<CoincheGame> gameRepository)
        {
            GameRepository = gameRepository;
        }

        public Task<Unit> Handle(GetPlayerCardsQuery request, CancellationToken cancellationToken)
        {
            var game = GameRepository.Get(request.GameId);
        }
    }
}
