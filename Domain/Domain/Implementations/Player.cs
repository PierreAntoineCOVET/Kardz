using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Domain.Implementations
{
    public class Player
    {
        public Guid Id { get; private set; }

        public Player(Guid id)
        {
            this.Id = id;
        }
    }
}
