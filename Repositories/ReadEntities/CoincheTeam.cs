using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.ReadEntities
{
    public class CoincheTeam
    {
        public int Number { get; set; }

        public int Score { get; set; }

        public ICollection<CoinchePlayer> Players { get; set; }

        public Guid GameId { get; set; }

        public CoincheGame Game { get; set; }
    }
}
