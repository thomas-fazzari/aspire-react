using Scalar.AspNetCore;
using WeatherApp.Api;
using WeatherApp.Api.Shared.Constants;
using WeatherApp.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Logging.AddSimpleConsole(options =>
{
    options.IncludeScopes = true;
    options.TimestampFormat = Formats.LogTimestamp;
});

builder.Services.AddOpenApi();
builder.AddRedisOutputCache("redis");
builder.Services.AddWeatherAppApi(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler("/error");
app.Map(
    "/error",
    () =>
        Results.Problem(
            statusCode: StatusCodes.Status500InternalServerError,
            title: "An unexpected error occurred."
        )
);

app.UseOutputCache();
app.MapDefaultEndpoints();
app.MapOpenApi();
app.MapScalarApiReference(options => options.Title = "WeatherApp API");

if (app.Environment.IsDevelopment())
    app.MapGet("/", () => Results.Redirect("/scalar/v1")).ExcludeFromDescription();

app.MapWeatherAppApi();

app.Run();
