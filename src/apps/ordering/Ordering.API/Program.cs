using Common.Logging;
using Infrastructure.Middlewares;
using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(SeriLogger.Configure);

Log.Information($"Start {builder.Environment.ApplicationName} up");

try
{
    // Add services to the container.
    builder.Host.AddAppConfigurations();
    builder.Services.AddConfigurationSettings(builder.Configuration);
    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.ConfigureMassTransit();

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName.Equals("Docker"))
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
            c.SwaggerEndpoint("/swagger/v1/swagger.json",
                "Swagger Order API v1"));
    }

    // Initialise and seed database
    using (var scope = app.Services.CreateScope())
    {
        var orderContextSeed = scope.ServiceProvider.GetRequiredService<OrderDbContextSeed>();
        await orderContextSeed.InitialiseAsync();
        await orderContextSeed.SeedAsync();
    }

    app.UseMiddleware<ErrorWrappingMiddleware>();
    
    // app.UseHttpsRedirection(); //production only
    app.UseRouting();

    app.UseAuthorization();

    app.MapControllers();

    app.MapDefaultControllerRoute();

    app.Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal)) throw;

    Log.Fatal(ex, $"Unhandled exception: {ex.Message}");
}
finally
{
    Log.Information($"Shut down {builder.Environment.ApplicationName} complete");
    Log.CloseAndFlush();
}