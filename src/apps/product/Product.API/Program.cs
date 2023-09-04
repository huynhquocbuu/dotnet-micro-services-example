using Common.Logging;
using Product.API.Extensions;
using Product.API.Persistence;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();
var serviceName = "Product API";
var builder = WebApplication.CreateBuilder(args);
Log.Information($"Start {serviceName}-{builder.Environment.EnvironmentName} up");
try
{
    builder.Host.UseSerilog(SeriLogger.Configure);
    builder.Host.AddAppConfigurations();
    // Add services to the container.
    builder.Services.AddInfrastructure(builder.Configuration);

    var app = builder.Build();
    app.UseInfrastructure(app.Environment);

    app.MigrateDatabase<ProductDbContext>((context, _) =>
    {
        ProductDbContextSeed.SeedProductAsync(context, Log.Logger).Wait();
    });

    app.Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information($"Shutdown {serviceName}-{builder.Environment.EnvironmentName} complete");
    Log.CloseAndFlush();
}