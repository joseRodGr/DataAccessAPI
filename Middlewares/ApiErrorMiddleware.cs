using DataAccessAPI.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccessAPI.Middlewares
{
    public class ApiErrorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiErrorMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ApiErrorMiddleware(RequestDelegate next, ILogger<ApiErrorMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var apiError = _env.IsDevelopment()
                    ? new ApiError(context.Response.StatusCode, ex.Message, context.TraceIdentifier, ex.StackTrace)
                    : new ApiError(context.Response.StatusCode, "Internal Server Error", context.TraceIdentifier);

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var jsonApiError = JsonSerializer.Serialize(apiError, options);
                await context.Response.WriteAsync(jsonApiError);
            }
        }
    }
}
