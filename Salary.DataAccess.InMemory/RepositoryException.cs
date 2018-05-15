using System;

namespace Salary.DataAccess.InMemory
{
    public class RepositoryException : Exception
    {
        public RepositoryException(string message, Exception innerException = null)
        : base(message, innerException)
        {

        }
    }
}
