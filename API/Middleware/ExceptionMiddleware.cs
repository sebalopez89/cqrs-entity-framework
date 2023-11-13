using CQRS.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace CQRS.Host.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            Console.WriteLine(exception);
            string result = JsonSerializer.Serialize(new ErrorDetails 
                { 
                    ErrorMessage = exception.Message, 
                    ErrorType = "Failure" 
                });

            switch (exception)
            {
                case BadRequestException badRequestException:
                    statusCode = HttpStatusCode.BadRequest;
                    break;
                case ValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(validationException.Errors);
                    break;
                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    break;
                default:
                    break;
            }

            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(result);
        }
    }

    public class ErrorDetails
    {
        public string ErrorType { get; set; }
        public string ErrorMessage { get; set; }
    }
}
