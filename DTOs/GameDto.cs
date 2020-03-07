using System;
using System.Collections.Generic;

namespace DTOs
{
    public class GameDto
    {
        public Guid Id { get; set; }

        public List<string> Players { get; set; }
    }
}
