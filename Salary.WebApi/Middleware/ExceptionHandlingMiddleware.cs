using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Salary.Models.Errors;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Salary.WebApi.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _nextMiddleware;

        public ExceptionHandlingMiddleware(RequestDelegate nextMiddleware)
        {
            _nextMiddleware = nextMiddleware;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _nextMiddleware.Invoke(context);
            }
            catch (RepositoryException exc)
            {
                await HandleException(context, exc.Message, (int)exc.StatusCode);
            }
            catch (ValidationException exc)
            {
                await HandleException(context, exc.Message, (int)exc.StatusCode);
            }
            catch (StrategyException exc)
            {
                await HandleException(context, exc.Message, (int)HttpStatusCode.InternalServerError);
            }
            catch (Exception exc)
            {
                await HandleException(context, exc.Message, (int)HttpStatusCode.InternalServerError);
            }
        }

        private Task HandleException(HttpContext context, string message, int responseStatus)
        {
            context.Response.StatusCode = responseStatus;
            context.Response.ContentType = "application/json";
            var responseBody = JsonConvert.SerializeObject(message);
            return context.Response.WriteAsync(responseBody);
        }
    }
}