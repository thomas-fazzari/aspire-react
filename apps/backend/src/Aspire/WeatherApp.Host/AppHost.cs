using Aspire.Hosting.Yarp.Transforms;

var builder = DistributedApplication.CreateBuilder(args);

var postgresPassword = builder.AddParameter("postgres-password", secret: true);
var openMeteoBaseUrl = builder.AddParameter("open-meteo-base-url");

var redis = builder.AddRedis("redis").WithLifetime(ContainerLifetime.Persistent);

var postgres = builder
    .AddPostgres("postgres", password: postgresPassword)
    .WithPgAdmin()
    .WithDataVolume(isReadOnly: false)
    .WithLifetime(ContainerLifetime.Persistent);

var db = postgres.AddDatabase("weatherdb");

var migrations = builder
    .AddProject<Projects.WeatherApp_Migrator>("migrations")
    .WithReference(db)
    .WaitFor(db);

var api = builder
    .AddProject<Projects.WeatherApp_Api>("api")
    .WithReference(db)
    .WithReference(redis)
    .WaitFor(db)
    .WaitFor(redis)
    .WaitForCompletion(migrations)
    .WithEnvironment("OpenMeteo__BaseUrl", openMeteoBaseUrl)
    .WithHttpHealthCheck("/health");

var frontendPath = Path.GetFullPath(Path.Combine(builder.AppHostDirectory, "../../../../frontend"));
var frontend = builder.AddViteApp("frontend", frontendPath).WithPnpm().WaitFor(api);

builder
    .AddYarp("gateway")
    .WithReference(api)
    .WithReference(frontend)
    .WaitFor(api)
    .WaitFor(frontend)
    .WithConfiguration(yarp =>
    {
        yarp.AddRoute("/api/{**catch-all}", api).WithTransformPathRemovePrefix("/api");
        yarp.AddRoute(frontend);
    });

builder.Build().Run();
