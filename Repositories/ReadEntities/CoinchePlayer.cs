using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.ReadEntities
{
    public class CoinchePlayer
    {
        public Guid Id { get; set; }

        public string Cards { get; set; }

        public int Number { get; set; }

        public Guid TeamId { get; set; }

        public CoincheTeam Team { get; set; }
    }
}
