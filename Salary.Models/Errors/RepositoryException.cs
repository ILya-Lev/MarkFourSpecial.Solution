using System;
using System.Net;

namespace Salary.Models.Errors
{
    public class RepositoryException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public RepositoryException(string message, Exception innerException = null)
        : base(message, innerException)
        {

        }
    }
}
