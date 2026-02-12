using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using WidgetService.Data;
using WidgetService.Middleware;
using WidgetService.Models;
using static WidgetService.Middleware.ApiKeyMiddleware;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddScoped<WidgetService.Services.WidgetService>();
builder.Services.AddScoped<WidgetService.Services.UserService>();
builder.Services.AddScoped<WidgetService.Services.OrderService>();

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services
    .AddIdentityCore<ApplicationUser>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
    })
    .AddEntityFrameworkStores<AppDbContext>();

// Swagger setup
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // 1️⃣ Define the API key scheme (components)
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        Name = "X-Api-Key",
        In = ParameterLocation.Header,
        Description = "Enter your API key",
        Scheme = "ApiKey"
    });

    // 2️⃣ Only apply the scheme per operation via OperationFilter
    c.OperationFilter<AddApiKeyHeaderParameter>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // Disable default "Authorize" button
        c.ConfigObject.AdditionalItems["persistAuthorization"] = false;
    });
}

app.UseHttpsRedirection();
app.UseMiddleware<ApiKeyMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.Run();

