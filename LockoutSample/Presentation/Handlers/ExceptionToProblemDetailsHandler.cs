using LockoutSample.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LockoutSample.Presentation.Handlers
{
    public class ExceptionToProblemDetailsHandler(IProblemDetailsService problemDetailsService)
        : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            int statusCode = exception switch
            {
                ValidationException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                ConflictException => StatusCodes.Status409Conflict,
                _ => 0
            };

            if (statusCode == 0)
            {
                return false;
            }

            string title = exception switch
            {
                InvalidCodeException => "Invalid code",
                LockCodeAlreadyExistsException => "Lock code already exists",
                LockCodeNotFoundException => "Lock code not found",
                OutOfAttemptsException => "Out of attempts",
                _ => "An error occured"
            };

            httpContext.Response.StatusCode = statusCode;

            var problemDetails = new ProblemDetails()
            {
                Title = title,
                Detail = exception.Message,
                Type = exception.GetType().Name
            };

            return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = problemDetails,
                Exception = exception
            });
        }
    }
}
