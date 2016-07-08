using System;

namespace Stave.Exceptions
{
    public class InvalidChildException : Exception
    {
        public InvalidChildException(string message)
            : base(message)
        {
        }
    }
}