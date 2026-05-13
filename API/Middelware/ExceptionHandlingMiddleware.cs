using FluentValidation;

namespace API.Middelware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context, Exception exception)
        {
            var (statusCode, message) = exception switch
            {
                ValidationException ex => (
                    StatusCodes.Status400BadRequest,
                    ex.Errors.Select(e => e.ErrorMessage)),

                KeyNotFoundException => (
                    StatusCodes.Status404NotFound,
                    new[] { exception.Message }),

                InvalidOperationException => (
                    StatusCodes.Status400BadRequest,
                    new[] { exception.Message }),

                _ => (
                    StatusCodes.Status500InternalServerError,
                    new[] { "An unexpected error occurred" })
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new
            {
                status = statusCode,
                errors = message
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
