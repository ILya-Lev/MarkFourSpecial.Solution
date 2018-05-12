using System;

namespace BL.Exceptions
{
    public class NoFilterException : Exception
    {
        public NoFilterException(string message, Exception innerException = null) : base(message, innerException)
        {

        }
    }
}