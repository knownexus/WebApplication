using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;
using WidgetService.Models;
namespace WidgetService.Middleware;
public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _scopeFactory;

    public ApiKeyMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
    {
        _next = next;
        _scopeFactory = scopeFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Allow swagger through
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue("X-Api-Key", out var key))
        {
            await RejectRequest(context);
            return;
        }

        // ✅ Create a scope per request
        using var scope = _scopeFactory.CreateScope();
        var userService = scope.ServiceProvider
            .GetRequiredService<WidgetService.Services.UserService>();

        var apiKey = key.ToString();

        var isValid = await userService.CheckAuthKey(apiKey);

        if (!isValid)
        {
            await RejectRequest(context);
            return;
        }

        await _next(context);
    }

    private static async Task RejectRequest(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "application/json";

        var response = new UnauthorizedResponseModel();

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }

    public class AddApiKeyHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<IOpenApiParameter>();

            // Add the API key header to all endpoints
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "X-Api-Key",
                In = ParameterLocation.Header,
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = JsonSchemaType.String  // ✅ enum, not string literal
                },
                Description = "API key needed to access this endpoint"
            });
        }
    }

}