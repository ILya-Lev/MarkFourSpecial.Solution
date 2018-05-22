using System;
using System.Net;

namespace Salary.Models.Errors
{
    public class ValidationException : Exception
    {
        public HttpStatusCode StatusCode { get; } = HttpStatusCode.Conflict;
        public ValidationException(string message, Exception innerException = null)
            : base(message, innerException)
        {

        }
    }
}