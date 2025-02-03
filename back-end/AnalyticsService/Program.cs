using AnalyticsService.Services.Analytics;
using DefaultAnalyticsService = AnalyticsService.Services.Analytics.AnalyticsService;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(cors => cors.AddDefaultPolicy(policy => policy.AllowAnyHeader().AllowAnyOrigin()));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "AnalyticsService";
    config.Title = "AnalyticsService v1";
    config.Version = "v1";
});
builder.Services.AddDataProtection()
    .SetApplicationName("AnalyticsService");

builder.Services.AddSingleton<IAnalyticsDataAccess, LoggingAnalyticsDataAccess>();
builder.Services.AddSingleton<IAnalyticsService, DefaultAnalyticsService>();

var app = builder.Build();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "AnalyticsService";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.MapControllers();

app.Run();