using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.ReadEntities
{
    public class CoincheGame
    {
        public Guid Id { get; set; }

        public string CurrentCards { get; set; }

        public string LastShuffle { get; set; }

        public bool IsFinished { get; set; }

        public ICollection<CoincheTeam> Teams { get; set; }
    }
}
