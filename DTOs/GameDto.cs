using System;
using System.Collections.Generic;

namespace DTOs
{
    public class GameDto
    {
        public Guid Id { get; set; }

        public List<Guid> PlayersId { get; set; }
    }
}
