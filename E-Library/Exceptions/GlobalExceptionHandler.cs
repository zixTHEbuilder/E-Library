using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace E_Library.Exceptions
{
    internal sealed class GlobalExceptionHandler(
        IProblemDetailsService problemDetailsService,
        ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, "Error occurred");

            var statusCode = exception switch
            {
                FluentValidation.ValidationException => StatusCodes.Status400BadRequest,
                ApplicationException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            context.Response.StatusCode = statusCode;

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = statusCode == StatusCodes.Status400BadRequest ? "Validation Error" : "Server Error",
                Type = exception.GetType().Name
            };

            if (exception is ValidationException validationException)
            {
                problemDetails.Extensions["errors"] = validationException.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage));
            }
            else
            {
                problemDetails.Detail = exception.Message;
            }

            return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = context,
                Exception = exception,
                ProblemDetails = problemDetails
            });

        }
    }
}