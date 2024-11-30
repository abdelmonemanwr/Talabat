using System.Net;
using System.Text.Json;

namespace Talabat.APIs.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;
        private readonly IHostEnvironment environment;

                                                                                            // StatusCode, msg, exceptions
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment environment)
        {
            this.next = next;
            this.logger = logger;
            this.environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException != null ? ex.InnerException.Message : string.Empty;
                logger.LogError(innerException);
                Console.WriteLine($"\n-----\nError: {ex.Message}, InnerException: {innerException}\n-----\n");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                
                object exceptionResponse = environment.IsDevelopment() ?
                    new { StatusCode = 500, ex.Message, Details = ex?.StackTrace?.ToString() } :
                    new { StatusCode = 500, ex.Message };

                var options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var json = JsonSerializer.Serialize(exceptionResponse, options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
