namespace InventoryManagement;

using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment env)
{
    private static readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception occurred");

            var problemDetails = new ProblemDetails
            {
                Title = "An unexpected error occurred",
                Status = (int)HttpStatusCode.InternalServerError,
                Instance = context.Request.Path,
                Detail = env.IsDevelopment() ? ex.ToString() : null,
                Type = "https://httpstatuses.com/500"
            };

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = problemDetails.Status.Value;

            var json = JsonSerializer.Serialize(problemDetails, _serializerOptions);
            await context.Response.WriteAsync(json);
        }
    }
}
