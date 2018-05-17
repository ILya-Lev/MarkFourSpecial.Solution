using System;

namespace Salary.Models.Errors
{
    public class StrategyException : Exception
    {
        public StrategyException(string message, Exception innerException = null)
        : base(message, innerException)
        {

        }
    }
}
