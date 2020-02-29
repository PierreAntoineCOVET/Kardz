using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class UnknownGameTypeException : Exception
    {
        public UnknownGameTypeException(string message) 
            : base(message)
        { }
    }
}
