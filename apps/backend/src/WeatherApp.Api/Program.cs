using Scalar.AspNetCore;
using WeatherApp.Api;
using WeatherApp.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Logging.AddSimpleConsole(options =>
{
    options.IncludeScopes = true;
    options.TimestampFormat = "HH:mm:ss ";
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
            title: "An unexpected error occurred.",
            statusCode: StatusCodes.Status500InternalServerError
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
