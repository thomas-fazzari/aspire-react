using Microsoft.EntityFrameworkCore;
using WeatherApp.Api.Infrastructure.Persistence;
using WeatherApp.Migrator;
using WeatherApp.ServiceDefaults;

var builder = Host.CreateApplicationBuilder(args);

builder.ConfigureOpenTelemetry();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("weatherdb"))
);

builder.Services.AddHostedService<MigrationWorker>();

var host = builder.Build();
host.Run();
