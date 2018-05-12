using System;

namespace BL.Exceptions
{
    public class NoCoffeeException : Exception
    {
        public NoCoffeeException(string message, Exception innerException = null) : base(message, innerException)
        {
        }
    }
}