using System;

namespace BL.Exceptions
{
    public class NoWaterException : Exception
    {
        public NoWaterException(string message, Exception innerException = null) : base(message, innerException)
        {
        }
    }
}